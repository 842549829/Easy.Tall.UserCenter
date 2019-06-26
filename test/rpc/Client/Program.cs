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
                var userName = serviceProvider.GetService<IUserService>().GetUserName(125454).Result;
                Console.WriteLine(userName);
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
