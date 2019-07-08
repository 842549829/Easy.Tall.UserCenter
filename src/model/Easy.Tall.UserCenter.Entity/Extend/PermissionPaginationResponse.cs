namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 权限分页查询结果
    /// </summary>
    public class PermissionPaginationResponse
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分组Id
        /// </summary>
        public string ClassifyId { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        public string Classify { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
    }
}