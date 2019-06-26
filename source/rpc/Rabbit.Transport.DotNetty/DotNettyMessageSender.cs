using DotNetty.Buffers;
using Rabbit.Rpc.Messages;
using Rabbit.Rpc.Transport.Codec;

namespace Rabbit.Transport.DotNetty
{
    /// <summary>
    /// 基于DotNetty的消息发送者基类。
    /// </summary>
    public abstract class DotNettyMessageSender
    {
        /// <summary>
        /// 传输消息编码器
        /// </summary>
        private readonly ITransportMessageEncoder _transportMessageEncoder;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="transportMessageEncoder">传输消息编码器</param>
        protected DotNettyMessageSender(ITransportMessageEncoder transportMessageEncoder)
        {
            _transportMessageEncoder = transportMessageEncoder;
        }

        /// <summary>
        /// 获取数据buffer
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>buffer</returns>
        protected IByteBuffer GetByteBuffer(TransportMessage message)
        {
            var data = _transportMessageEncoder.Encode(message);

            var buffer = Unpooled.Buffer(data.Length, data.Length);
            return buffer.WriteBytes(data);
        }
    }
}