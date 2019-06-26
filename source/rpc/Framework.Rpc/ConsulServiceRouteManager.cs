using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Logging;
using Rabbit.Rpc.Routing;
using Rabbit.Rpc.Routing.Implementation;
using Rabbit.Rpc.Serialization;

namespace Framework.Rpc
{
    /// <summary>
    /// 服务端服务路由管理者
    /// </summary>
    public class ConsulServiceRouteManager : ServiceRouteManagerBase
    {
        /// <summary>
        /// consul客户端
        /// </summary>
        private readonly IConsulClient _consulClient;

        /// <summary>
        /// 服务路由列表
        /// </summary>
        private readonly ConcurrentBag<ServiceRoute> _serviceRoutes = new ConcurrentBag<ServiceRoute>();

        /// <summary>
        /// consul配置文件
        /// </summary>
        private readonly ServiceDiscoveryOptions _discoveryOptions;

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ConsulServiceRouteManager> _logger;

        /// <summary>
        /// 注册服务id
        /// </summary>
        private string _registerServiceId = string.Empty;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="consulClient">consul客户端</param>
        /// <param name="serializer">序列化器</param>
        /// <param name="optionProvider">consul配置文件</param>
        /// <param name="logger">日志</param>
        public ConsulServiceRouteManager(
            IConsulClient consulClient,
            ISerializer<string> serializer,
            IServiceDiscoveryOptionProvider optionProvider,
            ILogger<ConsulServiceRouteManager> logger) : base(serializer)
        {
            _consulClient = consulClient;
            _discoveryOptions = optionProvider.Option;
            _logger = logger;
        }

        /// <summary>
        /// 清空所有的服务路由。
        /// </summary>
        /// <returns>一个任务。</returns>
        public override async Task ClearAsync()
        {
            if (_serviceRoutes == null)
            {
                return;
            }
            while (!_serviceRoutes.IsEmpty)
            {
                _serviceRoutes.TryTake(out _);
            }
            if (!string.IsNullOrEmpty(_registerServiceId))
            {
                await _consulClient.Agent.ServiceDeregister(_registerServiceId);
            }
        }

        /// <summary>
        /// 设置服务路由。
        /// </summary>
        /// <param name="routes">服务路由集合。</param>
        /// <returns>一个任务。</returns>
        protected override async Task SetRoutesAsync(IEnumerable<ServiceRouteDescriptor> routes)
        {
            await ClearAsync();
            var namespaces = new List<string>();
            foreach (var route in routes)
            {
                if (route.ServiceDescriptor?.Metadata != null)
                {
                    if (route.ServiceDescriptor.Metadata.TryGetValue("namespace", out var val))
                    {
                        namespaces.Add(val.ToString());
                    }
                }
            }
            if (namespaces.Any())
            {
                _registerServiceId = $"{_discoveryOptions.ListenAddress}:{_discoveryOptions.ListenPort}-{_discoveryOptions.ServiceName}".TrimStart('-');
                var namespacesGroup = namespaces.GroupBy(d => d).Select(d => d.Key);
                var registerServiceName = $"{_discoveryOptions.ServiceName}".TrimStart('-');

                var agentServiceCheck = new AgentServiceCheck
                {
                    TCP = $"{_discoveryOptions.ListenAddress}:{_discoveryOptions.ListenPort}",
                    Interval = TimeSpan.FromSeconds(_discoveryOptions.HealthCheckSeconds),
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(_discoveryOptions.CriticalDeregisterSeconds)
                };
                foreach (var item in namespacesGroup)
                {
                    var meta = new Dictionary<string, string>
                    {
                        {"namespace", item}
                    };
                    var registration = new AgentServiceRegistration
                    {
                        Tags = new[] { _discoveryOptions.ServiceVersion },
                        Checks = new[] { agentServiceCheck },
                        ID = _registerServiceId,
                        Name = registerServiceName,
                        Address = _discoveryOptions.ListenAddress,
                        Port = _discoveryOptions.ListenPort,
                        Meta = meta,
                        //Weights = new Weights
                        //{
                        //    Warning = _discoveryOptions.Weight,
                        //    Passing = 1
                        //}
                    };
                    await _consulClient.Agent.ServiceRegister(registration);
                }
            }
            else
            {
                _logger.LogWarning("服务中心未找到服务程序集");
            }
        }
    }
}