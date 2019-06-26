using Newtonsoft.Json;
using Rabbit.Rpc.Messages;
using System.Text;

namespace Rabbit.Rpc.Transport.Codec.Implementation
{
    /// <summary>
    /// 传输消息解码器
    /// </summary>
    public sealed class JsonTransportMessageDecoder : ITransportMessageDecoder
    {
        /// <summary>
        /// 解码器
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>传输信息模型</returns>
        public TransportMessage Decode(byte[] data)
        {
            var content = Encoding.UTF8.GetString(data);
            var message = JsonConvert.DeserializeObject<TransportMessage>(content);
            if (message.IsInvokeMessage())
            {
                message.Content = JsonConvert.DeserializeObject<RemoteInvokeMessage>(message.Content.ToString());
            }
            if (message.IsInvokeResultMessage())
            {
                message.Content = JsonConvert.DeserializeObject<RemoteInvokeResultMessage>(message.Content.ToString());
            }
            return message;
        }
    }
}