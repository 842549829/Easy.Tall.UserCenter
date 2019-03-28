using System;
using System.Collections.Generic;
using System.Text;
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
        /// 服务路由被创建
        /// </summary>
        private EventHandler<ServiceRouteEventArgs> _created;

        /// <summary>
        /// 服务路由被删除
        /// </summary>
        private EventHandler<ServiceRouteEventArgs> _removed;

        /// <summary>
        /// 服务路由被修改
        /// </summary>
        private EventHandler<ServiceRouteChangedEventArgs> _changed;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serializer">序列化器</param>
        protected ServiceRouteManagerBase(ISerializer<string> serializer)
        {
            _serializer = serializer;
        }

        public event EventHandler<ServiceRouteEventArgs> Created
        {
            add => _created += value;
            remove => _created -= value;
        }


    }
}