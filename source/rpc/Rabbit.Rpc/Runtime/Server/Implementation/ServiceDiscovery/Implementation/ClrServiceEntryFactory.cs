using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rabbit.Rpc.Convertibles;
using Rabbit.Rpc.Ids;
using Rabbit.Rpc.Messages;

namespace Rabbit.Rpc.Runtime.Server.Implementation.ServiceDiscovery.Implementation
{
    /// <summary>
    /// Clr服务条目工厂。
    /// </summary>
    public class ClrServiceEntryFactory : IClrServiceEntryFactory
    {
        /// <summary>
        /// 容器服务提供程序集
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 服务Id生成器
        /// </summary>
        private readonly IServiceIdGenerator _serviceIdGenerator;

        /// <summary>
        /// 类型转换服务
        /// </summary>
        private readonly ITypeConvertibleService _typeConvertibleService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceProvider">容器服务提供程序集</param>
        /// <param name="serviceIdGenerator">服务Id生成器</param>
        /// <param name="typeConvertibleService">类型转换服务</param>
        public ClrServiceEntryFactory(IServiceProvider serviceProvider, IServiceIdGenerator serviceIdGenerator, ITypeConvertibleService typeConvertibleService)
        {
            _serviceProvider = serviceProvider;
            _serviceIdGenerator = serviceIdGenerator;
            _typeConvertibleService = typeConvertibleService;
        }

        /// <summary>
        /// 创建服务条目。
        /// </summary>
        /// <param name="service">服务类型。</param>
        /// <param name="serviceImplementation">服务实现类型。</param>
        /// <returns>服务条目集合。</returns>
        public IEnumerable<ServiceEntry> CreateServiceEntry(Type service, Type serviceImplementation)
        {
            foreach (var methodInfo in service.GetTypeInfo().GetMethods())
            {
                var implementationMethodInfo = serviceImplementation.GetMethods().First(d => d.ToString() == methodInfo.ToString());
                yield return Create(methodInfo, implementationMethodInfo);
            }
        }

        /// <summary>
        /// 创建服务条目
        /// </summary>
        /// <param name="method">服务类型</param>
        /// <param name="implementationMethod">服务实现类型</param>
        /// <returns>服务条目</returns>
        private ServiceEntry Create(MethodInfo method, MethodInfo implementationMethod)
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
                Descriptor = serviceDescriptor,
                Func = (invokeMessage, serviceScope) =>
                {
                    var serviceContextAccessor = serviceScope.ServiceProvider.GetService<IRpcContextAccessor>();
                    if (serviceContextAccessor != null)
                    {
                        serviceContextAccessor.RpcContext = invokeMessage.RpcContext;
                    }

                    var instance = serviceScope.ServiceProvider.GetRequiredService(method.DeclaringType);
                    var clientParameters = invokeMessage.Parameters;
                    var genericMethod = implementationMethod;
                    if (invokeMessage.GenericParameters?.Count() > 0)
                    {
                        genericMethod = implementationMethod.MakeGenericMethod(invokeMessage.GenericParameters.Select(Type.GetType).ToArray());
                    }

                    var serviceParameters = genericMethod.GetParameters();
                    var parameterList = serviceParameters.Select(d => _typeConvertibleService.Convert(clientParameters[d.Name], d.ParameterType));
                    return Task.FromResult(genericMethod.Invoke(instance, parameterList.ToArray()));
                }
            };
        }
    }
}