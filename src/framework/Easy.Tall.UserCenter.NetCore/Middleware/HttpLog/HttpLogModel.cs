using Microsoft.AspNetCore.Http;

namespace Easy.Tall.UserCenter.NetCore.Middleware.HttpLog
{
    /// <summary>
    /// Log日志模型
    /// </summary>
    internal class HttpLogModel
    {
        /// <summary>
        /// 请求Body
        /// </summary>
        public string RequestBody { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// query参数
        /// </summary>
        public IQueryCollection Query { get; set; }

        /// <summary>
        /// 响应参数
        /// </summary>
        public string ResponseBody { get; set; }
    }
}