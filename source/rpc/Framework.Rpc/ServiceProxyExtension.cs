using System;
using System.Collections.Generic;
using System.Linq;
using Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rabbit.Rpc;
using Rabbit.Rpc.Messages;
using Rabbit.Rpc.ProxyGenerator;
using Rabbit.Rpc.Runtime.Server;
using Rabbit.Rpc.Runtime.Server.Implementation.ServiceDiscovery;
using Rabbit.Rpc.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Rabbit.Transport.DotNetty;

namespace Framework.Rpc
{
    /// <summary>
    /// 服务代理扩展类
    /// </summary>
    public static class ServiceProxyExtension
    {
        /// <summary>
        /// 引用consul服务
        /// </summary>
        /// <param name="builder">Rpc服务构建者</param>
        /// <param name="options">服务发现选项</param>
        /// <returns>Rpc服务构建者</returns>
        public static IRpcBuilder UseRpcConsulDiscovery(this IRpcBuilder builder, Action<ServiceDiscoveryOptions> options = null)
        {
            builder.Services.AddSingleton<IConsulClient>(d =>
            {
                var optionProvider = d.GetRequiredService<IServiceDiscoveryOptionProvider>();
                var config = optionProvider.Option;
                options?.Invoke(config);
                var client = new ConsulClient(c =>
                {
                    c.Address = config.EndPoint.ToUri();
                    c.Datacenter = c.Datacenter;
                    c.Token = c.Token;
                });
                return client;
            });
            return builder;
        }
        
        /// <summary>
        /// 使用随机端口
        /// </summary>
        /// <param name="builder">IRpcBuilder</param>
        /// <returns>IRpcBuilder</returns>
        public static IRpcBuilder UseRandomPort(this IRpcBuilder builder)
        {
            builder.Services.AddSingleton<IServiceDiscoveryOptionProvider, ServiceDiscoveryOptionProvider>();
            return builder;
        }

        /// <summary>
        /// 添加Rpc默认服务(服务端)，使用默认的服务发现特性
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultRpcServer(this IServiceCollection services)
        {
            services.AddDefaultRpcServer(null);
            return services;
        }

        /// <summary>
        /// 使用特定的RPC服务标记特性
        /// </summary>
        /// <typeparam name="T">服务标记特性类型</typeparam>
        /// /// <param name="services"></param>
        /// <returns>IRpcBuilder</returns>
        public static IServiceCollection AddRpcServiceAttribute<T>(this IServiceCollection services)
        {
            return AddRpcServiceAttribute(services, typeof(T));
        }

        /// <summary>
        /// 使用特定的RPC服务标记特性
        /// </summary>
        /// <param name="services"></param>
        /// <param name="rpcServiceType">服务标记特性类型</param>
        /// <returns>IRpcBuilder</returns>
        public static IServiceCollection AddRpcServiceAttribute(this IServiceCollection services, Type rpcServiceType)
        {
            var defaultConfig = services.Where(d => d.ServiceType == typeof(IServiceEntryProvider)).ToArray();
            foreach (var item in defaultConfig)
            {
                services.Remove(item);
            }
            services.AddSingleton<IServiceEntryProvider>(provider =>
                new AttributeServiceEntryProvider(
                    rpcServiceType,
                    provider.GetRequiredService<IClrServiceEntryFactory>(),
                    provider.GetRequiredService<ILogger<AttributeServiceEntryProvider>>()));
            return services;
        }

        /// <summary>
        /// 添加服务端服务(服务端)
        /// </summary>
        /// <param name="services">容器接口</param>
        /// <param name="rpcServiceAttribute">服务特性类型</param>
        /// <param name="options">服务发现选项</param>
        /// <returns>容器接口</returns>
        public static IServiceCollection AddDefaultRpcServer(this IServiceCollection services,
             Type rpcServiceAttribute, Action<ServiceDiscoveryOptions> options = null)
        {
            services.AddRpcCore()
                .AddServiceRuntime(rpcServiceAttribute)
                .UseServiceRouteManager<ConsulServiceRouteManager>()
                .UseDotNettyTransport()
                .UseRandomPort()
                .UseRpcConsulDiscovery(options);
                
            return services;
        }

        /// <summary>
        /// 添加Rpc客户端服务(客户端)
        /// </summary>
        /// <param name="serviceCollection">容器</param>
        /// <param name="serviceInterfaces">服务代理</param>
        /// <param name="options">选项</param>
        /// <returns>容器</returns>
        public static IRpcBuilder AddDefaultRpcClient(
            this IServiceCollection serviceCollection,
            IEnumerable<Type> serviceInterfaces,
            Action<ServiceDiscoveryOptions> options = null)
        {
            serviceInterfaces = serviceInterfaces.ToList();
            if (serviceInterfaces.Any(d => !d.IsInterface))
            {
                throw new ArgumentException($"参数{nameof(serviceInterfaces)}中的类型必须是接口");
            }

            var builder = serviceCollection
                .AddSingleton<IServiceDiscoveryOptionProvider, ServiceDiscoveryOptionProvider>()
                .AddClient(serviceInterfaces)
                .UseClientRouteManager<ConsulClientRouteManager>()
                .UseJsonCodec()
                .UseDotNettyTransport()
                .UseRpcConsulDiscovery(options);

            var provider = serviceCollection.BuildServiceProvider();

            //添加服务代理
            var proxyGen = provider.GetRequiredService<IServiceProxyGenerator>();
            var serviceTypes = proxyGen.GenerateProxies(serviceInterfaces);
            foreach (var service in serviceInterfaces)
            {
                serviceCollection.AddScoped(service, serviceTypes.FirstOrDefault(d => d.GetInterfaces().Contains(service)));
            }
            return builder;
        }

        /// <summary>
        /// 通过工厂方法提供ServiceContext
        /// </summary>
        /// <param name="builder">rpcBuilder</param>
        /// <param name="contextFactory">ServiceContext工厂</param>
        /// <returns></returns>
        public static IRpcBuilder UseServiceContextAccessor(this IRpcBuilder builder, Func<IServiceProvider, object> contextFactory)
        {
            builder.Services.AddScoped(typeof(IRpcContextAccessor), serviceProvider => new RpcContextAccessor
            {
                RpcContext = contextFactory?.Invoke(serviceProvider)
            });
            return builder;
        }
    }
}