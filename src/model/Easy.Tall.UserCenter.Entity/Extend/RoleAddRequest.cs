namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 添加角色
    /// </summary>
    public class RoleAddRequest
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        public string ClassifyId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
    }
}