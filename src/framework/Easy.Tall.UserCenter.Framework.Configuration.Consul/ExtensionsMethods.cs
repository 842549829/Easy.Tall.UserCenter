using System;
using System.Collections.Generic;
using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Easy.Tall.UserCenter.Framework.Configuration.Consul
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ExtensionsMethods
    {
        /// <summary>
        /// 添加consul配置
        /// </summary>
        /// <param name="configurationBuilder">configurationBuilder</param>
        /// <param name="section">section</param>
        /// <returns>IConfigurationBuilder</returns>
        public static IConfigurationBuilder AddConsulConfiguration(this IConfigurationBuilder configurationBuilder, string section)
        {
            var configurationRoot = configurationBuilder.Build();
            var consulConfig = configurationRoot.GetSection(section).Get<ConsulConfigurationOptions>();
            if (string.IsNullOrWhiteSpace(consulConfig.Address))
            {
                throw new ArgumentNullException(nameof(consulConfig.Address), "The address can't be empty.");
            }

            if (!string.IsNullOrWhiteSpace(consulConfig.RootFolder) && !consulConfig.RootFolder.EndsWith("/"))
            {
                throw new ArgumentException("Folder must end with \"/\".");
            }

            return Add(configurationBuilder, new ConsulAgentConfiguration
            {
                ClientConfiguration = new ConsulClientConfiguration
                {
                    Address = new Uri(consulConfig.Address),
                    Token = consulConfig.Token,
                    Datacenter = consulConfig.Datacenter
                },
                QueryOptions = new ConsulQueryOptions
                {
                    Folder = consulConfig.RootFolder,
                    Folders = consulConfig.Folders
                }
            });
        }

        /// <summary>
        /// 添加consul配置
        /// </summary>
        /// <param name="configurationBuilder">configurationBuilder</param>
        /// <returns>IConfigurationBuilder</returns>
        public static IConfigurationBuilder AddConsulConfiguration(this IConfigurationBuilder configurationBuilder)
        {
            return configurationBuilder.AddConsulConfiguration("ConfigurationConsul");
        }

        /// <summary>
        ///  consul监听服务注册
        /// </summary>
        /// <param name="services">容器</param>
        /// <returns>容器接口</returns>
        public static IServiceCollection AddConsulConfigurationCenter(this IServiceCollection services)
        {
            services.AddConsulOptions();
            services.Configure<ConsulConfigurationOptions>(d => { });
            services.AddSingleton<IHostedService, ConsulConfigurationHostedService>();
            return services;
        }

        /// <summary>
        /// consul监听服务注册
        /// </summary>
        /// <param name="services">容器</param>
        /// <param name="action">委托配置</param>
        /// <returns>容器接口</returns>
        public static IServiceCollection AddConsulConfigurationCenter(this IServiceCollection services, Action<ConsulConfigurationOptions> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            services.AddConsulOptions();
            services.Configure(action);
            services.AddSingleton<IHostedService, ConsulConfigurationHostedService>();
            return services;
        }

        /// <summary>
        /// 去掉文件夹前缀
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="folder">文件夹</param>
        /// <returns>结果</returns>
        internal static string TrimFolderPrefix(this string key, string folder)
        {
            if (string.IsNullOrWhiteSpace(folder) || folder.Length == 0)
            {
                return key;
            }
            return key.Substring(folder.Length, key.Length - folder.Length);
        }

        /// <summary>
        /// 添加一个Configuration配置
        /// </summary>
        /// <param name="configurationBuilder">configurationBuilder</param>
        /// <param name="configuration">configuration</param>
        /// <returns>IConfigurationBuilder</returns>
        private static IConfigurationBuilder Add(IConfigurationBuilder configurationBuilder, ConsulAgentConfiguration configuration)
        {
            return configurationBuilder.Add(new ConsulConfigurationSource(configuration));
        }

        /// <summary>
        /// 最大选筛选
        /// </summary>
        /// <typeparam name="TElement">TElement</typeparam>
        /// <typeparam name="TData">TData</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="selector">选项</param>
        /// <returns>结果</returns>
        public static TElement MaxElement<TElement, TData>(this IEnumerable<TElement> source, Func<TElement, TData> selector)
            where TData : IComparable<TData>
        {
            return ComparableElement(source, selector, true);
        }

        /// <summary>
        /// 比较筛选
        /// </summary>
        /// <typeparam name="TElement">TElement</typeparam>
        /// <typeparam name="TData">TData</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="selector">选项</param>
        /// <param name="isMax">是否最大</param>
        /// <returns>结果</returns>
        private static TElement ComparableElement<TElement, TData>(IEnumerable<TElement> source, Func<TElement, TData> selector, bool isMax)
            where TData : IComparable<TData>
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            bool firstElement = true;
            TElement result = default(TElement);
            TData maxValue = default(TData);
            foreach (TElement element in source)
            {
                var candidate = selector(element);
                if (!firstElement)
                {
                    if (isMax && candidate.CompareTo(maxValue) <= 0)
                    {
                        continue;
                    }

                    if (!isMax && candidate.CompareTo(maxValue) > 0)
                    {
                        continue;
                    }
                }

                firstElement = false;
                maxValue = candidate;
                result = element;
            }

            return result;
        }

        /// <summary>
        /// 添加consul options
        /// </summary>
        /// <param name="services">服务</param>
        /// <returns>容器</returns>
        private static IServiceCollection AddConsulOptions(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IOptions<>), typeof(OptionsMonitorSource<>));
            services.AddSingleton(typeof(IOptionsSnapshot<>), typeof(OptionsMonitorSource<>));
            services.AddSingleton(typeof(IOptionsMonitor<>), typeof(OptionsMonitorSource<>));
            services.TryAdd(ServiceDescriptor.Transient(typeof(IOptionsFactory<>), typeof(OptionsFactory<>)));
            services.TryAdd(ServiceDescriptor.Singleton(typeof(IOptionsMonitorCache<>), typeof(OptionsCache<>)));
            return services;
        }
    }
}