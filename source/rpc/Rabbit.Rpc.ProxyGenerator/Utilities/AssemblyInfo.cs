namespace Rabbit.Rpc.ProxyGenerator.Utilities
{
    /// <summary>
    /// 程序集信息
    /// </summary>
    public class AssemblyInfo
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Product
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// 版权
        /// </summary>
        public string Copyright { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 文件版本
        /// </summary>
        public string FileVersion { get; set; }

        /// <summary>
        /// ComVisible
        /// </summary>
        public bool ComVisible { get; set; }

        /// <summary>
        /// 创建一个程序集信息
        /// </summary>
        /// <param name="name">标题名称</param>
        /// <param name="copyright">版权</param>
        /// <param name="version">版本号</param>
        /// <returns>结果</returns>
        public static AssemblyInfo Create(string name, string copyright = "Copyright ©  Rabbit", string version = "1.0.0.0")
        {
            return new AssemblyInfo
            {
                Title = name,
                Product = name,
                Copyright = copyright,
                Guid = System.Guid.NewGuid().ToString("D"),
                ComVisible = false,
                Version = version,
                FileVersion = version
            };
        }
    }
}