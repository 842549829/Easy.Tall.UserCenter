using System.Collections.Generic;
using Consul;
using Microsoft.Extensions.Logging;

namespace Easy.Tall.UserCenter.Framework.Configuration.Consul
{
    /// <summary>
    /// 插件管理
    /// </summary>
    internal static class ObserverManager
    {
        /// <summary>
        /// 插件接口
        /// </summary>
        private static IObserver Observer { get; set; }

        /// <summary>
        /// 配置
        /// </summary>
        public static ConsulAgentConfiguration Configuration { get; private set; }

        /// <summary>
        /// 附加插件
        /// </summary>
        /// <param name="observer">插件</param>
        /// <param name="configuration">配置</param>
        public static void Attach(IObserver observer, ConsulAgentConfiguration configuration)
        {
            Observer = observer;
            Configuration = configuration;
        }

        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="kVPairs">kv</param>
        /// <param name="logger">日志</param>
        public static void Notify(List<KVPair> kVPairs, ILogger logger)
        {
            Observer.OnChange(kVPairs, logger);
        }
    }
}