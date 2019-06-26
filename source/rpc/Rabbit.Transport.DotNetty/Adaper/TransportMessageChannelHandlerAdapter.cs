using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using Rabbit.Rpc.Transport.Codec;

namespace Rabbit.Transport.DotNetty.Adaper
{
    /// <summary>
    /// 消息传输通道处理适配器
    /// </summary>
    public class TransportMessageChannelHandlerAdapter : ChannelHandlerAdapter
    {
        /// <summary>
        /// 消息传输解码器
        /// </summary>
        private readonly ITransportMessageDecoder _transportMessageDecoder;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="transportMessageDecoder">消息传输解码器</param>
        public TransportMessageChannelHandlerAdapter(ITransportMessageDecoder transportMessageDecoder)
        {
            _transportMessageDecoder = transportMessageDecoder;
        }

        /// <summary>
        /// 读取消息通道
        /// </summary>
        /// <param name="context">通道处理当前上下文</param>
        /// <param name="message">消息</param>
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var buffer = (IByteBuffer)message;
            var data = new byte[buffer.Capacity];
            buffer.ReadBytes(data);
            var transportMessage = _transportMessageDecoder.Decode(data);
            context.FireChannelRead(transportMessage);
            buffer.Release();
        }
    }
}