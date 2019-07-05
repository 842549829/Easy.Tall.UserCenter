using System;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Framework.Data;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.IRepository;
using Easy.Tall.UserCenter.IServices;
using Microsoft.Extensions.Logging;

namespace Easy.Tall.UserCenter.Services
{
    /// <summary>
    /// 角色服务
    /// </summary>
    public class RoleService : UnitOfWorkBase, IRoleService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbUnitOfWorkFactory">工作单元</param>
        /// <param name="dbConnectionFactory">数据库链接</param>
        /// <param name="repositoryFactory">仓储工厂</param>
        /// <param name="logger">日志</param>
        public RoleService(IDbUnitOfWorkFactory dbUnitOfWorkFactory,
            IDbConnectionFactory dbConnectionFactory,
            IRepositoryFactory repositoryFactory,
            ILogger logger)
            : base(dbUnitOfWorkFactory, dbConnectionFactory, repositoryFactory, logger)
        {
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleAddRequest">添加角色信息</param>
        /// <returns>结果</returns>
        public Result<bool> Add(RoleAddRequest roleAddRequest)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据主键Id删除角色
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>结果</returns>
        public Result<bool> Remove(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="roleUpdateRequest">角色信息</param>
        /// <returns>结果</returns>
        public Result<bool> Update(RoleUpdateRequest roleUpdateRequest)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 角色分页
        /// </summary>
        /// <param name="classifyFilter">查询条件</param>
        /// <returns>数据</returns>
        public Pagination<RolePaginationResponse> GetPagination(RoleFilter classifyFilter)
        {
            throw new NotImplementedException();
        }
    }
}