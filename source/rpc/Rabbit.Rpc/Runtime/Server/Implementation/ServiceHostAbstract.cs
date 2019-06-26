using System.Net;
using System.Threading.Tasks;
using Rabbit.Rpc.Messages;
using Rabbit.Rpc.Transport;
using Rabbit.Rpc.Transport.Implementation;

namespace Rabbit.Rpc.Runtime.Server.Implementation
{
    public abstract class ServiceHostAbstract : IServiceHost
    {
        /// <summary>
        /// 服务执行器
        /// </summary>
        private readonly IServiceExecutor _serviceExecutor;

        /// <summary>
        /// 消息监听者。
        /// </summary>
        protected IMessageListener MessageListener { get; } = new MessageListener();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceExecutor">服务执行器</param>
        protected ServiceHostAbstract(IServiceExecutor serviceExecutor)
        {
            _serviceExecutor = serviceExecutor;
            MessageListener.Received += MessageListener_Received;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// 启动主机。
        /// </summary>
        /// <param name="endPoint">主机终结点。</param>
        /// <returns>一个任务。</returns>
        public abstract Task StartAsync(EndPoint endPoint);

        /// <summary>
        /// 接收到消息事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="message">消息</param>
        /// <returns>一个异步任务</returns>
        private async Task MessageListener_Received(IMessageSender sender, TransportMessage message)
        {
            await _serviceExecutor.ExecuteAsync(sender, message);
        }
    }
}