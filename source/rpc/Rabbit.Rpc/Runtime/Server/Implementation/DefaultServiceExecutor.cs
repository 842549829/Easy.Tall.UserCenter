using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rabbit.Rpc.Messages;
using Rabbit.Rpc.Transport;

namespace Rabbit.Rpc.Runtime.Server.Implementation
{
    /// <summary>
    /// 服务执行者
    /// </summary>
    public class DefaultServiceExecutor : IServiceExecutor
    {

        /// <summary>
        /// 服务定位器
        /// </summary>
        private readonly IServiceEntryLocate _serviceEntryLocate;

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<DefaultServiceExecutor> _logger;

        /// <summary>
        /// 容器服务提供程小集
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceEntryLocate">服务定位器</param>
        /// <param name="serviceProvider">容器服务提供程小集</param>
        /// <param name="logger">日志记录器</param>
        public DefaultServiceExecutor(IServiceEntryLocate serviceEntryLocate,
            IServiceProvider serviceProvider,
            ILogger<DefaultServiceExecutor> logger)
        {
            _serviceEntryLocate = serviceEntryLocate;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        /// <summary>
        /// 服务执行
        /// </summary>
        /// <param name="sender">消息发送者</param>
        /// <param name="message">调用消息</param>
        /// <returns>一个异步任务</returns>
        public async Task ExecuteAsync(IMessageSender sender, TransportMessage message)
        {
            _logger.LogInformation("接收到消息。");
            if (!message.IsInvokeMessage())
            {
                return;
            }
            RemoteInvokeMessage remoteInvokeMessage;
            try
            {
                remoteInvokeMessage = message.GetContent<RemoteInvokeMessage>();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "将接收到的消息反序列化成 TransportMessage<RemoteInvokeMessage> 时发送了错误。");
                return;
            }

            var entry = _serviceEntryLocate.Locate(remoteInvokeMessage);
            if (entry == null)
            {
                _logger.LogError($"根据服务Id：{remoteInvokeMessage.ServiceId}，找不到服务条目。");
                return;
            }

            _logger.LogDebug($"服务Id为:{remoteInvokeMessage.ServiceId} 的服务开始执行 接收参数:", remoteInvokeMessage);
            var resultMessage = new RemoteInvokeResultMessage();

            //是否需要等待执行。
            if (entry.Descriptor.WaitExecution())
            {
                //执行本地代码。
                await LocalExecuteAsync(entry, remoteInvokeMessage, resultMessage);
                //向客户端发送调用结果。
                await SendRemoteInvokeResult(sender, message.Id, resultMessage);
            }
            else
            {
                //通知客户端已接收到消息。
                await SendRemoteInvokeResult(sender, message.Id, resultMessage);
                //确保新起一个线程执行，不堵塞当前线程。
                await Task.Factory.StartNew(async () =>
                {
                    //执行本地代码。
                    await LocalExecuteAsync(entry, remoteInvokeMessage, resultMessage);
                }, TaskCreationOptions.LongRunning);
            }
            _logger.LogDebug($"服务Id为:{remoteInvokeMessage.ServiceId} 的服务结束执行 返回参数:", resultMessage);
        }


        /// <summary>
        /// 本地异步执行
        /// </summary>
        /// <param name="entry">服务条目</param>
        /// <param name="remoteInvokeMessage">远程调用消息</param>
        /// <param name="resultMessage">远程调用消息结果</param>
        /// <returns>异步执行结果</returns>
        private async Task LocalExecuteAsync(ServiceEntry entry, RemoteInvokeMessage remoteInvokeMessage, RemoteInvokeResultMessage resultMessage)
        {
            try
            {
                var serviceScopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var result = await entry.Func(remoteInvokeMessage, scope);
                    var task = result as Task;

                    if (task == null)
                    {
                        resultMessage.Result = result;
                    }
                    else
                    {
                        task.Wait();

                        var taskType = task.GetType().GetTypeInfo();
                        if (taskType.IsGenericType)
                        {
                            resultMessage.Result = taskType.GetProperty("Result")?.GetValue(task);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "执行本地逻辑时候发生了错误。");
                resultMessage.ExceptionMessage = GetExceptionMessage(exception);
                resultMessage.ExceptionCode = GetExceptionCode(exception);
            }
        }

        /// <summary>
        /// 发送远程调用信息
        /// </summary>
        /// <param name="sender">消息发送者</param>
        /// <param name="messageId">消息Id</param>
        /// <param name="resultMessage">消息结果</param>
        /// <returns>异步结果</returns>
        private async Task SendRemoteInvokeResult(IMessageSender sender, string messageId, RemoteInvokeResultMessage resultMessage)
        {
            try
            {
                _logger.LogDebug("准备发送响应消息。");

                await sender.SendAndFlushAsync(TransportMessage.CreateInvokeResultMessage(messageId, resultMessage));
                _logger.LogDebug("响应消息发送成功。");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "发送响应消息时候发生了异常。");
            }
        }

        /// <summary>
        /// 获取异常信息
        /// </summary>
        /// <param name="exception">异常</param>
        /// <returns>异常结果</returns>
        private static string GetExceptionMessage(Exception exception)
        {
            if (exception == null)
                return string.Empty;

            var message = exception.Message;
            if (exception.InnerException != null)
            {
                return GetExceptionMessage(exception.InnerException);
            }
            return message;
        }

        /// <summary>
        /// 获取异常信息
        /// </summary>
        /// <param name="exception">异常</param>
        /// <returns>异常结果</returns>
        private static int GetExceptionCode(Exception exception)
        {
            if (exception == null)
                return 0;
            if (exception.InnerException != null)
            {
                return GetExceptionCode(exception.InnerException);
            }
            return exception.HResult;
        }

    }
}