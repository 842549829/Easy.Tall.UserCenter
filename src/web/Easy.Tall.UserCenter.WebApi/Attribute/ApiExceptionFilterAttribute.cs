using System;
using System.Security.Authentication;
using Easy.Tall.UserCenter.Framework.Data;
using Easy.Tall.UserCenter.Framework.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Easy.Tall.UserCenter.WebApi.Attribute
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ApiExceptionFilterAttribute> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志</param>
        public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 重写异常
        /// </summary>
        /// <param name="context">异常内容</param>
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            _logger.LogError(exception, "处理API调用发生了异常");

            var apiResult = new Result<string>
            {
                Msg = exception.Message,
                Code = exception.HResult
            };
            var objResult = new ObjectResult(apiResult);
            context.Result = objResult;

            switch (exception)
            {
                case ArgumentException _:
                case InvalidOperationException _:
                    objResult.StatusCode = 400;
                    break;
                case NotFoundException _:
                    objResult.StatusCode = 404;
                    break;
                case AuthenticationException _:
                    objResult.StatusCode = 403;
                    break;
                case TimeoutException _:
                    objResult.StatusCode = 408;
                    break;
                case BusinessException _:
                    objResult.StatusCode = 409;
                    break;
                case NotImplementedException _:
                    objResult.StatusCode = 501;
                    break;
                //case ServiceException _:
                //    objResult.StatusCode = 500;
                //    break;
                default:
                    objResult.StatusCode = 500;
                    break;
            }
        }
    }
}