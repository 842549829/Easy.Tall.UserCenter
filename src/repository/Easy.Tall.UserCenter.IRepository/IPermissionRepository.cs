using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Data;
using Easy.Tall.UserCenter.Framework.Db;

namespace Easy.Tall.UserCenter.IRepository
{
    /// <summary>
    /// 权限仓储接口
    /// </summary>
    public interface IPermissionRepository : IRepository<string, Permission>
    {
        /// <summary>
        /// 权限分页查询
        /// </summary>
        /// <param name="permissionFilter">权限查询条件</param>
        /// <returns>查询数据</returns>
        Pagination<PermissionPaginationResponse> GetPagination(PermissionFilter permissionFilter);
    }
}