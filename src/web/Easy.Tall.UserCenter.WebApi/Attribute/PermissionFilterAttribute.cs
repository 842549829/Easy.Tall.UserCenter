using System;
using System.Linq;
using System.Net;
using Easy.Tall.UserCenter.Entity.Enum;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Framework.Attribute;
using Easy.Tall.UserCenter.Framework.Constant;
using Easy.Tall.UserCenter.Framework.Data;
using Easy.Tall.UserCenter.Framework.Exceptions;
using Easy.Tall.UserCenter.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Easy.Tall.UserCenter.WebApi.Attribute
{
    /// <summary>
    /// 权限过滤
    /// </summary>
    public class PermissionFilterAttribute : System.Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Called early in the filter pipeline to confirm request is authorized.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext" />.</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                if (!ValidatePermission(context))
                {
                    context.Result = GetUnauthorized(HttpStatusCode.Forbidden, "无权限访问");
                }
            }
            catch (BusinessException exception)
            {
                context.Result = GetUnauthorized(HttpStatusCode.Conflict, exception.Message);
            }
            catch (Exception exception)
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<PermissionFilterAttribute>>();
                logger.LogError(exception, "权限错误");
                context.Result = GetUnauthorized(HttpStatusCode.InternalServerError, "系统错误");
            }
        }

        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="context">context</param>
        /// <returns>结果</returns>
        private static bool ValidatePermission(ActionContext context)
        {
            //Controller上是否有PermissionAttribute特性标记如果有则验证权限
            if (((ControllerActionDescriptor)context.ActionDescriptor).ControllerTypeInfo.GetCustomAttributes(typeof(PermissionAttribute), false).FirstOrDefault() is PermissionAttribute controller)
            {
                if (!ValidatePermission(context, controller.Path))
                {
                    return false;
                }
            }

            // Action上是否有PermissionAttribute特性标记如果有则验证权限
            if ((((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetCustomAttributes(typeof(PermissionAttribute), false).FirstOrDefault() is PermissionAttribute action))
            {
                if (!ValidatePermission(context, action.Path))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="context">当前上下文</param>
        /// <param name="path">权限路径</param>
        private static bool ValidatePermission(ActionContext context, string path)
        {
            var userId = context.HttpContext.User.Claims.FirstOrDefault(d => string.Equals(d.Type, AppSettingsSection.Uid, StringComparison.CurrentCultureIgnoreCase));
            if (userId?.Value == null)
            {
                throw new BusinessException(1, "用户需要登陆或Claim不完整");
            }
            var permissionCacheService = context.HttpContext.RequestServices.GetRequiredService<IPermissionCacheService>();
            if (permissionCacheService.IsPermission(userId.Value, path))
            {
                return true;
            }

            //刷新权限
            var permissionService = context.HttpContext.RequestServices.GetRequiredService<IPermissionService>();
            var permissionPaths = permissionService.GetPermissionPaths(new PermissionPathFilter { UserId = userId.Value, PermissionClassify = PermissionClassify.UserCenter });
            if (permissionPaths == null || !permissionPaths.Any())
            {
                throw new BusinessException(1, $"当前用户无访问权限,请管理员先配置权限");
            }
            return permissionCacheService.IsPermission(userId.Value, path);
        }

        /// <summary>
        /// 处理未授权的
        /// </summary>
        /// <param name="code">http状态码</param>
        /// <param name="message">消息</param>
        /// <returns>jsonResult</returns>
        private static JsonResult GetUnauthorized(HttpStatusCode code, string message)
        {
            return new JsonResult(new Result
            {
                Code = (int)code,
                Msg = message
            })
            {
                StatusCode = (int)code
            };
        }
    }
}