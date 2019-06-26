using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using DotNetty.Codecs;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.Logging;
using Rabbit.Rpc.Messages;
using Rabbit.Rpc.Runtime.Server;
using Rabbit.Rpc.Transport;
using Rabbit.Rpc.Transport.Codec;
using Rabbit.Rpc.Transport.Implementation;
using Rabbit.Transport.DotNetty.Adaper;

namespace Rabbit.Transport.DotNetty
{
    /// <summary>
    /// 基于DotNetty的传输客户端工厂。
    /// </summary>
    public class DotNettyTransportClientFactory : ITransportClientFactory, IDisposable
    {
        /// <summary>
        /// 编码器
        /// </summary>
        private readonly ITransportMessageEncoder _transportMessageEncoder;

        /// <summary>
        /// 解码器
        /// </summary>
        private readonly ITransportMessageDecoder _transportMessageDecoder;

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<DotNettyTransportClientFactory> _logger;

        /// <summary>
        /// 服务执行者
        /// </summary>
        private readonly IServiceExecutor _serviceExecutor;

        /// <summary>
        /// 客户端链接
        /// </summary>
        private readonly ConcurrentDictionary<EndPoint, Lazy<ITransportClient>> _clients = new ConcurrentDictionary<EndPoint, Lazy<ITransportClient>>();

        /// <summary>
        /// 辅助程序
        /// </summary>
        private readonly Bootstrap _bootstrap;

        /// <summary>
        /// 消息发送者缓存
        /// </summary>
        private static readonly AttributeKey<IMessageSender> messageSenderKey = AttributeKey<IMessageSender>.ValueOf(typeof(DotNettyTransportClientFactory), nameof(IMessageSender));

        /// <summary>
        /// 消息监听者缓存
        /// </summary>
        private static readonly AttributeKey<IMessageListener> messageListenerKey = AttributeKey<IMessageListener>.ValueOf(typeof(DotNettyTransportClientFactory), nameof(IMessageListener));

        /// <summary>
        /// 终端缓存
        /// </summary>
        private static readonly AttributeKey<EndPoint> origEndPointKey = AttributeKey<EndPoint>.ValueOf(typeof(DotNettyTransportClientFactory), nameof(EndPoint));

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="codecFactory">编解码器</param>
        /// <param name="logger">日志记录器</param>
        public DotNettyTransportClientFactory(ITransportMessageCodecFactory codecFactory, ILogger<DotNettyTransportClientFactory> logger)
            : this(codecFactory, logger, null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="codecFactory">编解码器</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="serviceExecutor">服务执行者</param>
        public DotNettyTransportClientFactory(ITransportMessageCodecFactory codecFactory, ILogger<DotNettyTransportClientFactory> logger, IServiceExecutor serviceExecutor)
        {
            _transportMessageEncoder = codecFactory.GetEncoder();
            _transportMessageDecoder = codecFactory.GetDecoder();
            _logger = logger;
            _serviceExecutor = serviceExecutor;
            _bootstrap = GetBootstrap();
            _bootstrap.Handler(new ActionChannelInitializer<ISocketChannel>(c =>
            {
                var pipeline = c.Pipeline;
                pipeline.AddLast(new LengthFieldPrepender(4));
                pipeline.AddLast(new LengthFieldBasedFrameDecoder(int.MaxValue, 0, 4, 0, 4));
                pipeline.AddLast(new TransportMessageChannelHandlerAdapter(_transportMessageDecoder));
                pipeline.AddLast(new DefaultChannelHandler(this));
            }));
        }

        /// <summary>
        /// 创建客户端
        /// </summary>
        /// <param name="endPoint">终结点</param>
        /// <returns>传输客户端实例</returns>
        public ITransportClient CreateClient(EndPoint endPoint)
        {
            var key = endPoint;
            if (_logger.IsEnabled(LogLevel.Debug))
                _logger.LogDebug($"准备为服务端地址：{key}创建客户端。");
            try
            {
                return _clients.GetOrAdd(key
                    , k => new Lazy<ITransportClient>(() =>
                        {

                            var bootstrap = _bootstrap;
                            var channel = bootstrap.ConnectAsync(k).Result;

                            var messageListener = new MessageListener();
                            channel.GetAttribute(messageListenerKey).Set(messageListener);
                            var messageSender = new DotNettyMessageClientSender(_transportMessageEncoder, channel);
                            channel.GetAttribute(messageSenderKey).Set(messageSender);
                            channel.GetAttribute(origEndPointKey).Set(k);

                            var client = new TransportClient(messageSender, messageListener, _logger, _serviceExecutor);
                            return client;
                        }
                    )).Value;
            }
            catch
            {
                _clients.TryRemove(key, out var value);
                throw;
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            foreach (var client in _clients.Values.Where(i => i.IsValueCreated))
            {
                (client.Value as IDisposable)?.Dispose();
            }
        }

        /// <summary>
        /// 获取Bootstrap
        /// </summary>
        /// <returns>Bootstrap</returns>
        private static Bootstrap GetBootstrap()
        {
            var bootstrap = new Bootstrap();
            bootstrap
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Group(new MultithreadEventLoopGroup());

            return bootstrap;
        }

        /// <summary>
        /// 默认通道处理
        /// </summary>
        protected class DefaultChannelHandler : ChannelHandlerAdapter
        {
            /// <summary>
            ///客户端传输工厂
            /// </summary>
            private readonly DotNettyTransportClientFactory _factory;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="factory">客户端传输工厂</param>
            public DefaultChannelHandler(DotNettyTransportClientFactory factory)
            {
                _factory = factory;
            }

            /// <summary>
            /// 删除通道
            /// </summary>
            /// <param name="context">上下文</param>
            public override void ChannelInactive(IChannelHandlerContext context)
            {
                _factory._clients.TryRemove(context.Channel.GetAttribute(origEndPointKey).Get(), out _);
            }

            /// <summary>
            /// 通道消息读取
            /// </summary>
            /// <param name="context">上下文</param>
            /// <param name="message">消息</param>
            public override void ChannelRead(IChannelHandlerContext context, object message)
            {
                var transportMessage = message as TransportMessage;
                var messageListener = context.Channel.GetAttribute(messageListenerKey).Get();
                var messageSender = context.Channel.GetAttribute(messageSenderKey).Get();
                messageListener.OnReceived(messageSender, transportMessage);
            }
        }
    }
}