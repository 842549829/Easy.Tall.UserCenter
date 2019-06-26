using System;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.Logging;
using Rabbit.Rpc.Messages;
using Rabbit.Rpc.Transport;
using Rabbit.Rpc.Transport.Codec;
using Rabbit.Transport.DotNetty.Adaper;

namespace Rabbit.Transport.DotNetty
{
    /// <summary>
    /// 消息监听者
    /// </summary>
    public class DotNettyServerMessageListener : IMessageListener, IDisposable
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<DotNettyServerMessageListener> _logger;

        /// <summary>
        /// 编码器
        /// </summary>
        private readonly ITransportMessageEncoder _transportMessageEncoder;

        /// <summary>
        /// 解码器
        /// </summary>
        private readonly ITransportMessageDecoder _transportMessageDecoder;

        /// <summary>
        /// 通道
        /// </summary>
        private IChannel _channel;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志</param>
        /// <param name="codecFactory">传输消息编解码器工厂</param>
        public DotNettyServerMessageListener(ILogger<DotNettyServerMessageListener> logger, ITransportMessageCodecFactory codecFactory)
        {
            _logger = logger;
            _transportMessageEncoder = codecFactory.GetEncoder();
            _transportMessageDecoder = codecFactory.GetDecoder();
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Task.Run(async () =>
            {
                if (_channel != null)
                {
                    await _channel.DisconnectAsync();
                }
            }).Wait();
        }

        /// <summary>
        /// 接收到消息的事件
        /// </summary>
        public event ReceivedDelegate Received;

        /// <summary>
        /// 触发接收到消息事件
        /// </summary>
        /// <param name="sender">消息接收者</param>
        /// <param name="message">接收到的消息</param>
        /// <returns>一个任务</returns>
        public async Task OnReceived(IMessageSender sender, TransportMessage message)
        {
            if (Received == null)
            {
                return;
            }
            await Received(sender, message);
        }

        /// <summary>
        /// 开始服务
        /// </summary>
        /// <param name="endPoint">终结点</param>
        /// <returns>一个异步任务</returns>
        public async Task StartAsync(EndPoint endPoint)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug($"准备启动服务主机，监听地址：{endPoint}。");
            }
            var bossGroup = new MultithreadEventLoopGroup(1);
            var workerGroup = new MultithreadEventLoopGroup();
            var bootstrap = new ServerBootstrap();
            bootstrap
                .Group(bossGroup, workerGroup)
                .Channel<TcpServerSocketChannel>()
                .Option(ChannelOption.SoBacklog, 100)
                .ChildOption(ChannelOption.Allocator, UnpooledByteBufferAllocator.Default)
                .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    var pipeline = channel.Pipeline;
                    pipeline.AddLast(new LengthFieldPrepender(4));
                    pipeline.AddLast(new LengthFieldBasedFrameDecoder(int.MaxValue, 0, 4, 0, 4));
                    pipeline.AddLast(new TransportMessageChannelHandlerAdapter(_transportMessageDecoder));
                    pipeline.AddLast(new ServerHandler(async (context, message) =>
                    {
                        var sender = new DotNettyMessageServerSender(_transportMessageEncoder, context);
                        await OnReceived(sender, message);
                    }, _logger));
                }));

            _channel = await bootstrap.BindAsync(endPoint);
            _logger.LogInformation($"服务主机启动成功，监听地址：{endPoint}。");
        }

        /// <summary>
        /// 服务处理
        /// </summary>
        private class ServerHandler : ChannelHandlerAdapter
        {
            /// <summary>
            /// 消息委托
            /// </summary>
            private readonly Action<IChannelHandlerContext, TransportMessage> _readAction;

            /// <summary>
            /// 日志
            /// </summary>
            private readonly ILogger _logger;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="readAction">消息委托</param>
            /// <param name="logger">日志</param>
            public ServerHandler(Action<IChannelHandlerContext, TransportMessage> readAction, ILogger logger)
            {
                _readAction = readAction;
                _logger = logger;
            }

            /// <summary>
            /// 读取消息
            /// </summary>
            /// <param name="context">上下文</param>
            /// <param name="message">消息</param>
            public override void ChannelRead(IChannelHandlerContext context, object message)
            {
                Task.Run(() =>
                {
                    var transportMessage = (TransportMessage)message;

                    _readAction(context, transportMessage);
                });
            }

            /// <summary>
            /// 读取成功
            /// </summary>
            /// <param name="context">上下文</param>
            public override void ChannelReadComplete(IChannelHandlerContext context)
            {
                context.Flush();
            }

            /// <summary>
            /// 错误
            /// </summary>
            /// <param name="context">上下文</param>
            /// <param name="exception">异常</param>
            public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
            {
                _logger.LogWarning(exception, $"与服务器：{context.Channel.RemoteAddress}通信时发生了错误。");
            }
        }
    }
}