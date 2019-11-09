using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Easy.Tall.UserCenter.Framework.NetCore.Middleware.HttpLog
{
    /// <summary>
    /// HttpLog中间件
    /// </summary>
    public class HttpLogMiddleware
    {
        /// <summary>
        /// 下一个请求委托
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<HttpLogMiddleware> _logger;

        /// <summary>
        /// 配置
        /// </summary>
        private readonly HttpLogOption _httpLogOption;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next">下一个请求委托</param>
        /// <param name="logger">日志</param>
        /// <param name="httpLogOption">配置</param>
        public HttpLogMiddleware(RequestDelegate next,
            ILogger<HttpLogMiddleware> logger,
            IOptions<HttpLogOption> httpLogOption)
        {
            _next = next;
            _logger = logger;
            _httpLogOption = httpLogOption.Value;
        }

        /// <summary>
        /// 异步日志记录
        /// </summary>
        /// <param name="context">当前上下文</param>
        /// <returns>表示一个异步</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            if (MatchRequestOption(context))
            {
                await WriteHttpLog(context);
            }
            else
            {
                await _next(context);
            }
        }

        /// <summary>
        /// 请求是否满足配置要求
        /// </summary>
        /// <param name="context">当前上下文</param>
        /// <returns>结果</returns>
        private bool MatchRequestOption(HttpContext context)
        {
            var method = context.Request.Method.ToUpper();
            switch (method)
            {
                case "GET" when MatchPath(context, _httpLogOption.HttpGet):
                case "POST" when MatchPath(context, _httpLogOption.HttpPost):
                case "PUT" when MatchPath(context, _httpLogOption.HttpPut):
                case "DELETE" when MatchPath(context, _httpLogOption.HttpDelete):
                case "HEAD" when MatchPath(context, _httpLogOption.HttpHead):
                case "OPTIONS" when MatchPath(context, _httpLogOption.HttpOptions):
                case "TRACE" when MatchPath(context, _httpLogOption.HttpTrace):
                case "CONNECT" when MatchPath(context, _httpLogOption.HttpConnect):
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 响应是否满足配置要求
        /// </summary>
        /// <param name="context">当前上下文</param>
        /// <returns>结果</returns>
        private bool MatchResponseOption(HttpContext context)
        {
            var method = context.Request.Method.ToUpper();
            switch (method)
            {
                case "GET" when MatchResponse(context, _httpLogOption.HttpGet):
                case "POST" when MatchResponse(context, _httpLogOption.HttpPost):
                case "PUT" when MatchResponse(context, _httpLogOption.HttpPut):
                case "DELETE" when MatchResponse(context, _httpLogOption.HttpDelete):
                case "HEAD" when MatchResponse(context, _httpLogOption.HttpHead):
                case "OPTIONS" when MatchResponse(context, _httpLogOption.HttpOptions):
                case "TRACE" when MatchResponse(context, _httpLogOption.HttpTrace):
                case "CONNECT" when MatchResponse(context, _httpLogOption.HttpConnect):
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 路径是否匹配
        /// </summary>
        /// <param name="context">当前上下文</param>
        /// <param name="path">路径</param>
        /// <returns>结果</returns>
        private static bool MatchPath(HttpContext context, Path path)
        {
            if (path == null || !path.Enabled)
            {
                return false;
            }
            if (!MatchRequestContentType(context, path))
            {
                return false;
            }
            var pathValue = context.Request.Path.Value.ToLower().Trim();
            var includePath = MatchIncludePath(path, pathValue);
            var existExclusive = MatchExclusivePath(path, pathValue);
            return includePath && !existExclusive;
        }

        /// <summary>
        /// 匹配请求类型
        /// </summary>
        /// <param name="context">当前上下文</param>
        /// <param name="path">路径</param>
        /// <returns>结果</returns>
        private static bool MatchRequestContentType(HttpContext context, Path path)
        {
            if (path.RequestContentType == null)
            {
                return true;
            }
            if (path.RequestContentType.Any(d => d == "*"))
            {
                return true;
            }
            return context.Request.ContentType != null && path.RequestContentType.Any(d => context.Request.ContentType.ToLower().Contains(d.ToLower()));
        }

        /// <summary>
        /// 匹配响应类型
        /// </summary>
        /// <param name="context">当前上下文</param>
        /// <param name="path">路径</param>
        /// <returns>结果</returns>
        private static bool MatchResponse(HttpContext context, Path path)
        {
            switch (path.ResponseContentType)
            {
                case null when context.Response.ContentType != null && context.Response.ContentType.ToLower().Contains("json"):
                    return true;
                case null:
                    return false;
            }
            if (path.ResponseContentType.Any(d => d == "*"))
            {
                return true;
            }
            return context.Response.ContentType != null && path.ResponseContentType.Any(d => context.Response.ContentType.ToLower().Contains(d.ToLower()));
        }

        /// <summary>
        /// 匹配白名单路径
        /// </summary>
        /// <param name="path">配置径值</param>
        /// <param name="pathValue">具体路径</param>
        /// <returns>是否允许</returns>
        private static bool MatchIncludePath(Path path, string pathValue)
        {
            return path.IncludePath == null
                   || path.IncludePath.Any(d => d == "*")
                   || path.IncludePath.Any(d => MatchPathRule(d.Trim(), pathValue));
        }

        /// <summary>
        /// 匹配黑名单路径
        /// </summary>
        /// <param name="path">配置径值</param>
        /// <param name="pathValue">具体路径</param>
        /// <returns>是否不允许</returns>
        private static bool MatchExclusivePath(Path path, string pathValue)
        {
            return path.ExcludePath != null
                   && path.ExcludePath.Any(d => MatchPathRule(d.Trim(), pathValue));
        }

        /// <summary>
        /// 匹配路径规则
        /// </summary>
        /// <param name="path">配置径值</param>
        /// <param name="pathValue">具体路径</param>
        /// <returns>结果</returns>
        private static bool MatchPathRule(string path, string pathValue)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            //如果第一个字符为$则通过正则匹配
            if (path[0] == '$')
            {
                var pattern = path.TrimStart('$');
                return Regex.IsMatch(pathValue, pattern, RegexOptions.IgnoreCase);
            }

            //查看最后一个字符是否是/结尾 如果是则匹配路径为扩展匹配
            if (path[path.Length - 1] == '/')
            {
                if (pathValue.Length >= path.Length)
                {
                    return string.Equals(path, pathValue.Substring(0, path.Length), StringComparison.OrdinalIgnoreCase);
                }
            }
            return string.Equals(path, pathValue, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="context">当前上下文</param>
        /// <returns>表示一个异步</returns>
        private async Task WriteHttpLog(HttpContext context)
        {
            HttpLogModel httpLogModel = null;
            var request = context.Request;
            var response = context.Response;

            // RequestBody
            request.EnableBuffering();
            using (var requestReader = new StreamReader(request.Body))
            {
                var requestBody = requestReader.ReadToEnd();
                request.Body.Position = 0;

                // ResponseBody
                var responseOriginalBody = response.Body;
                try
                {
                    using (var memStream = new MemoryStream())
                    {
                        response.Body = memStream;
                        await _next(context);
                        if (MatchResponseOption(context))
                        {
                            //日志Model
                            memStream.Position = 0;
                            var responseBody = new StreamReader(memStream);
                            httpLogModel = new HttpLogModel
                            {
                                Path = request.Path,
                                Method = request.Method,
                                Query = request.Query,
                                StatusCode = response.StatusCode,
                                RequestBody = requestBody,
                                ResponseBody = responseBody.ReadToEnd()
                            };
                        }
                        memStream.Position = 0;
                        await memStream.CopyToAsync(responseOriginalBody);
                    }
                }
                finally
                {
                    response.Body = responseOriginalBody;
                }
            }

            if (httpLogModel != null)
            {
                _logger.LogInformation("Http日志:{@logMode}", httpLogModel);
            }
        }
    }
}