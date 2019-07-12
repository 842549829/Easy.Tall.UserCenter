namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 角色分页信息
    /// </summary>
    public class RolePaginationResponse : RoleResponse
    {
        /// <summary>
        /// 所属分组Id
        /// </summary>
        public string ClassifyId { get; set; }

        /// <summary>
        /// 所属分组
        /// </summary>
        public string Classify { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
    }
}