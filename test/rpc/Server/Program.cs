using System;
using Framework.Rpc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using RpcLib;
using RpcLibService;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateRpcHostBuilder(args).Build().StartAsync();
        }

        public static IHostBuilder CreateRpcHostBuilder(string[] args)
        {
            return new HostBuilder()
                .CreateServerRpcHostBuilder()
                .ConfigureServices((configBuilder, services) =>
                {
                    services.AddLogging(builder =>
                    {
                        builder.AddNLog();
                    });
                    services.AddScoped<IContextAccessor, EchoContextAccessor>();
                    services.AddRpcServiceAttribute<RpcServiceAttribute>();
                    AddCollection(services);
                })
                .UseConsoleLifetime();
        }

        public static IServiceCollection AddCollection(IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            return services;
        }
    }
}