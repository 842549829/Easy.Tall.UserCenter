using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rabbit.Rpc.Routing.Event;

namespace Rabbit.Rpc.Routing.Implementation
{
    /// <summary>
    /// 客户端路由管理者基类。
    /// </summary>
    public abstract class ClientRouteManagerBase : IClientRouteManager
    {
        /// <summary>
        /// 服务路由被创建。
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
        /// 服务路由被修改
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