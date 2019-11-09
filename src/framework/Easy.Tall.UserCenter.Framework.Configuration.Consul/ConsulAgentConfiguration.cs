using Consul;

namespace Easy.Tall.UserCenter.Framework.Configuration.Consul
{
    /// <summary>
    /// consul配置
    /// </summary>
    public class ConsulAgentConfiguration
    {
        /// <summary>
        /// consul客户端配置
        /// </summary>
        public ConsulClientConfiguration ClientConfiguration { get; set; }

        /// <summary>
        /// consul筛选范围
        /// </summary>
        public ConsulQueryOptions QueryOptions { get; set; }
    }
}