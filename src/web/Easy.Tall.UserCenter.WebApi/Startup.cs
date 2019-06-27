﻿using System.Net.Http;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.Services;
using Easy.Tall.UserCenter.WebApi.Extensions;
using Easy.Tall.UserCenter.WebApi.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;

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

            //添加模型验证统一返回结果
            services.AddApiBehaviorOptions();

            // 添加日志
            services.AddLogging(configure => { configure.AddNLog(); });

            //添加身份验证
            services.AddJwtAuthentication(Configuration);

            //添加MVC框架
            services.AddMvcBuilder().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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

            //添加权限验证
            app.UseAuthentication();

            //添加MVC框架组件
            app.UseMvc();
        }
    }
}