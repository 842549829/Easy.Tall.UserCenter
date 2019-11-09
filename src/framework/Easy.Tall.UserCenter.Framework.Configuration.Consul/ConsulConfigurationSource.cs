using Microsoft.Extensions.Configuration;

namespace Easy.Tall.UserCenter.Framework.Configuration.Consul
{
    /// <summary>
    /// consul配置数据源
    /// </summary>
    internal class ConsulConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// consul配置
        /// </summary>
        private ConsulAgentConfiguration Config { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">consul配置</param>
        public ConsulConfigurationSource(ConsulAgentConfiguration config)
        {
            Config = config;
        }

        /// <summary>
        /// 创建一个ConfigurationProvider
        /// </summary>
        /// <param name="builder">builder</param>
        /// <returns>IConfigurationProvider</returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            var provider = new ConsulConfigurationProvider(Config);
            ObserverManager.Attach(provider, Config);
            return provider;
        }
    }
}