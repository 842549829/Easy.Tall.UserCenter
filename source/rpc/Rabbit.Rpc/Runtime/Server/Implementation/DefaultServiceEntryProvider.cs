using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Rabbit.Rpc.Runtime.Server.Implementation.ServiceDiscovery;

namespace Rabbit.Rpc.Runtime.Server.Implementation
{
    /// <summary>
    /// 服务条目提供程序
    /// </summary>
    public class DefaultServiceEntryProvider : IServiceEntryProvider
    {
        /// <summary>
        /// 服务类型
        /// </summary>
        private readonly IDictionary<Type, Type> _serviceTypeMap;

        /// <summary>
        /// 服务条目工厂
        /// </summary>
        private readonly IClrServiceEntryFactory _clrServiceEntryFactory;

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<DefaultServiceEntryProvider> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceTypeMap">服务类型</param>
        /// <param name="clrServiceEntryFactory">服务条目工厂</param>
        /// <param name="logger">日志</param>
        public DefaultServiceEntryProvider(
            IDictionary<Type, Type> serviceTypeMap,
            IClrServiceEntryFactory clrServiceEntryFactory,
            ILogger<DefaultServiceEntryProvider> logger)
        {
            _clrServiceEntryFactory = clrServiceEntryFactory;
            _serviceTypeMap = serviceTypeMap;
            _logger = logger;
        }

        /// <summary>
        /// 获取服务条目集合
        /// </summary>
        /// <returns>服务条目集合</returns>
        public IEnumerable<ServiceEntry> GetEntries()
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation($"发现了以下服务：{string.Join(",", _serviceTypeMap.Select(i => i.ToString()))}。");
            }

            var entries = new List<ServiceEntry>();
            foreach (var service in _serviceTypeMap)
            {
                entries.AddRange(_clrServiceEntryFactory.CreateServiceEntry(service.Key, service.Value));
            }
            return entries;
        }
    }
}