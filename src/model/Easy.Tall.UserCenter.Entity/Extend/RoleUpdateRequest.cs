namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 修改角色
    /// </summary>
    public class RoleUpdateRequest : RoleResponse
    {
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