namespace Framework.Rpc
{
    /// <summary>
    /// Consul配置信息提供接口
    /// </summary>
    public interface IServiceDiscoveryOptionProvider
    {
        /// <summary>
        /// Consul配置信息
        /// </summary>
        ServiceDiscoveryOptions Option { get; }
    }
}