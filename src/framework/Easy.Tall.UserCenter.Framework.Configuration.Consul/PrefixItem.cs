namespace Easy.Tall.UserCenter.Framework.Configuration.Consul
{
    /// <summary>
    /// 前缀项
    /// </summary>
    public class PrefixItem
    {
        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// value
        /// </summary>
        public byte[] Value { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }
    }
}