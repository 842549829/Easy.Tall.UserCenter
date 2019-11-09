using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Easy.Tall.UserCenter.Framework.Configuration.Consul
{
    /// <summary>
    /// consul配置监听宿主
    /// </summary>
    public class ConsulConfigurationHostedService : IHostedService
    {
        /// <summary>
        /// 信号标记
        /// </summary>
        private static readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// 用于启用阻塞查询。等待超时或到达下一个索引
        /// </summary>
        private ulong LastIndex { get; set; }

        /// <summary>
        /// consul在线配置
        /// </summary>
        private ConsulConfigurationOptions HostedServiceOptions { get; }

        /// <summary>
        /// 日志
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hostedServiceOptionsAccessor">consul在线配置</param>
        /// <param name="logger">日志</param>
        public ConsulConfigurationHostedService(IOptions<ConsulConfigurationOptions> hostedServiceOptionsAccessor, ILogger<ConsulConfigurationHostedService> logger)
        {
            HostedServiceOptions = hostedServiceOptionsAccessor.Value;
            Logger = logger;
        }

        /// <summary>
        /// 当应用程序主机准备启动服务时触发
        /// </summary>
        /// <param name="cancellationToken">指示启动进程已中止</param>
        /// <returns>异步操作</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                do
                {
                    try
                    {
                        await QueryConsulAsync();
                        //每次请求consul连接都需要使用主线程休眠5秒 防止consul挂掉无法阻塞线程 
                        Thread.Sleep(5000);
                    }
                    catch (TaskCanceledException e)
                    {
                        Logger.LogInformation("Query consul has been canceld.Message:{Message}", e.Message);
                    }
                    catch (Exception e)
                    {
                        Logger.LogError($"{e.Message}");
                    }
                } while (!CancellationTokenSource.IsCancellationRequested);
            }, CancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        /// <summary>
        /// 查询consul配置中心
        /// </summary>
        /// <returns>异步操作</returns>
        private async Task QueryConsulAsync()
        {
            using (var client = new ConsulClient(options =>
            {
                options.WaitTime = ObserverManager.Configuration.ClientConfiguration.WaitTime;
                options.Token = ObserverManager.Configuration.ClientConfiguration.Token;
                options.Datacenter = ObserverManager.Configuration.ClientConfiguration.Datacenter;
                options.Address = ObserverManager.Configuration.ClientConfiguration.Address;
            }))
            {
                var result = await client.KV.List(ObserverManager.Configuration.QueryOptions.Folder, new QueryOptions
                {
                    Token = ObserverManager.Configuration.ClientConfiguration.Token,
                    Datacenter = ObserverManager.Configuration.ClientConfiguration.Datacenter,
                    WaitIndex = LastIndex,
                    WaitTime = TimeSpan.FromSeconds(HostedServiceOptions.BlockingQueryWaitSeconds)
                }, CancellationTokenSource.Token);
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                    return;
                if (result.LastIndex > LastIndex)
                {
                    LastIndex = result.LastIndex;
                    ObserverManager.Notify(result.Response.ToList(), Logger);
                }
            }
        }

        /// <summary>
        /// 当应用程序主机执行优雅的关闭时触发
        /// </summary>
        /// <param name="cancellationToken">指示启动进程已中止</param>
        /// <returns>异步操作</returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            CancellationTokenSource.Cancel();
            await Task.CompletedTask;
        }
    }
}