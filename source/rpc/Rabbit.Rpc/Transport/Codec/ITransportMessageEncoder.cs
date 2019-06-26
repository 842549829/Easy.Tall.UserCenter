using Rabbit.Rpc.Messages;

namespace Rabbit.Rpc.Transport.Codec
{
    /// <summary>
    /// 传输消息编码器
    /// </summary>
    public interface ITransportMessageEncoder
    {
        /// <summary>
        /// 编码器
        /// </summary>
        /// <param name="message">传输消息模型</param>
        /// <returns>二进制</returns>
        byte[] Encode(TransportMessage message);
    }
}