using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Rabbit.Rpc.Address;
using Rabbit.Rpc.Routing;

namespace Rabbit.Rpc.Runtime.Client.HealthChecks.Implementation
{
    /// <summary>
    /// 一个默认的健康检查服务
    /// </summary>
    public class DefaultHealthCheckService : IHealthCheckService, IDisposable
    {
        /// <summary>
        /// 监听条目字段
        /// </summary>
        private readonly ConcurrentDictionary<string, MonitorEntry> _dictionary = new ConcurrentDictionary<string, MonitorEntry>();

        /// <summary>
        /// 心跳监听
        /// </summary>
        private readonly Timer _timer;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceRouteManager">客户端服务路由</param>
        public DefaultHealthCheckService(IClientRouteManager serviceRouteManager)
        {
            var timeSpan = TimeSpan.FromSeconds(10);
            _timer = new Timer(s =>
            {
                Check(_dictionary.ToArray().Select(i => i.Value));
            }, null, timeSpan, timeSpan);

            //去除监控。
            serviceRouteManager.Removed += (s, e) =>
            {
                Remove(e.Route.Address);
            };
            //重新监控。
            serviceRouteManager.Created += (s, e) =>
            {
                var keys = e.Route.Address.Select(i => i.ToString());
                Check(_dictionary.Where(i => keys.Contains(i.Key)).Select(i => i.Value));
            };
            //重新监控。
            serviceRouteManager.Changed += (s, e) =>
            {
                var keys = e.Route.Address.Select(i => i.ToString());
                Check(_dictionary.Where(i => keys.Contains(i.Key)).Select(i => i.Value));
            };
        }

        /// <summary>
        /// 检查
        /// </summary>
        /// <param name="entries">监听条目</param>
        private static void Check(IEnumerable<MonitorEntry> entries)
        {
            foreach (var monitorEntry in entries)
            {
                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    try
                    {
                        socket.Connect(monitorEntry.EndPoint);
                        monitorEntry.Health = true;
                    }
                    catch 
                    {
                        monitorEntry.Health = false;
                    }
                }
            }
        }

        /// <summary>
        /// 删除服务地址
        /// </summary>
        /// <param name="addressModels">服务地址</param>
        private void Remove(IEnumerable<AddressModel> addressModels)
        {
            foreach (var addressModel in addressModels)
            {
                _dictionary.TryRemove(addressModel.ToString(), out _);
            }
        }

        /// <summary>
        /// 监控一个地址。
        /// </summary>
        /// <param name="address">地址模型。</param>
        /// <returns>一个任务。</returns>
        public Task Monitor(AddressModel address)
        {
            return Task.Factory.StartNew(() =>
                {
                    _dictionary.GetOrAdd(address.ToString(), k => new MonitorEntry(address));
                });
        }

        /// <summary>
        /// 判断一个地址是否健康。
        /// </summary>
        /// <param name="address">地址模型。</param>
        /// <returns>健康返回true，否则返回false。</returns>
        public Task<bool> IsHealth(AddressModel address)
        {
            return Task.Factory.StartNew(() =>
            {
                var key = address.ToString();
                return !_dictionary.TryGetValue(key, out var entry) || entry.Health;
            });
        }

        /// <summary>
        /// 标记一个地址为失败的。
        /// </summary>
        /// <param name="address">地址模型。</param>
        /// <returns>一个任务。</returns>
        public Task MarkFailure(AddressModel address)
        {
            return Task.Factory.StartNew(() =>
            {
                var key = address.ToString();
                var entry = _dictionary.GetOrAdd(key, k => new MonitorEntry(address, false));
                entry.Health = false;
            });
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
