using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rabbit.Rpc.Exceptions;
using Rabbit.Rpc.Messages;
using Rabbit.Rpc.Runtime.Client.Address.Resolvers;
using Rabbit.Rpc.Runtime.Client.HealthChecks;
using Rabbit.Rpc.Transport;

namespace Rabbit.Rpc.Runtime.Client.Implementation
{
    /// <summary>
    /// 远程调用服务实现
    /// </summary>
    public class RemoteInvokeService : IRemoteInvokeService
    {
        /// <summary>
        /// 服务地址解析器
        /// </summary>
        private readonly IAddressResolver _addressResolver;

        /// <summary>
        /// 传输客户端工厂
        /// </summary>
        private readonly ITransportClientFactory _transportClientFactory;

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<RemoteInvokeService> _logger;

        /// <summary>
        /// 健康检查服务
        /// </summary>
        private readonly IHealthCheckService _healthCheckService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="addressResolver">服务地址解析器</param>
        /// <param name="transportClientFactory">传输客户端工厂</param>
        /// <param name="logger">日志</param>
        /// <param name="healthCheckService">健康检查服务</param>
        public RemoteInvokeService(IAddressResolver addressResolver, ITransportClientFactory transportClientFactory, ILogger<RemoteInvokeService> logger, IHealthCheckService healthCheckService)
        {
            _addressResolver = addressResolver;
            _transportClientFactory = transportClientFactory;
            _logger = logger;
            _healthCheckService = healthCheckService;
        }

        /// <summary>
        /// 调用远程服务
        /// </summary>
        /// <param name="context">调用当前上下文</param>
        /// <returns>远程调用结果消息模型</returns>
        public Task<RemoteInvokeResultMessage> InvokeAsync(RemoteInvokeContext context)
        {
            return InvokeAsync(context, Task.Factory.CancellationToken);
        }

        /// <summary>
        /// 调用远程服务
        /// </summary>
        /// <param name="context">调用上下文。</param>
        /// <param name="cancellationToken">取消操作通知实例。</param>
        /// <returns>远程调用结果消息模型。</returns>
        public async Task<RemoteInvokeResultMessage> InvokeAsync(RemoteInvokeContext context, CancellationToken cancellationToken)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.InvokeMessage == null)
            {
                throw new ArgumentNullException(nameof(context.InvokeMessage));
            }

            if (string.IsNullOrEmpty(context.InvokeMessage.ServiceId))
            {
                throw new ArgumentException("服务Id不能为空。", nameof(context.InvokeMessage.ServiceId));
            }

            //服务地址解析
            var invokeMessage = context.InvokeMessage;
            var address = await _addressResolver.Resolver(invokeMessage.ServiceId);

            if (address == null)
            {
                throw new RpcException($"无法解析服务Id：{invokeMessage.ServiceId}的地址信息。");
            }

            try
            {
                // 服务远程调用
                var endPoint = address.CreateEndPoint();
                //服务远程调用记录调用IP地址
                _logger.LogDebug($"使用地址：'{endPoint}'进行调用。");
                var client = _transportClientFactory.CreateClient(endPoint);
                var result = await client.SendAsync(context.InvokeMessage);
                return result;
            }
            catch (RpcCommunicationException)
            {
                await _healthCheckService.MarkFailure(address);
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError($"发起请求中发生了错误，服务Id：{invokeMessage.ServiceId}。", exception);
                throw;
            }
        }
    }
}