using System;
using System.Net;
using System.Threading.Tasks;
using Rabbit.Rpc.Transport;

namespace Rabbit.Rpc.Runtime.Server.Implementation
{
    /// <summary>
    /// 一个默认的服务主机
    /// </summary>
    public class DefaultServiceHost : ServiceHostAbstract
    {
        /// <summary>
        /// 消息监听工厂
        /// </summary>
        private readonly Func<EndPoint, Task<IMessageListener>> _messageListenerFactory;

        /// <summary>
        /// 消息监听
        /// </summary>
        private IMessageListener _serverMessageListener;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="messageListenerFactory">消息监听工厂</param>
        /// <param name="serviceExecutor">消息监听</param>
        public DefaultServiceHost(Func<EndPoint, Task<IMessageListener>> messageListenerFactory, IServiceExecutor serviceExecutor) : base(serviceExecutor)
        {
            _messageListenerFactory = messageListenerFactory;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            _serverMessageListener?.Dispose();
        }

        /// <summary>
        /// 启动主机。
        /// </summary>
        /// <param name="endPoint">主机终结点。</param>
        /// <returns>一个任务。</returns>
        public override async Task StartAsync(EndPoint endPoint)
        {
            if (_serverMessageListener != null)
            {
                return;
            }
            _serverMessageListener = await _messageListenerFactory(endPoint);
            _serverMessageListener.Received += async (sender, message) =>
            {
                await Task.Factory.StartNew(() =>
                {
                    MessageListener.OnReceived(sender, message);
                });
            };
        }
    }
}