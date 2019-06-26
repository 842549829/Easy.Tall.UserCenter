using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Rabbit.Rpc.ProxyGenerator.Implementation;

namespace Rabbit.Rpc.ProxyGenerator
{
    /// <summary>
    /// Rpc服务集合扩展
    /// </summary>
    public static class RpcServiceCollectionExtensions
    {
        /// <summary>
        /// 添加客户端代理
        /// </summary>
        /// <param name="builder">Rpc服务构建者</param>
        /// <returns>Rpc服务构建者</returns>
        public static IRpcBuilder AddClientProxy(this IRpcBuilder builder)
        {
            var services = builder.Services;
            services.AddSingleton<IServiceProxyGenerator, ServiceProxyGenerator>();
            services.AddSingleton<IServiceProxyFactory, ServiceProxyFactory>();
            return builder;
        }

        /// <summary>
        /// 添加Rpc客户端
        /// </summary>
        /// <param name="services">容器</param>
        /// <param name="serviceInterfaces">代理接口</param>
        /// <returns>Rpc服务构建者</returns>
        public static IRpcBuilder AddClient(this IServiceCollection services, IEnumerable<Type> serviceInterfaces)
        {
            return services
                .AddRpcCore()
                .AddClientRuntime(serviceInterfaces)
                .AddClientProxy();
        }
    }
}