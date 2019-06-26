using Rabbit.Rpc.Messages;

namespace Rabbit.Rpc.Transport.Codec
{
    /// <summary>
    /// 传输消息解码器
    /// </summary>
    public interface ITransportMessageDecoder
    {
        /// <summary>
        /// 解码器
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>传输信息模型</returns>
        TransportMessage Decode(byte[] data);
    }
}