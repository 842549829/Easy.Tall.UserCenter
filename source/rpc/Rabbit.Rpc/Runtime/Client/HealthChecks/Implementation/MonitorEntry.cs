using System.Net;
using Rabbit.Rpc.Address;

namespace Rabbit.Rpc.Runtime.Client.HealthChecks.Implementation
{
    /// <summary>
    /// 监听条目
    /// </summary>
    public class MonitorEntry
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="addressModel">服务地址</param>
        /// <param name="health">是否健康</param>
        public MonitorEntry(AddressModel addressModel, bool health = true)
        {
            EndPoint = addressModel.CreateEndPoint();
            Health = health;
        }

        /// <summary>
        /// 总结点
        /// </summary>
        public EndPoint EndPoint { get; set; }

        /// <summary>
        /// 是否健康
        /// </summary>
        public bool Health { get; set; }
    }
}