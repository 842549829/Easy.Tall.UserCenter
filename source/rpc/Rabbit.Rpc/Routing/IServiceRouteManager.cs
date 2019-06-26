using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rabbit.Rpc.Routing.Event;

namespace Rabbit.Rpc.Routing
{
    /// <summary>
    /// 一个抽象的服务路由发现者
    /// </summary>
    public interface IServiceRouteManager
    {
        /// <summary>
        /// 服务路由被创建
        /// </summary>
        event EventHandler<ServiceRouteEventArgs> Created;

        /// <summary>
        /// 服务路由被删除
        /// </summary>
        event EventHandler<ServiceRouteEventArgs> Removed;

        /// <summary>
        /// 服务路由被修改
        /// </summary>
        event EventHandler<ServiceRouteChangedEventArgs> Changed;

        /// <summary>
        /// 设置服务理由
        /// </summary>
        /// <param name="routes">服务路由集合</param>
        /// <returns>一个任务</returns>
        Task SetRouteAsync(IEnumerable<ServiceRoute> routes);

        /// <summary>
        /// 清空所有的服务路由。
        /// </summary>
        /// <returns>一个任务。</returns>
        Task ClearAsync();
    }
}