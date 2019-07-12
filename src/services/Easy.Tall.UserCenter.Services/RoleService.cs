using System.Collections.Generic;
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
            ILogger<RoleService> logger)
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
            return Execute(roleAddRequest, (connection, repositoryFactory, data) =>
            {
                var repository = repositoryFactory.CreateRepository(connection);
                var roleRepository = repository.CreateRoleRepository(connection);
                roleRepository.Add(data.ToRole());
            });
        }

        /// <summary>
        /// 根据主键Id删除角色
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>结果</returns>
        public Result<bool> Remove(string id)
        {
            return Execute(id, (connection, factory, data) =>
            {
                var repository = factory.CreateRepository(connection);
                var role = repository.CreateRoleRepository(connection);
                // 验证
                // 角色是否已经绑定相关用户  如果已经绑定且提升是否删除相关用户拥有的该角色权限 

                // 删除角色绑定的相关权限
                role.Remove(new Role { Id = id });
            });
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="roleUpdateRequest">角色信息</param>
        /// <returns>结果</returns>
        public Result<bool> Update(RoleUpdateRequest roleUpdateRequest)
        {
            return Execute(roleUpdateRequest, (connection, factory, data) =>
            {
                var repository = factory.CreateRepository(connection);
                var role = repository.CreateRoleRepository(connection);
                role.Update(roleUpdateRequest.ToRole());
            });
        }

        /// <summary>
        /// 角色分页
        /// </summary>
        /// <param name="roleFilter">查询条件</param>
        /// <returns>数据</returns>
        public Pagination<RolePaginationResponse> GetPagination(RoleFilter roleFilter)
        {
            return Query(roleFilter, (connection, factory, data) =>
            {
                var repository = factory.CreateRepository(connection);
                var role = repository.CreateRoleRepository(connection);
                return role.GetPagination(data);
            });
        }

        /// <summary>
        /// 获取角色分组
        /// </summary>
        /// <returns>角色分组</returns>
        public IEnumerable<RoleGroupByResponse> GetRoleGroupByResponses()
        {
            return Query((connection, factory) =>
            {
                var repository = factory.CreateRepository(connection);
                var role = repository.CreateRoleRepository(connection);
                return role.GetRoleGroupByResponses();
            });
        }
    }
}