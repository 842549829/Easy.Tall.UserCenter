namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 权限分页查询条件
    /// </summary>
    public class PermissionFilter
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属分组
        /// </summary>
        public string ClassifyId { get; set; }
    }
}