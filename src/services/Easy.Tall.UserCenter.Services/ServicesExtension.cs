using CSRedis;
using Easy.Tall.UserCenter.IServices;
using Easy.Tall.UserCenter.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
            services.TryAddRepository();
            services.TryAddScoped<IClassifyService, ClassifyService>();
            services.TryAddScoped<IEnumAttributeService, EnumAttributeService>();
            services.TryAddSingleton<IPermissionCacheService, PermissionCacheService>();
            services.TryAddScoped<IPermissionService, PermissionService>();
            services.TryAddSingleton<IRedisCacheService<CSRedisClient>, RedisCacheService>();
            services.TryAddScoped<IRoleService, RoleService>();
            services.TryAddScoped<IUserService, UserService>();
            return services;
        }
    }
}