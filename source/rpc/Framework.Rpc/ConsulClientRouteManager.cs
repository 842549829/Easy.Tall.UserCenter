using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Consul;
using Microsoft.Extensions.Logging;
using Rabbit.Rpc;
using Rabbit.Rpc.Address;
using Rabbit.Rpc.Routing;
using Rabbit.Rpc.Routing.Implementation;
using Rabbit.Rpc.Runtime.Client;
using Rabbit.Rpc.Serialization;

namespace Framework.Rpc
{
    /// <summary>
    /// 客户端服务路由管理者
    /// </summary>
    public class ConsulClientRouteManager : ClientRouteManagerBase
    {
        /// <summary>
        /// 客户端服务条目管理
        /// </summary>
        private readonly IClientEntryManager _entryManager;

        /// <summary>
        /// consul客户端
        /// </summary>
        private readonly IConsulClient _consulClient;

        /// <summary>
        /// 服务路由
        /// </summary>
        private readonly ConcurrentBag<ServiceRoute> _serviceRoutes = new ConcurrentBag<ServiceRoute>();

        /// <summary>
        /// consul配置
        /// </summary>
        private readonly ServiceDiscoveryOptions _discoveryOptions;

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ConsulClientRouteManager> _logger;

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object SyncObject = new object();

        /// <summary>
        /// 心跳检测
        /// </summary>
        private readonly Timer _timer = new Timer();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="entryManager">客户端服务条目管理</param>
        /// <param name="consulClient">consul客户端</param>
        /// <param name="optionProvider">consul配置</param>
        /// <param name="logger">日志</param>
        public ConsulClientRouteManager(
          IClientEntryManager entryManager,
          IConsulClient consulClient,
          IServiceDiscoveryOptionProvider optionProvider,
          ILogger<ConsulClientRouteManager> logger)
        {
            _entryManager = entryManager;
            _consulClient = consulClient;
            _discoveryOptions = optionProvider.Option;
            _logger = logger;
            _timer.Interval = TimeSpan.FromSeconds(_discoveryOptions.ChangeCheckSeconds).TotalMilliseconds;
            _timer.Elapsed += (sender, e) =>
            {
                lock (SyncObject)
                {
                    UpdateRouteFromConsul();
                }
            };
            _timer.Start();
        }

        /// <summary>
        /// 清除路由列表
        /// </summary>
        /// <returns>表示一个异步</returns>
        public override Task ClearAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                lock (SyncObject)
                {
                    ClearRoutes();
                }
            });
        }

        /// <summary>
        /// 获取路由列表
        /// </summary>
        /// <returns>路由列表</returns>
        public override async Task<IEnumerable<ServiceRoute>> GetRoutesAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                lock (SyncObject)
                {
                    if (_serviceRoutes.Count == 0)
                    {
                        UpdateRouteFromConsul();
                    }
                    return _serviceRoutes;
                }
            });

        }

        /// <summary>
        /// 清除路由列表
        /// </summary>
        private void ClearRoutes()
        {
            if (_serviceRoutes == null)
            {
                return;
            }
            while (!_serviceRoutes.IsEmpty)
            {
                _serviceRoutes.TryTake(out _);
            }
        }

        /// <summary>
        /// 添加服务路由列表
        /// </summary>
        /// <param name="routes">路由列表</param>
        private void AddRoutes(IEnumerable<ServiceRoute> routes)
        {
            foreach (var item in routes)
            {
                _serviceRoutes.Add(item);
            }
        }

        /// <summary>
        /// 更新路由列表
        /// </summary>
        /// <returns></returns>
        private void UpdateRouteFromConsul()
        {
            var serviceRoutes = new List<ServiceRoute>();
            //如果未配置远程服务，取所有服务
            if (_discoveryOptions.RemoteServices.Count == 0)
            {
                var serviceQuery = _consulClient.Catalog.Services().Result;
                foreach (var item in serviceQuery.Response)
                {
                    var query = _consulClient.Health.Service(item.Key).Result;
                    var serviceEntries = query.Response;
                    if (serviceEntries == null)
                    {
                        _logger.LogWarning($"服务中心未找到{item.Key}的服务条目");
                        continue;
                    }
                    serviceEntries = serviceEntries.Where(d => d.Service.Meta != null).ToArray();
                    if (serviceEntries.Length > 0)
                    {
                        serviceRoutes.AddRange(GetServiceRoutes(serviceEntries));
                    }
                }
            }
            else
            {
                //获取整个集群中健康的服务
                foreach (var item in _discoveryOptions.RemoteServices)
                {
                    var query = _consulClient.Health.Service(item.Key, item.Value, true).Result;
                    var serviceEntries = query.Response;
                    if (serviceEntries == null)
                    {
                        _logger.LogWarning($"服务中心未找到{item.Key}的服务条目");
                        continue;
                    }
                    serviceEntries = serviceEntries.Where(d => d.Service.Meta != null).ToArray();
                    if (serviceEntries.Length > 0)
                    {
                        serviceRoutes.AddRange(GetServiceRoutes(serviceEntries));
                    }
                }
            }

            ClearRoutes();
            AddRoutes(serviceRoutes);
        }

        /// <summary>
        /// 获取服务路由
        /// </summary>
        /// <param name="serviceEntries">服务路由</param>
        /// <returns>服务路由</returns>
        private IEnumerable<ServiceRoute> GetServiceRoutes(IEnumerable<Consul.ServiceEntry> serviceEntries)
        {
            //去掉consul自带服务
            try
            {
                serviceEntries = serviceEntries.Where(d => d.Service.ID != "consul").ToArray();
                var entries = _entryManager.GetEntries();
                var serviceRoutes = new List<ServiceRoute>();
                foreach (var entriesItem in entries)
                {
                    if (entriesItem.Descriptor?.Metadata != null && entriesItem.Descriptor.Metadata.TryGetValue("namespace", out var entriesNamespace))
                    {
                        foreach (var serviceEntryItem in serviceEntries)
                        {
                            if (serviceEntryItem.Service.Meta.TryGetValue("namespace", out var serviceEntryNamespace))
                            {
                                if (entriesNamespace.ToString().Contains(serviceEntryNamespace))
                                {
                                    serviceRoutes.Add(new ServiceRoute
                                    {
                                        Address = new[]
                                        {
                                            new IpAddressModel
                                            {
                                                Ip = serviceEntryItem.Service.Address,
                                                Port = serviceEntryItem.Service.Port,
                                                //Weights = new Rabbit.Rpc.Address.Weights
                                                //{
                                                //    //Passing = serviceEntryItem.Service.Meta.TryGetValue(),
                                                //    //Warning = serviceEntryItem.Service.Weights.Warning
                                                //}
                                            }
                                        },
                                        ServiceDescriptor = new ServiceDescriptor
                                        {
                                            Id = entriesItem.Descriptor.Id
                                        }
                                    });
                                }
                            }
                        }
                    }
                }

                return serviceRoutes.GroupBy(d => d.ServiceDescriptor.Id)
                    .Select(d => new ServiceRoute
                    {
                        Address = d.SelectMany(di => di.Address),
                        ServiceDescriptor = new ServiceDescriptor
                        {
                            Id = d.Key
                        }
                    }).ToArray();
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "获取服务条目出错");
            }
            return new ServiceRoute[] { };
        }
    }
}
