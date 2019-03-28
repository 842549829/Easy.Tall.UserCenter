using System.Collections.Generic;

namespace Rabbit.Rpc.Messages
{
    /// <summary>
    /// 远程调用消息。
    /// </summary>
    public class RemoteInvokeMessage
    {
        /// <summary>
        /// 服务Id。
        /// </summary>
        public string ServiceId { get; set; }

        /// <summary>
        /// 为有状态的服务提供上下文
        /// </summary>
        public object RpcContext { get; set; }

        /// <summary>
        /// 服务参数。
        /// </summary>
        public IDictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// 泛型参数
        /// </summary>
        public IEnumerable<string> GenericParameters { get; set; }
    }
}