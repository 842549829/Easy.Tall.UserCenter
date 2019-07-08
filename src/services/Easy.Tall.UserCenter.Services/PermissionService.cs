using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Data;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.IRepository;
using Easy.Tall.UserCenter.IServices;
using Easy.Tall.UserCenter.Services.Factory;
using Microsoft.Extensions.Logging;

namespace Easy.Tall.UserCenter.Services
{
    /// <summary>
    /// 权限
    /// </summary>
    public class PermissionService : UnitOfWorkBase, IPermissionService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbUnitOfWorkFactory">工作单元</param>
        /// <param name="dbConnectionFactory">数据库链接</param>
        /// <param name="repositoryFactory">仓储工厂</param>
        /// <param name="logger">日志</param>
        public PermissionService(IDbUnitOfWorkFactory dbUnitOfWorkFactory, IDbConnectionFactory dbConnectionFactory, IRepositoryFactory repositoryFactory, ILogger logger) : base(dbUnitOfWorkFactory, dbConnectionFactory, repositoryFactory, logger)
        {
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="permissionAddRequest">权限信息</param>
        /// <returns>结果</returns>
        public Result<bool> Add(PermissionAddRequest permissionAddRequest)
        {
            return Execute(permissionAddRequest, (connection, factory, data) =>
            {
                var repository = factory.CreateRepository(connection);
                var permission = repository.CreatePermissionRepository(connection);
                permission.Add(data.ToPermission());
            });
        }

        /// <summary>
        /// 根据主键Id删除权限
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>结果</returns>
        public Result<bool> Remove(string id)
        {
            return Execute(id, (connection, factory, data) =>
            {
                var repository = factory.CreateRepository(connection);
                var permission = repository.CreatePermissionRepository(connection);
                permission.Remove(new Permission { Id = data });
            });
        }

        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="permissionUpdateRequest">权限信息</param>
        /// <returns>结果</returns>
        public Result<bool> Update(PermissionUpdateRequest permissionUpdateRequest)
        {
            return Execute(permissionUpdateRequest, (connection, factory, data) =>
            {
                var repository = factory.CreateRepository(connection);
                var permission = repository.CreatePermissionRepository(connection);
                permission.Update(data.ToPermission());
            });
        }

        /// <summary>
        /// 权限分页
        /// </summary>
        /// <param name="permissionFilter">查询条件</param>
        /// <returns>数据</returns>
        public Pagination<PermissionPaginationResponse> GetPagination(PermissionFilter permissionFilter)
        {
            return Query(permissionFilter, (connection, factory, data) =>
            {
                var repository = factory.CreateRepository(connection);
                var permission = repository.CreatePermissionRepository(connection);
                return permission.GetPagination(data);
            });
        }
    }
}