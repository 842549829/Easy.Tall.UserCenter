using System;
using System.Threading.Tasks;
using Easy.Tall.UserCenter.Framework.Constant;
using Easy.Tall.UserCenter.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
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
        private readonly IPermissionCacheService _permissionCacheService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next">http请求处理管道</param>
        /// <param name="permissionCacheService">缓存</param>
        public LoginCheckMiddleware(RequestDelegate next,
            IPermissionCacheService permissionCacheService)
        {
            _next = next;
            _permissionCacheService = permissionCacheService;
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
                var loginCache = await _permissionCacheService.GetUserTokenAsync(userId);
                var authenticateResult = await context.AuthenticateAsync();
                var token = authenticateResult.Properties.Items[".Token.access_token"];
                if (string.Equals(loginCache, token, StringComparison.CurrentCultureIgnoreCase))
                {
                    // 刷新token
                    await _permissionCacheService.RefreshUserTokenAsync(userId);
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