using System;
using System.IO;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.WebApi.Attribute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq;
using Easy.Tall.UserCenter.Entity.Constant;
using Easy.Tall.UserCenter.WebApi.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Easy.Tall.UserCenter.WebApi.Extensions
{
    /// <summary>
    /// Api容器扩展
    /// </summary>
    public static class ApiIocExtension
    {
        /// <summary>
        /// 添加配置文件强类型节点
        /// </summary>
        /// <param name="services">容器</param>
        /// <param name="configuration">配置文件</param>
        /// <returns>容器接口</returns>
        public static IServiceCollection AddConfigure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiDocOptions>(configuration.GetSection(AppSettingsSection.ApiDoc));
            return services;
        }

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
                options.InvalidModelStateResponseFactory = context =>
                {
                    var result = new Result<object>
                    {
                        Code = 400,
                        Msg = string.Join(",", context.ModelState.Select(d => d.Value.Errors.First().ErrorMessage)),
                        Data = context.ModelState.Select(d => new { d.Key, d.Value.Errors.First().ErrorMessage })
                    };
                    return new BadRequestObjectResult(result);
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
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }

        /// <summary>
        /// 添加Swagger
        /// </summary>
        /// <param name="services">容器</param>
        /// <returns>容器接口</returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
#if DEBUG
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
    }
}