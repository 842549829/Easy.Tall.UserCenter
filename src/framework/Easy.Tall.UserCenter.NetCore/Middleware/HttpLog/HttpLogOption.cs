namespace Easy.Tall.UserCenter.NetCore.Middleware.HttpLog
{
    /// <summary>
    /// Http请求日志配置
    /// </summary>
    public class HttpLogOption
    {
        /// <summary>
        /// 默认路径
        /// </summary>
        private static Path DefaultPath => new Path { Enabled = true };

        /// <summary>
        /// 向指定资源提交数据进行处理请求（例如提交表单或者上传文件）。数据被包含在请求体中。POST请求可能会导致新的资源的建立和/或已有资源的修改。 Loadrunner中对应POST请求函数：web_submit_data,web_submit_form
        /// </summary>
        public Path HttpPost { get; set; } = DefaultPath;

        /// <summary>
        /// 请求服务器删除Request-URL所标识的资源
        /// </summary>
        public Path HttpDelete { get; set; } = DefaultPath;

        /// <summary>
        /// 向指定资源位置上传其最新内容
        /// </summary>
        public Path HttpPut { get; set; } = DefaultPath;

        /// <summary>
        /// 向特定的资源发出请求。注意：GET方法不应当被用于产生“副作用”的操作中，例如在Web Application中，其中一个原因是GET可能会被网络蜘蛛等随意访问。Loadrunner中对应get请求函数：web_link和web_url
        /// </summary>
        public Path HttpGet { get; set; }

        /// <summary>
        /// 向服务器索与GET请求相一致的响应，只不过响应体将不会被返回。这一方法可以再不必传输整个响应内容的情况下，就可以获取包含在响应小消息头中的元信息。
        /// </summary>
        public Path HttpHead { get; set; }

        /// <summary>
        /// 返回服务器针对特定资源所支持的HTTP请求方法，也可以利用向web服务器发送‘*’的请求来测试服务器的功能性
        /// </summary>
        public Path HttpOptions { get; set; }

        /// <summary>
        /// 回显服务器收到的请求，主要用于测试或诊断
        /// </summary>
        public Path HttpTrace { get; set; }

        /// <summary>
        /// HTTP/1.1协议中预留给能够将连接改为管道方式的代理服务器。
        /// </summary>
        public Path HttpConnect { get; set; }
    }

    /// <summary>
    /// 路径
    /// </summary>
    public class Path
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 包含可用路径 null 或者 *表示包含所有路径
        /// </summary>
        public string[] IncludePath { get; set; }

        /// <summary>
        /// 排除的可用路径不支持*
        /// </summary>
        public string[] ExcludePath { get; set; }
        
        /// <summary>
        /// 请求类型
        /// </summary>
        public string[] RequestContentType { get; set; }

        /// <summary>
        /// 响应类型
        /// </summary>
        public string[] ResponseContentType { get; set; }
    }
}