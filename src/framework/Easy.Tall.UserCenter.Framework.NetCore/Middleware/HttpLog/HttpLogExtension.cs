using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Easy.Tall.UserCenter.Framework.NetCore.Middleware.HttpLog
{
    /// <summary>
    /// HttpLog扩展
    /// </summary>
    public static class HttpLogExtension
    {
        /// <summary>
        /// 添加HttpLog服务
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <returns>IServiceCollection</returns> 
        public static IServiceCollection AddHttpLog(this IServiceCollection services)
        {
            return services.AddHttpLog(config => { });
        }

        /// <summary>
        /// 添加HttpLog服务
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="configuration">IConfiguration</param>
        /// <returns>IServiceCollection</returns> 
        public static IServiceCollection AddHttpLog(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddHttpLogDependency()
                .Configure<HttpLogOption>(configuration);
        }

        /// <summary>
        /// 添加HttpLog服务
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="logOption"></param>
        /// <returns>IServiceCollection</returns> 
        public static IServiceCollection AddHttpLog(this IServiceCollection services, Action<HttpLogOption> logOption)
        {
            return services
                .AddHttpLogDependency()
                .Configure(logOption);
        }

        /// <summary>
        /// 添加HttpLog依赖
        /// </summary>
        /// <param name="services">服务DI</param>
        /// <returns>服务DI</returns>
        private static IServiceCollection AddHttpLogDependency(this IServiceCollection services)
        {
            return services
                .AddOptions()
                .AddLogging();
        }

        /// <summary>
        /// HttpLog中间件
        /// </summary>
        /// <param name="builder">IApplicationBuilder</param>
        /// <returns>IApplicationBuilder</returns>
        public static IApplicationBuilder UseHttpLog(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpLogMiddleware>();
        }
    }
}