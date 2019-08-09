using System.Threading.Tasks;
using Easy.Tall.UserCenter.Framework.Constant;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Easy.Tall.UserCenter.WebApi.Middleware
{
    /// <summary>
    /// 登陆检查中间件
    /// </summary>
    public class LoginCheckMiddleware
    {
        /// <summary>
        /// http请求处理管道
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// 缓存
        /// </summary>
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next">http请求处理管道</param>
        /// <param name="cache">缓存</param>
        public LoginCheckMiddleware(RequestDelegate next,
            IDistributedCache cache)
        {
            _next = next;
            _cache = cache;
        }

        /// <summary>
        /// 异步检测
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            //未登陆不进行检查
            if (context.User.Identity.IsAuthenticated)
            {
                var user = context.User;
                var userId = user.FindFirst(AppSettingsSection.Uid).Value;
                var cacheKey = $"user:token:{userId}";
                var loginCache = await _cache.GetStringAsync(cacheKey);
                if (!string.IsNullOrWhiteSpace(loginCache))
                {
                    // 刷新token
                    await _cache.RefreshAsync(cacheKey);
                }
                else
                {
                    await SetResult(context, "用户未登陆或登陆已过期");
                    return;
                }
            }
            await _next(context);
        }

        /// <summary>
        /// 设置结果
        /// </summary>
        /// <param name="context">http上下文</param>
        /// <param name="message">消息</param>
        /// <returns>结果</returns>
        private static Task SetResult(HttpContext context, string message)
        {
            context.Response.StatusCode = 401;
            var response = new
            {
                msg = message,
                data = default(object),
                code = 1
            };
            context.Response.ContentType = "application/json";
            var json = JsonConvert.SerializeObject(response);
            return context.Response.WriteAsync(json);
        }
    }
}