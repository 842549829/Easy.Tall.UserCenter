﻿using System.Collections.Generic;
using Easy.Tall.UserCenter.Entity.Enum;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Data;

namespace Easy.Tall.UserCenter.IServices
{
    /// <summary>
    /// 权限
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
        /// 查询权限
        /// </summary>
        /// <param name="permissionClassify">所属分类</param>
        /// <returns>权限</returns>
        IEnumerable<Permission> GetFunctions(PermissionClassify permissionClassify);
    }
}