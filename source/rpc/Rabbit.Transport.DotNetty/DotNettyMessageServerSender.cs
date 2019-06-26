using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Rabbit.Rpc.Messages;
using Rabbit.Rpc.Transport;
using Rabbit.Rpc.Transport.Codec;

namespace Rabbit.Transport.DotNetty
{
    /// <summary>
    /// 基于DotNetty服务端的消息发送者。
    /// </summary>
    public class DotNettyMessageServerSender : DotNettyMessageSender, IMessageSender, IDisposable
    {
        /// <summary>
        /// 通道处理上下文
        /// </summary>
        private readonly IChannelHandlerContext _context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="transportMessageEncoder">传输消息编码器</param>
        /// <param name="context">通道处理上下文</param>
        public DotNettyMessageServerSender(ITransportMessageEncoder transportMessageEncoder, IChannelHandlerContext context) : base(transportMessageEncoder)
        {
            _context = context;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Task.Run(async () =>
            {
                await _context.DisconnectAsync();
            }).Wait();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <returns>一个任务</returns>
        public Task SendAsync(TransportMessage message)
        {
            var buffer = GetByteBuffer(message);
            return _context.WriteAsync(buffer);
        }

        /// <summary>
        /// 发送消息并清空缓冲区
        /// </summary>
        /// <param name="message">小心内容</param>
        /// <returns>一个任务</returns>
        public Task SendAndFlushAsync(TransportMessage message)
        {
            var buffer = GetByteBuffer(message);
            return _context.WriteAndFlushAsync(buffer);
        }
    }
}