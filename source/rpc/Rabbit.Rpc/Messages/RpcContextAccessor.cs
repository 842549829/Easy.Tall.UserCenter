namespace Rabbit.Rpc.Messages
{
    /// <summary>
    /// 有状态服务上下文获取器
    /// </summary>
    public class RpcContextAccessor : IRpcContextAccessor
    {
        /// <summary>
        /// 服务上下文
        /// </summary>
        public object RpcContext { get; set; }
    }
}