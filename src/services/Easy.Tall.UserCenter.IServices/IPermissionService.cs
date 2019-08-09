using System.Collections.Generic;
using Easy.Tall.UserCenter.Entity.Enum;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Data;

namespace Easy.Tall.UserCenter.IServices
{
    /// <summary>
    /// 功能
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="permissionAddRequest">权限信息</param>
        /// <returns>结果</returns>
        Result<bool> Add(PermissionAddRequest permissionAddRequest);

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="permissionRemoveRequest">删除信息</param>
        /// <returns>结果</returns>
        Result<bool> Remove(PermissionRemoveRequest permissionRemoveRequest);

        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="permissionUpdateRequest">权限信息</param>
        /// <returns>结果</returns>
        Result<bool> Update(PermissionUpdateRequest permissionUpdateRequest);

        /// <summary>
        /// 编辑权限
        /// </summary>
        /// <param name="permissionEditRequest">权限信息</param>
        /// <returns>结果</returns>
        Result<bool> Edit(PermissionEditRequest permissionEditRequest);

        /// <summary>
        /// 查询权限
        /// </summary>
        /// <param name="permissionClassify">所属分类</param>
        /// <returns>权限</returns>
        IEnumerable<Permission> GetPermissions(PermissionClassify permissionClassify);

        /// <summary>
        /// 查询权限
        /// </summary>
        /// <param name="permissionsFilter">查询条件</param>
        /// <returns>权限</returns>
        IEnumerable<PermissionResponse> GetPermissionsByRoleId(PermissionFilter permissionsFilter);

        /// <summary>
        /// 查询权限
        /// </summary>
        /// <param name="permissionFilter">查询条件</param>
        /// <returns>权限</returns>
        IEnumerable<Permission> GetPermissionsByUserId(PermissionFilter permissionFilter);

        /// <summary>
        /// 查询权限路径
        /// </summary>
        /// <param name="permissionFilter">查询条件</param>
        /// <returns>权限</returns>
        IEnumerable<string> GetPermissionPathByUserId(PermissionFilter permissionFilter);
    }
}