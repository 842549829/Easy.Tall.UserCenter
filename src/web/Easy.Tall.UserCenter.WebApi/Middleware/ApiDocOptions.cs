using System;

namespace Easy.Tall.UserCenter.WebApi.Middleware
{
    /// <summary>
    /// 文档配置
    /// </summary>
    public class ApiDocOptions
    {
        /// <summary>
        /// 文档服务POST地址
        /// </summary>
        public Uri ApiDocEndPoint { get; set; }

        /// <summary>
        /// 文档接口地址
        /// </summary>
        public Uri ApiHost { get; set; }

        /// <summary>
        /// 文档名称
        /// </summary>
        public string ApiName { get; set; }

        /// <summary>
        /// 更新文档路由
        /// </summary>
        public string ApiDocUpdatePath { get; set; } = "/u";

        /// <summary>
        /// apiJsonFile路径
        /// </summary>
        public string ApiJsonFilePath { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
    }
}