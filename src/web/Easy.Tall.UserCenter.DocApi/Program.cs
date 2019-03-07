using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Easy.Tall.UserCenter.DocApi
{
    /// <summary>
    /// 程序入口
    /// 启动命令 dotnet Holder.ERP.Picking.WebApi.dll --project=E:\code\picking\WebApi\Holder.ERP.Picking.WebApi --urls=http://localhost:5001/
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 程序Main方法
        /// </summary>
        /// <param name="args">args</param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 创建IRpcHostBuilder
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>IRpcHostBuilder</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}