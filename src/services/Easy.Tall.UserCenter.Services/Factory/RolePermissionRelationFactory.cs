using System.Collections.Generic;
using System.Linq;
using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.Services.Factory
{
    /// <summary>
    /// 角色权限工厂
    /// </summary>
    public static class RolePermissionRelationFactory
    {
        /// <summary>
        /// 创建角色权限
        /// </summary>
        /// <param name="permissionIdList">权限Id列表</param>
        /// <param name="roleId">角色Id</param>
        /// <returns>角色权限</returns>
        public static IEnumerable<RolePermissionRelation> ToRolePermissionRelation(this IEnumerable<string> permissionIdList, string roleId)
        {
            return permissionIdList.Select(item => new RolePermissionRelation { PermissionId = item, RoleId = roleId });
        }

        /// <summary>
        /// 创建权限Id
        /// </summary>
        /// <param name="rolePermissionRelations">角色权限</param>
        /// <returns>结果</returns>
        public static IEnumerable<string> ToPermissionIdList(this IEnumerable<RolePermissionRelation> rolePermissionRelations)
        {
            return rolePermissionRelations.Select(rolePermissionRelation => rolePermissionRelation.PermissionId);
        }
    }
}