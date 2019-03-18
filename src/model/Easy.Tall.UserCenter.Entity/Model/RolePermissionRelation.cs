namespace Easy.Tall.UserCenter.Entity.Model
{
    /// <summary>
    ///角色权限权限关系
    /// </summary>
    public class RolePermissionRelation
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
        public string PermissionId { get; set; }
    }
}