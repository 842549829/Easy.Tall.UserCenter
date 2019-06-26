using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rabbit.Rpc.Exceptions;
using Rabbit.Rpc.Messages;
using Rabbit.Rpc.Runtime.Server;

namespace Rabbit.Rpc.Transport.Implementation
{
    /// <summary>
    /// 一个默认的传输客户端实现。
    /// </summary>
    public class TransportClient: ITransportClient, IDisposable
    {
        /// <summary>
        /// 消息发送者
        /// </summary>
        private readonly IMessageSender _messageSender;

        /// <summary>
        /// 消息监听者
        /// </summary>
        private readonly IMessageListener _messageListener;

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// 服务执行器
        /// </summary>
        private readonly IServiceExecutor _serviceExecutor;

        /// <summary>
        /// 传输消息
        /// </summary>
        private readonly ConcurrentDictionary<string, TaskCompletionSource<TransportMessage>> _resultDictionary =
            new ConcurrentDictionary<string, TaskCompletionSource<TransportMessage>>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="messageSender">消息发送者</param>
        /// <param name="messageListener">消息监听者</param>
        /// <param name="logger">日志</param>
        /// <param name="serviceExecutor">服务执行器</param>
        public TransportClient(IMessageSender messageSender, IMessageListener messageListener, ILogger logger,
            IServiceExecutor serviceExecutor)
        {
            _messageSender = messageSender;
            _messageListener = messageListener;
            _logger = logger;
            _serviceExecutor = serviceExecutor;
            messageListener.Received += MessageListener_Received;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">远程调用消息模型</param>
        /// <returns>远程调用消息的传输信息</returns>
        public async Task<RemoteInvokeResultMessage> SendAsync(RemoteInvokeMessage message)
        {
            try
            {
                _logger.LogDebug("准备发送消息。");
                var transportMessage = TransportMessage.CreateInvokeMessage(message);

                //注册结果回调
                var callbackTask = RegisterResultCallbackAsync(transportMessage.Id);

                try
                {
                    //发送
                    await _messageSender.SendAndFlushAsync(transportMessage);
                }
                catch (Exception exception)
                {
                    throw new RpcCommunicationException("与服务端通讯时发生了异常。", exception);
                }

                _logger.LogDebug("消息发送成功。");
                return await callbackTask;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "消息发送失败。");
                throw;
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _messageSender?.Dispose();
            _messageListener?.Dispose();
            foreach (var taskCompletionSource in _resultDictionary.Values)
            {
                taskCompletionSource.TrySetCanceled();
            }
        }

        /// <summary>
        /// 注册指定消息的回调任务。
        /// </summary>
        /// <param name="id">消息Id。</param>
        /// <returns>远程调用结果消息模型。</returns>
        private async Task<RemoteInvokeResultMessage> RegisterResultCallbackAsync(string id)
        {
            _logger.LogDebug($"准备获取Id为：{id}的响应内容。");
            var task = new TaskCompletionSource<TransportMessage>();
            _resultDictionary.TryAdd(id, task);
            try
            {
                var result = await task.Task;
                return result.GetContent<RemoteInvokeResultMessage>();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "注册指定消息的回调任务");
                return null;
            }
            finally
            {
                //删除回调任务
                _resultDictionary.TryRemove(id, out _);
            }
        }

        /// <summary>
        /// 接收消息事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="message">消息</param>
        /// <returns>一个异步任务</returns>
        private async Task MessageListener_Received(IMessageSender sender, TransportMessage message)
        {
            _logger.LogDebug("接收到消息。");
            if (!_resultDictionary.TryGetValue(message.Id, out var task))
            {
                return;
            }
            if (message.IsInvokeResultMessage())
            {
                //远程出现错误将错误如实写入消息体中
                task.SetResult(message);
            }
            if (_serviceExecutor != null && message.IsInvokeMessage())
            {
                await _serviceExecutor.ExecuteAsync(sender, message);
            }
        }
    }
}
