using System.Collections.Generic;
using Consul;
using Microsoft.Extensions.Logging;

namespace Easy.Tall.UserCenter.Framework.Configuration.Consul
{
    /// <summary>
    /// consul配置插件接口
    /// </summary>
    internal interface IObserver
    {
        /// <summary>
        /// 配置更改
        /// </summary>
        /// <param name="kvPairs">keyValue</param>
        /// <param name="logger">日志</param>
        void OnChange(List<KVPair> kvPairs, ILogger logger);
    }
}