using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rabbit.Rpc.Address;
using Rabbit.Rpc.Routing;
using Rabbit.Rpc.Runtime.Server;

namespace Framework.Rpc
{
    /// <summary>
    /// 定义由主机管理的对象的方法
    /// </summary>
    public class RpcHost : IHostedService
    {
        /// <summary>
        /// Consul配置信息
        /// </summary>
        private readonly ServiceDiscoveryOptions _serviceDiscoveryOptions;

        /// <summary>
        /// 容器接口
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 服务主机
        /// </summary>
        private IServiceHost _rpcServiceHost;

        /// <summary>
        /// 服务路由管理
        /// </summary>
        private IServiceRouteManager _serviceRouteManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceProvider">容器接口</param>
        public RpcHost(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var optionProvider = _serviceProvider.GetRequiredService<IServiceDiscoveryOptionProvider>();
            _serviceDiscoveryOptions = optionProvider.Option;
        }

        /// <summary>
        /// 当应用程序主机准备启动服务时触发
        /// </summary>
        /// <param name="cancellationToken">指示启动进程已中止</param>
        /// <returns>表示异步操作</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_rpcServiceHost != null)
            {
                throw new InvalidOperationException("已存在一个运行的ServiceHost实例");
            }
            var serviceHost = _serviceProvider.GetRequiredService<IServiceHost>();
            _rpcServiceHost = serviceHost;
            return Task.Run(async () =>
            {
                var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
                var log = loggerFactory.CreateLogger(GetType());
                //启动主机
                await serviceHost.StartAsync(new IPEndPoint(IPAddress.Parse(_serviceDiscoveryOptions.ListenAddress), _serviceDiscoveryOptions.ListenPort));
                log.LogInformation("RPC Server started.Listen address {ListenAddress} port {port}", _serviceDiscoveryOptions.ListenAddress, _serviceDiscoveryOptions.ListenPort);

                //自动生成服务路由
                var serviceEntryManager = _serviceProvider.GetRequiredService<IServiceEntryManager>();
                var addressDescriptors = serviceEntryManager.GetEntries().Select(i => new ServiceRoute
                {
                    Address = new[]
                    {
                        new IpAddressModel { Ip = _serviceDiscoveryOptions.ListenAddress, Port = _serviceDiscoveryOptions.ListenPort }
                    },
                    ServiceDescriptor = i.Descriptor
                });

                _serviceRouteManager = _serviceProvider.GetRequiredService<IServiceRouteManager>();
                await _serviceRouteManager.SetRouteAsync(addressDescriptors);
            }, cancellationToken);
        }

        /// <summary>
        /// 当应用程序主机执行优雅的关闭时触发
        /// </summary>
        /// <param name="cancellationToken">指示关闭过程不再是优雅的</param>
        /// <returns>表示异步操作</returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _rpcServiceHost?.Dispose();
            if (_serviceRouteManager != null)
            {
                await _serviceRouteManager.ClearAsync();
            }
        }
    }
}