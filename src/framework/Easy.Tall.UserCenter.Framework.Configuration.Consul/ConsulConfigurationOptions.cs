namespace Easy.Tall.UserCenter.Framework.Configuration.Consul
{
    /// <summary>
    /// consul在线配置
    /// </summary>
    public class ConsulConfigurationOptions
    {
        /// <summary>
        /// 链接consul地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 根目录
        /// </summary>
        public string RootFolder { get; set; }

        /// <summary>
        /// 子目录
        /// </summary>
        public string[] Folders { get; set; }

        /// <summary>
        /// 心跳频率(单位秒)
        /// </summary>
        public int BlockingQueryWaitSeconds { get; set; } = 30;

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 数据中心
        /// </summary>
        public string Datacenter { get; set; }
    }
}