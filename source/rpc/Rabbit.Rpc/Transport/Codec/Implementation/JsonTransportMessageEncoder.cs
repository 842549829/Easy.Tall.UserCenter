using Newtonsoft.Json;
using Rabbit.Rpc.Messages;
using System.Text;

namespace Rabbit.Rpc.Transport.Codec.Implementation
{
    /// <summary>
    /// 传输消息编码器
    /// </summary>
    public sealed class JsonTransportMessageEncoder : ITransportMessageEncoder
    {
        /// <summary>
        /// 编码器
        /// </summary>
        /// <param name="message">传输消息模型</param>
        /// <returns>二进制</returns>
        public byte[] Encode(TransportMessage message)
        {
            var content = JsonConvert.SerializeObject(message);
            return Encoding.UTF8.GetBytes(content);
        }
    }
}