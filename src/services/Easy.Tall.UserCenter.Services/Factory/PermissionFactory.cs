using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.Services.Factory
{
    /// <summary>
    /// 权限工厂
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
                Describe = permissionAddRequest.Describe,
                ClassifyId = permissionAddRequest.ClassifyId
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
                Name = permissionUpdateRequest.Name,
                Id = permissionUpdateRequest.Id,
                ClassifyId = permissionUpdateRequest.ClassifyId,
                Describe = permissionUpdateRequest.Describe
            };
        }
    }
}