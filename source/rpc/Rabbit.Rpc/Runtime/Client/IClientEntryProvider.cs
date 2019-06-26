using System;
using System.Collections.Generic;

namespace Rabbit.Rpc.Runtime.Client
{
    /// <summary>
    /// 客户端服务条目提供程序
    /// </summary>
    public interface IClientEntryProvider
    {
        /// <summary>
        /// 获取客户端服务条目
        /// </summary>
        /// <returns>客户端服务条目</returns>
        IEnumerable<ServiceEntry> GetEntries();
    }
}
