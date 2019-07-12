using System;
using System.Collections.Generic;
using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.IRepository
{
    /// <summary>
    /// 角色权限仓储接口
    /// </summary>
    public interface IRolePermissionRelationRepository : IDisposable
    {
        /// <summary>
        /// 添加角色权限
        /// </summary>
        /// <param name="rolePermissionRelations">角色权限</param>
        void AddRange(IEnumerable<RolePermissionRelation> rolePermissionRelations);

        /// <summary>
        /// 删除角色权限
        /// </summary>
        /// <param name="rolePermissionRelations">角色权限</param>
        void RemoveRange(IEnumerable<RolePermissionRelation> rolePermissionRelations);

        /// <summary>
        /// 根据角色Id查询角色权限
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns>结果</returns>
        IEnumerable<RolePermissionRelation> QueryByRoleId(string roleId);
    }
}