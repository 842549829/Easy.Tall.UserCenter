using System;
using System.IO;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.WebApi.Attribute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using Easy.Tall.UserCenter.Framework.Constant;
using Easy.Tall.UserCenter.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NLog;

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

                    //配置模型验证过滤器
                    config.Filters.Add(typeof(ValidateModelAttribute));
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
                });
        }

        /// <summary>
        /// 添加无效的模型状态响应工厂
        /// </summary>
        /// <param name="services">容器</param>
        /// <returns>容器接口</returns>
        public static IServiceCollection AddApiBehaviorOptions(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new Result<string>
                        {
                            Code = 490,
                            Msg = e.Value.Errors.First().ErrorMessage,
                            Data = e.Key
                        }).ToArray();
                    return new BadRequestObjectResult(errors);
                };
            });
            return services;
        }

        /// <summary>
        /// 添加HttpContext
        /// </summary>
        /// <param name="services">容器</param>
        /// <returns>容器接口</returns>
        public static IServiceCollection AddContexts(this IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
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