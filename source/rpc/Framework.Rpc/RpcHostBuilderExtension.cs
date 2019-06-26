using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Framework.Rpc
{
    /// <summary>
    /// RpcHost扩展
    /// </summary>
    public static class RpcHostBuilderExtension
    {
        /// <summary>
        /// 创建默认的Rpc服务宿主
        /// </summary>
        /// <param name="builder">IHostBuilder</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder CreateServerRpcHostBuilder(this IHostBuilder builder)
        {
            return builder.ConfigureAppConfiguration((hostContext, configApp) =>
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                configApp.SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appSettings.json".ToLower(), true, true)
                    .AddJsonFile($"appSettings.{env}.json".ToLower(), true, true);
            }).ConfigureServices((hostContext, services) =>
            {
                var config = hostContext.Configuration;
                services.Configure<ServiceDiscoveryOptions>(config.GetSection("ServiceDiscoveryOptions"));
                services.AddDefaultRpcServer();
                services.AddHostedService<RpcHost>();
                services.AddLogging(logBuilder =>
                {
                    logBuilder.AddConfiguration(config.GetSection("Logging"));
                    logBuilder.AddConsole();
                });
            });
        }
    }
}