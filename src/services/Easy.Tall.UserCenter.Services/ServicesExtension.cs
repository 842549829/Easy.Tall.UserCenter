﻿using Easy.Tall.UserCenter.IServices;
using Easy.Tall.UserCenter.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Easy.Tall.UserCenter.Services
{
    /// <summary>
    /// Services扩展类
    /// </summary>
    public static class ServicesExtension
    {
        /// <summary>
        /// 添加用户服务
        /// </summary>
        /// <param name="services">容器</param>
        /// <returns>容器接口</returns>
        public static IServiceCollection AddUserCollection(this IServiceCollection services)
        {
            services.AddRepository();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IClassifyService, ClassifyService>();
            services.AddTransient<IRoleService, RoleService>();
            return services;
        }
    }
}