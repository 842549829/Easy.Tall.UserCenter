using System;
using System.Linq;
using System.Reflection;
using Rabbit.Rpc.Convertibles;
using Rabbit.Rpc.Runtime.Client;

namespace Rabbit.Rpc.ProxyGenerator.Implementation
{
    /// <summary>
    /// 默认的服务代理工厂实现
    /// </summary>
    public class ServiceProxyFactory : IServiceProxyFactory
    {
        /// <summary>
        /// 远程调用服务
        /// </summary>
        private readonly IRemoteInvokeService _remoteInvokeService;

        /// <summary>
        /// 类型转换服务
        /// </summary>
        private readonly ITypeConvertibleService _typeConvertibleService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="remoteInvokeService">远程调用服务</param>
        /// <param name="typeConvertibleService">类型转换服务</param>
        public ServiceProxyFactory(IRemoteInvokeService remoteInvokeService, ITypeConvertibleService typeConvertibleService)
        {
            _remoteInvokeService = remoteInvokeService;
            _typeConvertibleService = typeConvertibleService;
        }

        /// <summary>
        /// 创建服务代理。
        /// </summary>
        /// <param name="proxyType">代理类型。</param>
        /// <returns>服务代理实例。</returns>
        public object CreateProxy(Type proxyType)
        {
            var instance = proxyType.GetTypeInfo().GetConstructors().First().Invoke(new object[] { _remoteInvokeService, _typeConvertibleService, null });
            return instance;
        }

        /// <summary>
        /// 创建服务代理。
        /// </summary>
        /// <param name="proxyType">代理类型。</param>
        /// <param name="serviceContext">服务上下文。</param>
        /// <returns>服务代理实例。</returns>
        public object CreateProxy(Type proxyType, object serviceContext)
        {
            object instance = proxyType.GetTypeInfo().GetConstructors().First().Invoke(new [] { _remoteInvokeService, _typeConvertibleService, serviceContext });
            return instance;
        }
    }
}