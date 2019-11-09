namespace Easy.Tall.UserCenter.Framework.Configuration.Consul
{
    /// <summary>
    /// consul筛选范围配置
    /// </summary>
    public class ConsulQueryOptions
    {
        /// <summary>
        /// 跟目录
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// 子目录集合
        /// </summary>
        public string[] Folders { get; internal set; }
    }
}