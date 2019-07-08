namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 添加权限实体
    /// </summary>
    public class PermissionAddRequest
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属分组
        /// </summary>
        public string ClassifyId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
    }
}