using System;
using System.Collections.Generic;
using System.Reflection;
using Rabbit.Rpc.Ids;

namespace Rabbit.Rpc.Runtime.Client.Implementation.ServiceDiscovery.Implementation
{
    /// <summary>
    /// 客户端服务条目
    /// </summary>
    public class CrlClientEntryFactory : IClrClientEntryFactory
    {
        /// <summary>
        /// 服务Id生成器
        /// </summary>
        private readonly IServiceIdGenerator _serviceIdGenerator;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceIdGenerator">服务Id生成器</param>
        public CrlClientEntryFactory(IServiceIdGenerator serviceIdGenerator)
        {
            _serviceIdGenerator = serviceIdGenerator;
        }

        /// <summary>
        /// 创建服务条目。
        /// </summary>
        /// <param name="service">服务类型。</param>
        /// <returns>服务条目集合。</returns>
        public IEnumerable<ServiceEntry> CreateServiceEntry(Type service)
        {
            foreach (var methodInfo in service.GetTypeInfo().GetMethods())
            {
                yield return Create(methodInfo);
            }
        }

        /// <summary>
        /// 创建服务条目
        /// </summary>
        /// <param name="method">服务类型</param>
        /// <returns>服务条目</returns>
        private ServiceEntry Create(MethodInfo method)
        {
            var serviceId = _serviceIdGenerator.GenerateServiceId(method);
            var type = method.DeclaringType;
            if (type == null)
            {
                throw new ArgumentNullException(nameof(method.DeclaringType), "方法的定义类型不能为空。");
            }
            var serviceDescriptor = new ServiceDescriptor
            {
                Id = serviceId,
                Metadata = new Dictionary<string, object> { { "namespace", type.AssemblyQualifiedName?.Split(',')[1].Trim() } }
            };
            return new ServiceEntry
            {
                Func = null,
                Descriptor = serviceDescriptor
            };
        }
    }
}