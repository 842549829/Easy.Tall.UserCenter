namespace Rabbit.Rpc.Routing.Event
{
    /// <summary>
    /// 服务理由事件参数
    /// </summary>
    public class ServiceRouteEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="route">服务路由</param>
        public ServiceRouteEventArgs(ServiceRoute route)
        {
            Route = route;
        }

        /// <summary>
        /// 服务路由信息
        /// </summary>
        public ServiceRoute Route { get; set; }
    }
}