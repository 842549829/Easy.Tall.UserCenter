using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Rabbit.Rpc.Address;
using Rabbit.Rpc.Routing;
using Rabbit.Rpc.Routing.Event;
using Rabbit.Rpc.Runtime.Client.HealthChecks;

namespace Rabbit.Rpc.Runtime.Client.Address.Resolvers.Implementation.Selectors.Implementation
{
    /// <summary>
    /// 轮询的地址选择器
    /// </summary>
    public class PollingAddressSelector : AddressSelectorBase
    {
        /// <summary>
        /// 健康检查服务
        /// </summary>
        private readonly IHealthCheckService _healthCheckService;

        /// <summary>
        /// 轮询服务条目
        /// </summary>
        private readonly ConcurrentDictionary<string, Lazy<PollingAddressEntry>> _concurrent =
            new ConcurrentDictionary<string, Lazy<PollingAddressEntry>>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceRouteManager">客户端理由服务</param>
        /// <param name="healthCheckService">健康检查服务</param>
        public PollingAddressSelector(IClientRouteManager serviceRouteManager, IHealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
            //路由发生变更时重建地址条目。
            serviceRouteManager.Changed += ServiceRouteManager_Removed;
            serviceRouteManager.Removed += ServiceRouteManager_Removed;
        }

        /// <summary>
        /// 选择一个地址。
        /// </summary>
        /// <param name="context">地址选择上下文。</param>
        /// <returns>地址模型。</returns>
        protected override async Task<AddressModel> SelectAsync(AddressSelectContext context)
        {
            var serviceId = GetServiceId(context.Descriptor);
            // 根据服务id缓存服务地址
            var addressEntry = _concurrent.GetOrAdd(serviceId, k => new Lazy<PollingAddressEntry>(() => new PollingAddressEntry(context.Address))).Value;
            AddressModel addressModel;
            do
            {
                addressModel = addressEntry.GetAddress();
            } while (await _healthCheckService.IsHealth(addressModel) == false);
            return addressModel;
        }

        /// <summary>
        /// 服务路由删除事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void ServiceRouteManager_Removed(object sender, ServiceRouteEventArgs e)
        {
            var key = GetServiceId(e.Route.ServiceDescriptor);
            _concurrent.TryRemove(key, out _);
        }

        /// <summary>
        /// 获取服务Id
        /// </summary>
        /// <param name="descriptor">服务描述符</param>
        /// <returns>服务Id</returns>
        private static string GetServiceId(ServiceDescriptor descriptor)
        {
            return descriptor.Id;
        }
    }
}