using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rabbit.Rpc.Address;
using Rabbit.Rpc.Routing;
using Rabbit.Rpc.Runtime.Client.Address.Resolvers.Implementation.Selectors;
using Rabbit.Rpc.Runtime.Client.HealthChecks;

namespace Rabbit.Rpc.Runtime.Client.Address.Resolvers.Implementation
{
    /// <summary>
    /// 一个人默认的服务地址解析器
    /// </summary>
    public class DefaultAddressResolver : IAddressResolver
    {
        /// <summary>
        /// 客户端路由服务
        /// </summary>
        private readonly IClientRouteManager _clientRouteManager;

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<DefaultAddressResolver> _logger;

        /// <summary>
        /// 地址选择器
        /// </summary>
        private readonly IAddressSelector _addressSelector;

        /// <summary>
        /// 健康检查
        /// </summary>
        private readonly IHealthCheckService _healthCheckService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="clientRouteManager">客户端路由服务</param>
        /// <param name="logger">日志</param>
        /// <param name="addressSelector">地址选择器</param>
        /// <param name="healthCheckService">健康检查</param>
        public DefaultAddressResolver(IClientRouteManager clientRouteManager, ILogger<DefaultAddressResolver> logger, IAddressSelector addressSelector, IHealthCheckService healthCheckService)
        {
            _clientRouteManager = clientRouteManager;
            _logger = logger;
            _addressSelector = addressSelector;
            _healthCheckService = healthCheckService;
        }

        /// <summary>
        /// 解析服务地址。
        /// </summary>
        /// <param name="serviceId">服务Id。</param>
        /// <returns>服务地址模型。</returns>
        public async Task<AddressModel> Resolver(string serviceId)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            _logger.LogDebug($"准备为服务id：{serviceId}，解析可用地址。");
            var descriptors = await _clientRouteManager.GetRoutesAsync();
            var descriptor = descriptors.FirstOrDefault(i => i.ServiceDescriptor.Id == serviceId);
            _logger.LogDebug($"服务Id为:{serviceId}的服务,Resolver耗时:{stopwatch.ElapsedMilliseconds} 毫秒");
            stopwatch.Restart();
            if (descriptor == null)
            {
                _logger.LogWarning($"根据服务id：{serviceId}，找不到相关服务信息。");
                return null;
            }

            var address = new List<AddressModel>();
            foreach (var addressModel in descriptor.Address)
            {
                await _healthCheckService.Monitor(addressModel);
                if (!await _healthCheckService.IsHealth(addressModel))
                    continue;

                address.Add(addressModel);
            }

            var hasAddress = address.Any();
            if (!hasAddress)
            {
                _logger.LogWarning($"根据服务id：{serviceId}，找不到可用的地址。");
                return null;
            }

            _logger.LogInformation($"根据服务id：{serviceId}，找到以下可用地址：{string.Join(",", address.Select(i => i.ToString()))}。");
            _logger.LogDebug($"服务Id为:{serviceId}的服务,Process耗时:{stopwatch.ElapsedMilliseconds} 毫秒");

            stopwatch.Restart();
            var result = await _addressSelector.SelectAsync(new AddressSelectContext
            {
                Descriptor = descriptor.ServiceDescriptor,
                Address = address
            });
            _logger.LogDebug($"服务Id为:{serviceId}的服务,Select耗时:{stopwatch.ElapsedMilliseconds} 毫秒");
            stopwatch.Stop();
            return result;
        }
    }
}