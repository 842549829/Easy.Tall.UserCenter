using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.Services.Factory
{
    /// <summary>
    /// 权限
    /// </summary>
    public static class PermissionFactory
    {
        /// <summary>
        /// 创建权限
        /// </summary>
        /// <param name="permissionAddRequest">添加权限信息</param>
        /// <returns>权限</returns>
        public static Permission ToPermission(this PermissionAddRequest permissionAddRequest)
        {
            return new Permission
            {
                Name = permissionAddRequest.Name,
                Type = permissionAddRequest.Type,
                Classify = permissionAddRequest.Classify,
                Describe = permissionAddRequest.Describe,
                Flag = permissionAddRequest.Flag,
                Icon = permissionAddRequest.Icon,
                ParentId = permissionAddRequest.ParentId,
                Path = permissionAddRequest.Path,
                Sort = permissionAddRequest.Sort
            };
        }

        /// <summary>
        /// 创建权限
        /// </summary>
        /// <param name="permissionUpdateRequest">修改权限信息</param>
        /// <returns>权限</returns>
        public static Permission ToPermission(this PermissionUpdateRequest permissionUpdateRequest)
        {
            return new Permission
            {
                Id = permissionUpdateRequest.Id,
                Name = permissionUpdateRequest.Name,
                Describe = permissionUpdateRequest.Describe,
                Flag = permissionUpdateRequest.Flag,
                Icon = permissionUpdateRequest.Icon,
                Path = permissionUpdateRequest.Path,
                Sort = permissionUpdateRequest.Sort
            };
        }
    }
}