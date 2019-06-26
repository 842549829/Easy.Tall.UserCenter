using System.Collections.Generic;

namespace Framework.Rpc
{
    /// <summary>
    /// Consul配置信息
    /// </summary>
    public class ServiceDiscoveryOptions
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务Id
        /// </summary>
        public string ServiceId { get; set; }

        /// <summary>
        /// 服务版号
        /// </summary>
        public string ServiceVersion { get; set; }

        /// <summary>
        /// 为每个请求提供数据中心。如果没有提供，则使用默认代理数据中心。
        /// </summary>
        public string DataCenter { get; set; }

        /// <summary>
        /// 健康检查间隔时间(单位秒)
        /// </summary>
        public int HealthCheckSeconds { get; set; }

        /// <summary>
        /// 路由列表更新间隔时间(单位秒)
        /// </summary>
        public int ChangeCheckSeconds { get; set; } = 10;

        /// <summary>
        /// 检查处于临界状态的值超过此配置值，然后它的关联服务(及其所有关联检查)将自动注销。(单位秒) {默认30秒}
        /// </summary>
        public int CriticalDeregisterSeconds { get; set; } = 30;

        /// <summary>
        /// 服务监听地址
        /// </summary>
        public string ListenAddress { get; set; }

        /// <summary>
        /// 服务监听端口
        /// </summary>
        public int ListenPort { get; set; }

        /// <summary>
        /// 客户端消费的服务名称、服务版本
        /// </summary>
        public Dictionary<string, string> RemoteServices { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 负载均衡权重值
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// Consul终端
        /// </summary>
        public RpcEndpoint EndPoint { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
    }
}
