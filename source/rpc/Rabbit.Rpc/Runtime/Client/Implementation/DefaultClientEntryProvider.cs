using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using Rabbit.Rpc.Runtime.Client.Implementation.ServiceDiscovery;

namespace Rabbit.Rpc.Runtime.Client.Implementation
{
    /// <summary>
    /// 一个默认的客户端服务条目提供程序
    /// </summary>
    public class DefaultClientEntryProvider : IClientEntryProvider
    {
        /// <summary>
        /// 服务类型
        /// </summary>
        private readonly IEnumerable<Type> _serviceTypes;

        /// <summary>
        /// 服务条目工厂
        /// </summary>
        private readonly IClrClientEntryFactory _clrClientEntryFactory;

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<DefaultClientEntryProvider> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceTypes">服务类型</param>
        /// <param name="clrClientEntryFactory">服务条目工厂</param>
        /// <param name="logger">日志</param>
        public DefaultClientEntryProvider(
            IEnumerable<Type> serviceTypes,
            IClrClientEntryFactory clrClientEntryFactory,
            ILogger<DefaultClientEntryProvider> logger)
        {
            _clrClientEntryFactory = clrClientEntryFactory;
            _serviceTypes = serviceTypes;
            _logger = logger;
        }

        /// <summary>
        /// 获取客户端服务条目
        /// </summary>
        /// <returns>客户端服务条目</returns>
        public IEnumerable<ServiceEntry> GetEntries()
        {
#if NET
            var assemblys = AppDomain.CurrentDomain.GetAssemblies();
#else
            var assemblies = DependencyContext.Default.RuntimeLibraries.SelectMany(i => i.GetDefaultAssemblyNames(DependencyContext.Default).Select(z => Assembly.Load(new AssemblyName(z.Name))));
#endif
            var types = assemblies.Where(i => !i.IsDynamic).SelectMany(i => i.ExportedTypes).ToArray();
            var services = types.Where(i =>
            {
                var typeInfo = i.GetTypeInfo();
                return typeInfo.IsInterface && _serviceTypes.Any(d => d.FullName == i.FullName);
            }).ToArray();
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation($"发现了以下服务：{string.Join(",", services.Select(i => i.ToString()))}。");
            }

            var entries = new List<ServiceEntry>();
            foreach (var service in services)
            {
                entries.AddRange(_clrClientEntryFactory.CreateServiceEntry(service));
            }
            return entries;
        }
    }
}