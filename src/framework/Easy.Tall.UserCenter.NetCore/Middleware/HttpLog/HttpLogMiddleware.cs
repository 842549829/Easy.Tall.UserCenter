using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Easy.Tall.UserCenter.NetCore.Middleware.HttpLog
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
            var contentType = context.Request.ContentType?.ToLower();
            var method = context.Request.Method.ToUpper();
            if (IsWriteHttpLog(contentType, method))
            {
                await WriteHttpLog(context);
            }
            else
            {
                await _next(context);
            }
        }

        /// <summary>
        /// 是否写入httpLog
        /// </summary>
        /// <param name="contentType">类型</param>
        /// <param name="method">类型</param>
        /// <returns>结果</returns>
        private bool IsWriteHttpLog(string contentType, string method)
        {
            return contentType != null
                   && contentType.Contains("json")
                   && method == "GET" && _httpLogOption.HttpGet
                   || method == "POST" && _httpLogOption.HttpPost
                   || method == "PUT" && _httpLogOption.HttpPut
                   || method == "DELETE" && _httpLogOption.HttpDelete
                   || method == "HEAD" && _httpLogOption.HttpHead
                   || method == "OPTIONS" && _httpLogOption.HttpOptions
                   || method == "TRACE" && _httpLogOption.HttpTrace
                   || method == "CONNECT" && _httpLogOption.HttpConnect;
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="context">当前上下文</param>
        /// <returns>表示一个异步</returns>
        private async Task WriteHttpLog(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;

            //日志Model
            var logMode = new HttpLogModel
            {
                Path = request.Path,
                Method = request.Method,
                Query = request.Query
            };

            // RequestBody
            request.EnableBuffering();
            using (var requestReader = new StreamReader(request.Body))
            {
                logMode.RequestBody = requestReader.ReadToEnd();
                request.Body.Position = 0;

                // ResponseBody
                var responseOriginalBody = response.Body;
                try
                {
                    using (var memStream = new MemoryStream())
                    {
                        response.Body = memStream;
                        await _next(context);
                        memStream.Position = 0;
                        var responseBody = new StreamReader(memStream);
                        logMode.ResponseBody = responseBody.ReadToEnd();
                        memStream.Position = 0;
                        await memStream.CopyToAsync(responseOriginalBody);
                    }
                }
                finally
                {
                    response.Body = responseOriginalBody;
                }
            }
            _logger.LogInformation("Http日志:{@logMode}", logMode);
        }
    }
}