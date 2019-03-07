using Easy.Tall.UserCenter.IServices;
using Easy.Tall.UserCenter.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Easy.Tall.UserCenter.Services
{
    /// <summary>
    /// 用户扩展类
    /// </summary>
    public static class UserExtension
    {
        /// <summary>
        /// 添加用户服务
        /// </summary>
        /// <param name="services">容器</param>
        /// <returns>容器接口</returns>
        public static IServiceCollection AddUserCollection(this IServiceCollection services)
        {
            services.AddRepository();
            services.AddScoped<IUserServices, UserServices>();
            return services;
        }
    }
}