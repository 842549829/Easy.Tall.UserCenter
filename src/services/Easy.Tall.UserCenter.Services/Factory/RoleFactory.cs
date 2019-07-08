using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.Services.Factory
{
    /// <summary>
    /// 角色工厂
    /// </summary>
    public static class RoleFactory
    {
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="roleAddRequest">添加角色信息</param>
        /// <returns>角色</returns>
        public static Role ToRole(this RoleAddRequest roleAddRequest)
        {
            return new Role
            {
                Name = roleAddRequest.Name,
                ClassifyId = roleAddRequest.ClassifyId,
                Describe = roleAddRequest.Describe
            };
        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="roleUpdateRequest">修改角色信息</param>
        /// <returns>角色</returns>
        public static Role ToRole(this RoleUpdateRequest roleUpdateRequest)
        {
            return new Role
            {
                Id = roleUpdateRequest.Id,
                Name = roleUpdateRequest.Name,
                ClassifyId = roleUpdateRequest.ClassifyId,
                Describe = roleUpdateRequest.Describe
            };
        }
    }
}