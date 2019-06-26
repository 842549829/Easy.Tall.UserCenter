namespace Rabbit.Rpc.Routing.Event
{
    /// <summary>
    /// 服务路由变更事件参数
    /// </summary>
    public class ServiceRouteChangedEventArgs : ServiceRouteEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="route">服务路由</param>
        /// <param name="oldRoute">旧的服务路由</param>
        public ServiceRouteChangedEventArgs(ServiceRoute route, ServiceRoute oldRoute) : base(route)
        {
            OldRoute = oldRoute;
        }

        /// <summary>
        /// 旧的服务路由信息。
        /// </summary>
        public ServiceRoute OldRoute { get; set; }
    }
}