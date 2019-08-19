using System;
using System.Collections.Generic;
using System.IO;
using Easy.Tall.UserCenter.WebApi.Attribute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using CSRedis;
using Easy.Tall.UserCenter.Entity.Extend.Options;
using Easy.Tall.UserCenter.Framework.Constant;
using Easy.Tall.UserCenter.Framework.Data;
using Easy.Tall.UserCenter.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Easy.Tall.UserCenter.WebApi.Extensions
{
    /// <summary>
    /// Api容器扩展
    /// </summary>
    public static class ApiIocExtension
    {
        /// <summary>
        /// 添加MVC服务
        /// </summary>
        /// <param name="services">容器</param>
        /// <returns>容器接口</returns>
        public static IMvcBuilder AddMvcBuilder(this IServiceCollection services)
        {
            return services.AddMvc(config =>
                {   // 配置异常过滤器
                    config.Filters.Add(typeof(ApiExceptionFilterAttribute));
                })
                // 设置json序列化方式
                .AddJsonOptions(mvcJsonOptions =>
                {
                    //忽略循环引用
                    mvcJsonOptions.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    ////不使用驼峰样式的key
                    //mvcJsonOptions.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //设置时间格式
                    //mvcJsonOptions.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                })
                // 设置型验证统一返回结果
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        var errors = actionContext.ModelState
                            .Where(e => e.Value.Errors.Count > 0)
                            .Select(e => new Result<string>
                            {
                                Code = 400,
                                Msg = e.Value.Errors.First().ErrorMessage,
                                Data = e.Key
                            }).FirstOrDefault();
                        return new BadRequestObjectResult(errors);
                    };
                })
                // 设置版本
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        /// <summary>
        /// 添加HttpContext
        /// </summary>
        /// <param name="services">容器</param>
        /// <returns>容器接口</returns>
        public static IServiceCollection AddContexts(this IServiceCollection services)
        {
            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }

        /// <summary>
        /// 添加Swagger
        /// </summary>
        /// <param name="services">容器</param>
        /// <param name="configuration">配置文件</param>
        /// <returns>容器接口</returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
#if DEBUG
            services.Configure<ApiDocOptions>(configuration.GetSection(AppSettingsSection.ApiDoc));
            services.AddSwaggerGen(options =>
            {
                var provider = services.BuildServiceProvider();
                var apiDocOption = provider.GetService<IOptions<ApiDocOptions>>().Value;
                options.SwaggerDoc(apiDocOption.ApiName, new Swashbuckle.AspNetCore.Swagger.Info { Title = apiDocOption.Title, Version = apiDocOption.Version });
                var files = Directory.GetFiles(AppContext.BaseDirectory).Where(n => n.EndsWith(".xml"));
                foreach (var file in files)
                {
                    options.IncludeXmlComments(file);
                }
            });
#endif
            return services;
        }

        /// <summary>
        /// 注册身份证验证
        /// </summary>
        /// <param name="services">容器</param>
        /// <param name="configuration">配置文件</param>
        /// <returns>容器接口</returns>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SsoOptions>(configuration.GetSection(AppSettingsSection.Sso));
            services.AddScoped<JwtTokenValidator>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var provider = services.BuildServiceProvider();
                var validator = provider.GetService<JwtTokenValidator>();
                o.SecurityTokenValidators.Add(validator);
            });
            return services;
        }

        /// <summary>
        /// 添加RedisCache缓存
        /// </summary>
        /// <param name="services">容器</param>
        /// <param name="configuration">配置文件</param>
        /// <returns>容器接口</returns>
        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            //csRedis实例
            var redisConfig = configuration.GetSection(AppSettingsSection.Redis).Get<Dictionary<string, string[]>>();
            redisConfig.TryGetValue(AppSettingsSection.User, out var redisConnectionString);
            var csRedis = new CSRedisClient(NodeRule: null, redisConnectionString);
            // 初始化 RedisHelper
            RedisHelper.Initialization(csRedis);
            //注册redis实例
            services.TryAddSingleton(RedisHelper.Instance);
            //注册mvc分布式缓存
            services.TryAddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));
            return services;
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="services">容器</param>
        /// <param name="configuration">配置文件</param>
        /// <returns>容器接口</returns>
        public static IServiceCollection AddLog(this IServiceCollection services, IConfiguration configuration)
        {
            // 添加日志
            services.AddLogging(builder =>
            {
                builder.AddConfiguration(configuration.GetSection(AppSettingsSection.Logging));
                builder.AddConsole();
                builder.AddNLog();
            });
            return services;
        }

        /// <summary>
        /// 添加跨越
        /// </summary>
        /// <param name="services">容器</param>
        /// <returns>容器</returns>
        public static IServiceCollection AddCorsService(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(AppSettingsSection.AllowSpecificOrigins,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });
            return services;
        }

        /// <summary>
        /// 全局异常处理
        /// </summary>
        /// <param name="app">app</param>
        /// <returns>app</returns>
        public static IApplicationBuilder UseApplicationExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(
                options =>
                    options.Run(
                        async context =>
                        {
                            var log = app.ApplicationServices.GetService(typeof(Logger)) as Logger;
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.ContentType = "json/application";
                            var ex = context.Features.Get<IExceptionHandlerFeature>();
                            if (ex != null)
                            {
                                var err = new
                                {
                                    code = (int)HttpStatusCode.InternalServerError,
                                    msg = ex.Error.Message,
                                    data = ex.Error.StackTrace
                                };
                                log?.Error("系统错误", ex);
                                await context.Response.WriteAsync(JsonConvert.SerializeObject(err));
                            }
                        })
            );
            return app;
        }
    }
}