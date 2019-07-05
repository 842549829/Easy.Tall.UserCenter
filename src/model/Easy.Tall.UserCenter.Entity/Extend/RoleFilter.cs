namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 角色查询条件
    /// </summary>
    public class RoleFilter
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