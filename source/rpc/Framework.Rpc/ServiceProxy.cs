using System;
using Rabbit.Rpc.ProxyGenerator.Implementation;

namespace Framework.Rpc
{
    /// <summary>
    /// Rpc代理生成类
    /// </summary>
    public sealed class ServiceProxy
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceProvider">服务容器</param>
        private ServiceProxy(IServiceProvider serviceProvider)
        {
            Services = serviceProvider;
        }

        /// <summary>
        /// 创建代理
        /// </summary>
        /// <param name="serviceProvider">服务容器</param>
        /// <returns>ServiceProxy</returns>
        public static ServiceProxy CreateServiceProxy(IServiceProvider serviceProvider)
        {
            return new ServiceProxy(serviceProvider);
        }

        /// <summary>
        /// 容器服务
        /// </summary>
        public IServiceProvider Services { get; private set; }

        /// <summary>
        /// 生成服务
        /// </summary>
        /// <typeparam name="T">服务接口类型</typeparam>
        /// <returns>rpc代理服务</returns>
        public T Generate<T>(object serviceContext) where T : class
        {
            var instance = Services.GetService(typeof(T)) as T;
            if (instance != null && instance.GetType().IsSubclassOf(typeof(ServiceProxyBase)))
            {
                if (instance is ServiceProxyBase server)
                {
                    server.RpcContext = serviceContext;
                }
            }
            return instance;
        }
    }
}