using System.Collections.Generic;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Framework.Data;

namespace Easy.Tall.UserCenter.IServices
{
    /// <summary>
    /// 角色
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleAddRequest">添加角色信息</param>
        /// <returns>结果</returns>
        Result<bool> Add(RoleAddRequest roleAddRequest);

        /// <summary>
        /// 根据主键Id删除角色
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>结果</returns>
        Result<bool> Remove(string id);

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="roleUpdateRequest">角色信息</param>
        /// <returns>结果</returns>
        Result<bool> Update(RoleUpdateRequest roleUpdateRequest);

        /// <summary>
        /// 角色分页
        /// </summary>
        /// <param name="classifyFilter">查询条件</param>
        /// <returns>数据</returns>
        Pagination<RolePaginationResponse> GetPagination(RoleFilter classifyFilter);

        /// <summary>
        /// 获取角色分组
        /// </summary>
        /// <returns>角色分组</returns>
        IEnumerable<RoleGroupByResponse> GetRoleGroupByResponses();
    }
}