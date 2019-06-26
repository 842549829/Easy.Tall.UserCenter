using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;

namespace Rabbit.Rpc.Runtime.Server.Implementation.ServiceDiscovery.Attributes
{
    /// <summary>
    /// Service标记类型的服务条目提供程序。
    /// </summary>
    public class AttributeServiceEntryProvider : IServiceEntryProvider
    {
        /// <summary>
        /// 服务发现特性类型
        /// </summary>
        private readonly Type _attributeType;

        /// <summary>
        /// Clr服务条目工厂
        /// </summary>
        private readonly IClrServiceEntryFactory _clrServiceEntryFactory;

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<AttributeServiceEntryProvider> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="attributeType">服务发现特性类型</param>
        /// <param name="clrServiceEntryFactory">Clr服务条目工厂</param>
        /// <param name="logger">日志记录器</param>
        public AttributeServiceEntryProvider(Type attributeType, IClrServiceEntryFactory clrServiceEntryFactory, ILogger<AttributeServiceEntryProvider> logger)
        {
            _attributeType = attributeType;
            _clrServiceEntryFactory = clrServiceEntryFactory;
            _logger = logger;
        }

        /// <summary>
        /// 获取服务条目集合。
        /// </summary>
        /// <returns>服务条目集合。</returns>
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
                return typeInfo.IsInterface && typeInfo.GetCustomAttribute(_attributeType) != null;
            }).ToArray();
            var serviceImplementations = types.Where(i =>
            {
                var typeInfo = i.GetTypeInfo();
                return typeInfo.IsClass && !typeInfo.IsAbstract && i.Namespace != null && !i.Namespace.StartsWith("System") &&
                !i.Namespace.StartsWith("Microsoft");
            }).ToArray();

            _logger.LogInformation($"发现了以下服务：\r\n{string.Join("\r\n", services.Select(i => i.ToString()))}");

            var entries = new List<ServiceEntry>();
            foreach (var service in services)
            {
                foreach (var serviceImplementation in serviceImplementations.Where(i => service.GetTypeInfo().IsAssignableFrom(i)))
                {
                    entries.AddRange(_clrServiceEntryFactory.CreateServiceEntry(service, serviceImplementation));
                }
            }
            return entries;
        }
    }
}