using Easy.Tall.UserCenter.Entity.Extend;
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
        /// 根据主键Id删除权限
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>结果</returns>
        Result<bool> Remove(string id);

        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="permissionUpdateRequest">权限信息</param>
        /// <returns>结果</returns>
        Result<bool> Update(PermissionUpdateRequest permissionUpdateRequest);

        /// <summary>
        /// 权限分页
        /// </summary>
        /// <param name="permissionFilter">查询条件</param>
        /// <returns>数据</returns>
        Pagination<PermissionPaginationResponse> GetPagination(PermissionFilter permissionFilter);
    }
}