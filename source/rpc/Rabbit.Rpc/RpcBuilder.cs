using System;
using Microsoft.Extensions.DependencyInjection;

namespace Rabbit.Rpc
{
    /// <summary>
    /// 默认的Rpc服务构建者。
    /// </summary>
    internal sealed class RpcBuilder : IRpcBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="services">服务容器集合</param>
        public RpcBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// 服务集合。
        /// </summary>
        public IServiceCollection Services { get; }
    }
}