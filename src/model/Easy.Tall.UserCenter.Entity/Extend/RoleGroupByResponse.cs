using System.Collections.Generic;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 角色分组
    /// </summary>
    public class RoleGroupByResponse : ClassifyResponse
    {
        /// <summary>
        /// 角色
        /// </summary>
        public IEnumerable<RoleResponse> Roles { get; set; }
    }
}