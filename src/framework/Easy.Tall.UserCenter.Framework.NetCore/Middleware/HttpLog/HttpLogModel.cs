﻿using Microsoft.AspNetCore.Http;

namespace Easy.Tall.UserCenter.Framework.NetCore.Middleware.HttpLog
{
    /// <summary>
    /// Log日志模型
    /// </summary>
    internal class HttpLogModel
    {
        /// <summary>
        /// 请求方式
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 请求Body
        /// </summary>
        public string RequestBody { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// query参数
        /// </summary>
        public IQueryCollection Query { get; set; }

        /// <summary>
        /// Http状态码
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// 响应参数
        /// </summary>
        public string ResponseBody { get; set; }
    }
}