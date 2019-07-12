using System.Collections.Generic;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 权限编辑
    /// </summary>
    public class PermissionEditRequest
    {
        /// <summary>
        /// 角色
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 权限Id列表
        /// </summary>
        public IEnumerable<string> PermissionIdList { get; set; }
    }
}