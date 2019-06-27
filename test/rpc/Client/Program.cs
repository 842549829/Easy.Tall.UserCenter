using System;
using System.Collections.Generic;
using System.Diagnostics;
using Framework.Rpc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Rabbit.Rpc.Exceptions;
using RpcLib;
using RpcLib.Models;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            SingleThread(GetServices(new[] { typeof(IUserService) }));
        }

        static void SingleThread(IServiceProvider serviceProvider)
        {
            Console.WriteLine("按任意键开始,Q退出...");
            try
            {
                // 简单测试
                var userName = serviceProvider.GetService<IUserService>().GetUserName(125454).Result;
                Console.WriteLine(userName);

                // 带当前上下文的测试
                var serviceProxy = ServiceProxy.CreateServiceProxy(serviceProvider);
                var userService = serviceProxy.Generate<IUserService>(new ServiceContext
                {
                    Key = "W_AK",
                    Name = "流动",
                    UserId = "58964145"
                });
                var result = userService.GetUserId("test");
            }
            catch (RpcRemoteException remoteException)
            {
                Console.WriteLine(remoteException.Message);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            Console.ReadKey();
        }

        static IServiceProvider GetServices(IEnumerable<Type> interfaces)
        {
            var serviceCollection = new ServiceCollection();
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("AppSettings.json", true, true)
                .AddJsonFile($"AppSettings.{env}.json", true, true)
                .Build();

            serviceCollection.Configure<ServiceDiscoveryOptions>(config.GetSection("ServiceDiscoveryOptions"));
            serviceCollection
                .AddOptions()
                .AddLogging(builder =>
                {
                    builder.AddConfiguration(config.GetSection("Logging"));
                    builder.AddNLog();
                    builder.AddConsole();
                })
                .AddDefaultRpcClient(interfaces);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
