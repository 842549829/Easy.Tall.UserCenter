using System.Net.Http;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.Services;
using Easy.Tall.UserCenter.WebApi.Extensions;
using Easy.Tall.UserCenter.WebApi.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Easy.Tall.UserCenter.WebApi
{
    /// <summary>
    /// 启动项
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 启动项构造
        /// </summary>
        /// <param name="configuration">配置文件</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 配置文件
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940</param>
        /// <returns>DI容器</returns>
        public void ConfigureServices(IServiceCollection services)
        {
            //添加HttpContext
            services.AddContexts();

            //添加IHttpClientFactory服务
            services.AddHttpClient(Options.DefaultName);

            //添加API文档支持
            services.AddSwagger(Configuration);

            //添加DB服务
            services.AddDbFramework();

            //添加用户服务
            services.AddUserCollection();

            // 添加日志
            services.AddLog(Configuration);

            //添加身份验证
            services.AddJwtAuthentication(Configuration);

            //添加redis缓存
            services.AddRedisCache(Configuration);

            //添加压缩服服务
            services.AddResponseCompression();

            //添加MVC框架
            services.AddMvcBuilder();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">app</param>
        /// <param name="env">env</param>
        /// <param name="httpClientFactory">http请求工厂</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IHttpClientFactory httpClientFactory)
        {
            // 全局异常处理
            app.UseApplicationExceptionHandler();

            //添加配置文件生成中间件
            app.UseSwaggerApiDoc(httpClientFactory);

            //添加权限验证中间件
            app.UseAuthentication();

            //添加压缩中间件
            app.UseResponseCompression();

            //添加Token检测
            app.UseLoginCheck();

            //添加MVC框架组件
            app.UseMvc();
        }
    }
}