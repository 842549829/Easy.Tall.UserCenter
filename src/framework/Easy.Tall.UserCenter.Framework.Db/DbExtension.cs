using Microsoft.Extensions.DependencyInjection;

namespace Easy.Tall.UserCenter.Framework.Db
{
    /// <summary>
    /// Db扩展
    /// </summary>
    public static class DbExtension
    {
        /// <summary>
        /// 注册Db服务
        /// </summary>
        /// <param name="services">服务描述符集合的契约</param>
        /// <returns>服务描述符集合的契约</returns>
        public static IServiceCollection AddDbFramework(this IServiceCollection services)
        {
            services.AddScoped<IDbUnitOfWorkFactory, DbUnitOfWorkFactory>();
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            return services;
        }
    }
}
