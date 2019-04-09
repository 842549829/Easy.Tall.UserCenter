using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rabbit.Rpc.Serialization;

namespace Rabbit.Rpc.Routing.Implementation
{
    /// <summary>
    /// 服务路由管理者基类。
    /// </summary>
    public abstract class ServiceRouteManagerBase : IServiceRouteManager
    {
        /// <summary>
        /// 序列化器
        /// </summary>
        private readonly ISerializer<string> _serializer;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serializer">序列化器</param>
        protected ServiceRouteManagerBase(ISerializer<string> serializer)
        {
            _serializer = serializer;
        }

        /// <summary>
        /// 服务路由被创建
        /// </summary>
        public event EventHandler<ServiceRouteEventArgs> Created;
        
        /// <summary>
        /// 服务路由被删除。
        /// </summary>
        public event EventHandler<ServiceRouteEventArgs> Removed;

        /// <summary>
        /// 服务路由被修改。
        /// </summary>
        public event EventHandler<ServiceRouteChangedEventArgs> Changed;

        /// <summary>
        /// 获取所有可用的服务路由信息。
        /// </summary>
        /// <returns>服务路由集合。</returns>
        public abstract Task<IEnumerable<ServiceRoute>> GetRoutesAsync();

        /// <summary>
        /// 清空所有的服务路由。
        /// </summary>
        /// <returns>一个任务。</returns>
        public abstract Task ClearAsync();

        /// <summary>
        /// 设置服务路由。
        /// </summary>
        /// <param name="routes">服务路由集合。</param>
        /// <returns>一个任务。</returns>
        public Task SetRoutesAsync(IEnumerable<ServiceRoute> routes)
        {
            if (routes == null)
            {
                throw new ArgumentNullException(nameof(routes));
            }
            var descriptors = routes.Where(route => route != null).Select(route => new ServiceRouteDescriptor
            {
                AddressDescriptors = route.Address?.Select(address => new ServiceAddressDescriptor
                {
                    Type = address.GetType().FullName,
                    Value = _serializer.Serialize(address)
                }) ?? Enumerable.Empty<ServiceAddressDescriptor>(),
                ServiceDescriptor = route.ServiceDescriptor
            });

            return SetRoutesAsync(descriptors);
        }

        /// <summary>
        /// 设置服务路由。
        /// </summary>
        /// <param name="routes">服务路由集合。</param>
        /// <returns>一个任务。</returns>
        protected abstract Task SetRoutesAsync(IEnumerable<ServiceRouteDescriptor> routes);

        /// <summary>
        /// 服务路由被创建
        /// </summary>
        /// <param name="args">参数</param>
        protected void OnCreated(params ServiceRouteEventArgs[] args)
        {
            if (Created == null)
            {
                return;
            }
            foreach (var arg in args)
            {
                Created(this, arg);
            }
        }

        /// <summary>
        /// 服务路由被修改
        /// </summary>
        /// <param name="args">参数</param>
        protected void OnChanged(params ServiceRouteChangedEventArgs[] args)
        {
            if (Changed == null)
            {
                return;
            }
            foreach (var arg in args)
            {
                Changed(this, arg);
            }
        }

        /// <summary>
        /// 服务路由被删除
        /// </summary>
        /// <param name="args">参数</param>
        protected void OnRemoved(params ServiceRouteEventArgs[] args)
        {
            if (Removed == null)
            {
                return;
            }
            foreach (var arg in args)
            {
                Removed(this, arg);
            }
        }
    }
}