using Easy.Tall.UserCenter.IRepository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Easy.Tall.UserCenter.Repository
{
    /// <summary>
    /// 仓储扩展
    /// </summary>
    public static class RepositoryExtension
    {
        /// <summary>
        /// 添加用户服务
        /// </summary>
        /// <param name="services">容器</param>
        /// <returns>容器接口</returns>
        public static IServiceCollection TryAddRepository(this IServiceCollection services)
        {
            services.TryAddSingleton<IRepositoryFactory, RepositoryFactory>();
            return services;
        }
    }
}