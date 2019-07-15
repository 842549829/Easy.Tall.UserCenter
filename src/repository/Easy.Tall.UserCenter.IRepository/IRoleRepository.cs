using System.Collections.Generic;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Data;
using Easy.Tall.UserCenter.Framework.Db;

namespace Easy.Tall.UserCenter.IRepository
{
    /// <summary>
    /// 角色仓储接口
    /// </summary>
    public interface IRoleRepository : IRepository<string, Role>
    {
        /// <summary>
        /// 角色分页查询
        /// </summary>
        /// <param name="roleFilter">角色查询条件</param>
        /// <returns>查询数据</returns>
        Pagination<RolePaginationResponse> GetPagination(RoleFilter roleFilter);

        /// <summary>
        /// 获取角色分组
        /// </summary>
        /// <returns>角色分组</returns>
        IEnumerable<RoleGroupByResponse> GetRoleGroupByResponses();

        /// <summary>
        /// 是否使用该分类
        /// </summary>
        /// <param name="classifyId">分类Id</param>
        /// <returns>结果</returns>
        bool ContainsClassifyType(string classifyId);
    }
}