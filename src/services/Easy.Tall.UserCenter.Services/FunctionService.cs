using System.Collections.Generic;
using Easy.Tall.UserCenter.Entity.Enum;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Data;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.Framework.Exceptions;
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
            return Execute(permissionAddRequest, (unitOfWork, repositoryFactory, data) =>
            {
                var repository = repositoryFactory.CreateRepository(unitOfWork.Connection);
                var function = repository.CreatePermissionRepository(unitOfWork);
                function.Add(data.ToPermission());
            });
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="permissionRemoveRequest">删除信息</param>
        /// <returns>结果</returns>
        public Result<bool> Remove(PermissionRemoveRequest permissionRemoveRequest)
        {
            return Execute(permissionRemoveRequest, (unitOfWork, repositoryFactory, data) =>
            {
                var repository = repositoryFactory.CreateRepository(unitOfWork.Connection);
                var function = repository.CreatePermissionRepository(unitOfWork);
                if (data.IsChildNodes)
                {
                    function.Remove(new Permission { Id = data.Id });
                    function.RemoveChildren(data.Id);
                }
                else
                {
                    if (function.ContainsChildren(data.Id))
                    {
                        throw new BusinessException("包含子节点不允许删除");
                    }
                    function.Remove(new Permission { Id = data.Id });
                }
            });
        }

        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="permissionUpdateRequest">权限信息</param>
        /// <returns>结果</returns>
        public Result<bool> Update(PermissionUpdateRequest permissionUpdateRequest)
        {
            return Execute(permissionUpdateRequest, (unitOfWork, repositoryFactory, data) =>
            {
                var repository = repositoryFactory.CreateRepository(unitOfWork.Connection);
                var function = repository.CreatePermissionRepository(unitOfWork);
                function.Add(data.ToPermission());
            });
        }

        /// <summary>
        /// 查询权限
        /// </summary>
        /// <param name="permissionClassify">所属分类</param>
        /// <returns>权限</returns>
        public IEnumerable<Permission> GetFunctions(PermissionClassify permissionClassify)
        {
            return Query(permissionClassify, (connection, repositoryFactory, filter) =>
            {
                var repository = repositoryFactory.CreateRepository(connection);
                var function = repository.CreatePermissionRepository(connection);
                return function.GetPermissions(filter);
            });
        }
    }
}