using Microsoft.AspNetCore.Builder;

namespace Easy.Tall.UserCenter.WebApi.Middleware
{
    /// <summary>
    /// 登录检查扩展
    /// </summary>
    public static class LoginCheckExtension
    {
        /// <summary>
        /// 加入中间件
        /// </summary>
        /// <param name="builder">builder</param>
        /// <returns>IApplicationBuilder</returns>
        public static IApplicationBuilder UseLoginCheck(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoginCheckMiddleware>();
        }
    }
}