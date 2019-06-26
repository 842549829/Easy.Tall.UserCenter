using System;
using System.Collections.Generic;

namespace Rabbit.Rpc.Runtime.Client.Implementation.ServiceDiscovery
{
    /// <summary>
    /// 客户端服务条目
    /// </summary>
    public interface IClrClientEntryFactory
    {
        /// <summary>
        /// 创建服务条目。
        /// </summary>
        /// <param name="service">服务类型。</param>
        /// <returns>服务条目集合。</returns>
        IEnumerable<ServiceEntry> CreateServiceEntry(Type service);
    }
}