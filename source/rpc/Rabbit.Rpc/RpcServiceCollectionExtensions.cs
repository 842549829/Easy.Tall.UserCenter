using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rabbit.Rpc.Convertibles;
using Rabbit.Rpc.Convertibles.Implementation;
using Rabbit.Rpc.Ids;
using Rabbit.Rpc.Ids.Implementation;
using Rabbit.Rpc.Messages;
using Rabbit.Rpc.Routing;
using Rabbit.Rpc.Routing.Implementation;
using Rabbit.Rpc.Runtime.Client;
using Rabbit.Rpc.Runtime.Client.Address.Resolvers;
using Rabbit.Rpc.Runtime.Client.Address.Resolvers.Implementation;
using Rabbit.Rpc.Runtime.Client.Address.Resolvers.Implementation.Selectors;
using Rabbit.Rpc.Runtime.Client.Address.Resolvers.Implementation.Selectors.Implementation;
using Rabbit.Rpc.Runtime.Client.HealthChecks;
using Rabbit.Rpc.Runtime.Client.HealthChecks.Implementation;
using Rabbit.Rpc.Runtime.Client.Implementation;
using Rabbit.Rpc.Runtime.Client.Implementation.ServiceDiscovery;
using Rabbit.Rpc.Runtime.Client.Implementation.ServiceDiscovery.Implementation;
using Rabbit.Rpc.Runtime.Server;
using Rabbit.Rpc.Runtime.Server.Implementation;
using Rabbit.Rpc.Runtime.Server.Implementation.ServiceDiscovery;
using Rabbit.Rpc.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Rabbit.Rpc.Runtime.Server.Implementation.ServiceDiscovery.Implementation;
using Rabbit.Rpc.Serialization;
using Rabbit.Rpc.Serialization.Implementation;
using Rabbit.Rpc.Transport.Codec;
using Rabbit.Rpc.Transport.Codec.Implementation;

namespace Rabbit.Rpc
{
    /// <summary>
    /// Rrc扩展
    /// </summary>
    public static class RpcServiceCollectionExtensions
    {
        #region CodecOrSerialization
        /// <summary>
        /// 添加Json序列化支持。
        /// </summary>
        /// <param name="builder">Rpc服务构建者。</param>
        /// <returns>Rpc服务构建者。</returns>
        public static IRpcBuilder AddJsonSerialization(this IRpcBuilder builder)
        {
            var services = builder.Services;

            services.AddSingleton<ISerializer<string>, JsonSerializer>();
            services.AddSingleton<ISerializer<byte[]>, StringByteArraySerializer>();
            services.AddSingleton<ISerializer<object>, StringObjectSerializer>();

            return builder;
        }

        /// <summary>
        /// 使用编解码器。
        /// </summary>
        /// <typeparam name="T">编解码器工厂实现类型。</typeparam>
        /// <param name="builder">Rpc服务构建者。</param>
        /// <returns>Rpc服务构建者。</returns>
        public static IRpcBuilder UseCodec<T>(this IRpcBuilder builder) where T : class, ITransportMessageCodecFactory
        {
            builder.Services.AddSingleton<ITransportMessageCodecFactory, T>();
            return builder;
        }

        /// <summary>
        /// 使用Json编解码器。
        /// </summary>
        /// <param name="builder">Rpc服务构建者。</param>
        /// <returns>Rpc服务构建者。</returns>
        public static IRpcBuilder UseJsonCodec(this IRpcBuilder builder)
        {
            return builder.UseCodec<JsonTransportMessageCodecFactory>();
        }
        #endregion

        #region AddressSelector

        /// <summary>
        /// 设置服务地址选择器。
        /// </summary>
        /// <typeparam name="T">地址选择器实现类型。</typeparam>
        /// <param name="builder">Rpc服务构建者。</param>
        /// <returns>Rpc服务构建者。</returns>
        public static IRpcBuilder UseAddressSelector<T>(this IRpcBuilder builder) where T : class, IAddressSelector
        {
            builder.Services.AddSingleton<IAddressSelector, T>();
            return builder;
        }

        /// <summary>
        /// 设置服务地址选择器。
        /// </summary>
        /// <param name="builder">Rpc服务构建者。</param>
        /// <param name="factory">服务地址选择器实例工厂。</param>
        /// <returns>Rpc服务构建者。</returns>
        public static IRpcBuilder UseAddressSelector(this IRpcBuilder builder,
            Func<IServiceProvider, IAddressSelector> factory)
        {
            builder.Services.AddSingleton(factory);

            return builder;
        }

        /// <summary>
        /// 设置服务地址选择器。
        /// </summary>
        /// <param name="builder">Rpc服务构建者。</param>
        /// <param name="instance">地址选择器实例。</param>
        /// <returns>Rpc服务构建者。</returns>
        public static IRpcBuilder UseAddressSelector(this IRpcBuilder builder, IAddressSelector instance)
        {
            builder.Services.AddSingleton(instance);

            return builder;
        }

        /// <summary>
        /// 使用轮询的地址选择器。
        /// </summary>
        /// <param name="builder">Rpc服务构建者。</param>
        /// <returns>Rpc服务构建者。</returns>
        public static IRpcBuilder UsePollingAddressSelector(this IRpcBuilder builder)
        {
            builder.Services.AddSingleton<IAddressSelector, PollingAddressSelector>();
            return builder;
        }

        /// <summary>
        /// 使用随机的地址选择器。
        /// </summary>
        /// <param name="builder">Rpc服务构建者。</param>
        /// <returns>Rpc服务构建者。</returns>
        public static IRpcBuilder UseRandomAddressSelector(this IRpcBuilder builder)
        {
            builder.Services.AddSingleton<IAddressSelector, RandomAddressSelector>();
            return builder;
        }

        /// <summary>
        /// 使用哈希源地址选择器。
        /// </summary>
        /// <param name="builder">Rpc服务构建者。</param>
        /// <returns>Rpc服务构建者。</returns>
        public static IRpcBuilder UseKetAmaHashingAddressSelector(this IRpcBuilder builder)
        {
            builder.Services.AddSingleton<IKetAmaHashingIpAddress, DefaultKetAmaHashingIpAddress>();
            builder.Services.AddSingleton<IAddressSelector, KetAmaHashingAddressSelector>();
            return builder;
        }

        #endregion AddressSelector

        #region RouteManager
        /// <summary>
        /// 设置服务路由管理者。
        /// </summary>
        /// <typeparam name="T">服务路由管理者实现。</typeparam>
        /// <param name="builder">Rpc服务构建者。</param>
        /// <returns>Rpc服务构建者。</returns>
        public static IRpcBuilder UseServiceRouteManager<T>(this IRpcBuilder builder) where T : class, IServiceRouteManager
        {
            builder.Services.AddSingleton<IServiceRouteManager, T>();
            return builder;
        }

        /// <summary>
        /// 设置服务路由管理者。
        /// </summary>
        /// <typeparam name="T">服务路由管理者实现。</typeparam>
        /// <param name="builder">Rpc服务构建者。</param>
        /// <returns>Rpc服务构建者。</returns>
        public static IRpcBuilder UseClientRouteManager<T>(this IRpcBuilder builder) where T : class, IClientRouteManager
        {
            builder.Services.AddSingleton<IClientRouteManager, T>();
            return builder;
        }

        /// <summary>
        /// 设置服务路由管理者。
        /// </summary>
        /// <param name="builder">Rpc服务构建者。</param>
        /// <param name="factory">服务路由管理者实例工厂。</param>
        /// <returns>Rpc服务构建者。</returns>
        public static IRpcBuilder UseRouteManager(this IRpcBuilder builder, Func<IServiceProvider, IServiceRouteManager> factory)
        {
            builder.Services.AddSingleton(factory);
            return builder;
        }

        /// <summary>
        /// 设置服务路由管理者。
        /// </summary>
        /// <param name="builder">Rpc服务构建者。</param>
        /// <param name="instance">服务路由管理者实例。</param>
        /// <returns>Rpc服务构建者。</returns>
        public static IRpcBuilder UseRouteManager(this IRpcBuilder builder, IServiceRouteManager instance)
        {
            builder.Services.AddSingleton(instance);
            return builder;
        }
        #endregion RouteManager

        /// <summary>
        /// 添加RPC核心服务。
        /// </summary>
        /// <param name="services">服务集合。</param>
        /// <returns>Rpc服务构建者。</returns>
        public static IRpcBuilder AddRpcCore(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            //服务Id生成器
            services.AddSingleton<IServiceIdGenerator, DefaultServiceIdGenerator>();
            //类型转换提供程序
            services.AddSingleton<ITypeConvertibleProvider, DefaultTypeConvertibleProvider>();
            //类型转换服务
            services.AddSingleton<ITypeConvertibleService, DefaultTypeConvertibleService>();
            //服务路由工厂
            services.AddSingleton<IServiceRouteFactory, DefaultServiceRouteFactory>();
            //有状态服务上下文获取器
            services.AddScoped<IRpcContextAccessor, RpcContextAccessor>();

            // 添加JSON序列器 和 添加JSON编解码工厂
            return new RpcBuilder(services)
                .AddJsonSerialization()
                .UseJsonCodec();
        }

        /// <summary>
        /// 添加客户端运行时服务。
        /// </summary>
        /// <param name="builder">Rpc服务构建者。</param>
        /// <param name="serviceInterfaces">服务接口</param>
        /// <returns>Rpc服务构建者。</returns>
        public static IRpcBuilder AddClientRuntime(this IRpcBuilder builder, IEnumerable<Type> serviceInterfaces)
        {
            var services = builder.Services;
            //服务条目工厂
            services.AddSingleton<IClrClientEntryFactory, CrlClientEntryFactory>();
            //通过(服务发现特性类型扫)描服务条目
            services.AddSingleton<IClientEntryProvider>(provider => new DefaultClientEntryProvider(serviceInterfaces, provider.GetRequiredService<IClrClientEntryFactory>(), provider.GetRequiredService<ILogger<DefaultClientEntryProvider>>()));
            //服务条目管理者(避免有重复的服务条目)
            services.AddSingleton<IClientEntryManager, DefaultClientEntryManager>();
            //健康地址检查
            services.AddSingleton<IHealthCheckService, DefaultHealthCheckService>();
            //地址解析
            services.AddSingleton<IAddressResolver, DefaultAddressResolver>();
            //远程调用
            services.AddSingleton<IRemoteInvokeService, RemoteInvokeService>();
            // 负载算法
            return builder.UsePollingAddressSelector();
        }

        /// <summary>
        /// 添加服务端运行时服务
        /// </summary>
        /// <param name="builder">Rpc服务构建者</param>
        /// <param name="discoveryAttribute">服务接口</param>
        /// <returns>Rpc服务构建者</returns>
        public static IRpcBuilder AddServiceRuntime(this IRpcBuilder builder, Type discoveryAttribute)
        {
            var services = builder.Services;
            //服务条目工厂
            services.AddSingleton<IClrServiceEntryFactory, ClrServiceEntryFactory>();
            //通过(服务发现特性类型扫)描服务条目
            services.AddSingleton<IServiceEntryProvider>(provider => new AttributeServiceEntryProvider(discoveryAttribute ?? typeof(RpcServiceBundleAttribute), provider.GetRequiredService<IClrServiceEntryFactory>(), provider.GetRequiredService<ILogger<AttributeServiceEntryProvider>>()));
            //服务条目管理者(避免有重复的服务条目)
            services.AddSingleton<IServiceEntryManager, DefaultServiceEntryManager>();
            //根据Id定位对应的服务条目
            services.AddSingleton<IServiceEntryLocate, DefaultServiceEntryLocate>();
            //服务执行者 
            services.AddSingleton<IServiceExecutor, DefaultServiceExecutor>();
            return builder;
        }
    }
}