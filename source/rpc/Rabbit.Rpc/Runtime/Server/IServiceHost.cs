﻿using System;
using System.Net;
using System.Threading.Tasks;

namespace Rabbit.Rpc.Runtime.Server
{
    /// <summary>
    /// 一个抽象的服务主机
    /// </summary>
    public interface IServiceHost : IDisposable
    {
        /// <summary>
        /// 启动主机
        /// </summary>
        /// <param name="endPoint">主机总结点</param>
        /// <returns>一个任务</returns>
        Task StartAsync(EndPoint endPoint);
    }
}