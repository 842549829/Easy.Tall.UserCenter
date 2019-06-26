namespace Rabbit.Rpc.Messages
{
    /// <summary>
    /// 有状态服务上下文获取器
    /// </summary>
    public interface IRpcContextAccessor
    {
        /// <summary>
        /// 服务上下文
        /// </summary>
        object RpcContext { get; set; }
    }
}