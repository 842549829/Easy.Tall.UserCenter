using System.Collections.Generic;
using System.Linq;
using CSRedis;
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
        /// 权限缓存
        /// </summary>
        private readonly IPermissionCacheService _permissionCacheService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbUnitOfWorkFactory">工作单元</param>
        /// <param name="dbConnectionFactory">数据库链接</param>
        /// <param name="repositoryFactory">仓储工厂</param>
        /// <param name="permissionCacheService">权限缓存</param>
        /// <param name="logger">日志</param>
        public PermissionService(IDbUnitOfWorkFactory dbUnitOfWorkFactory,
            IDbConnectionFactory dbConnectionFactory,
            IRepositoryFactory repositoryFactory,
            IPermissionCacheService permissionCacheService,
            ILogger<PermissionService> logger)
            : base(dbUnitOfWorkFactory, dbConnectionFactory, repositoryFactory, logger)
        {
            _permissionCacheService = permissionCacheService;
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
                var permission = repository.CreatePermissionRepository(unitOfWork);
                permission.Add(data.ToPermission());
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
                var permission = repository.CreatePermissionRepository(unitOfWork);
                if (data.IsChildNodes)
                {
                    permission.Remove(new Permission { Id = data.Id });
                    permission.RemoveChildren(data.Id);
                }
                else
                {
                    if (permission.ContainsChildren(data.Id))
                    {
                        throw new BusinessException("包含子节点不允许删除");
                    }
                    permission.Remove(new Permission { Id = data.Id });
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
                function.Update(data.ToPermission());
            });
        }

        /// <summary>
        /// 编辑权限
        /// </summary>
        /// <param name="permissionEditRequest">权限信息</param>
        /// <returns>结果</returns>
        public Result<bool> Edit(PermissionEditRequest permissionEditRequest)
        {
            // 根据角色查询角色权限Id
            // 需要删除的权限
            // 需要新增的权限
            return Execute(permissionEditRequest, (unitOfWork, factory, data) =>
            {
                var repository = factory.CreateRepository(unitOfWork.Connection);
                var rolePermissionRelationRepository = repository.CreateRolePermissionRelationRepository(unitOfWork);
                var rolePermissionRelation = rolePermissionRelationRepository.QueryByRoleId(data.RoleId);
                var oldRolePermissionRelation = rolePermissionRelation.ToPermissionIdList().ToList();
                var newRolePermissionRelation = data.PermissionIdList.ToList();
                // 待删除的角色权限
                var awaitRemovePermissionId = oldRolePermissionRelation.Except(newRolePermissionRelation).ToList();
                var awaitRemoveRolePermissionRelation = awaitRemovePermissionId.ToRolePermissionRelation(data.RoleId).ToList();
                // 待添加的角色权限
                var awaitAddRolePermissionId = newRolePermissionRelation.Except(oldRolePermissionRelation).ToList();
                var awaitAddRolePermissionRelation = awaitAddRolePermissionId.ToRolePermissionRelation(data.RoleId).ToList();
                rolePermissionRelationRepository.RemoveRange(awaitRemoveRolePermissionRelation);
                rolePermissionRelationRepository.AddRange(awaitAddRolePermissionRelation);
            });
        }

        /// <summary>
        /// 查询权限
        /// </summary>
        /// <param name="permissionClassify">所属分类</param>
        /// <returns>权限</returns>
        public IEnumerable<Permission> GetPermissions(PermissionClassify permissionClassify)
        {
            return Query(permissionClassify, (connection, repositoryFactory, filter) =>
            {
                var repository = repositoryFactory.CreateRepository(connection);
                var function = repository.CreatePermissionRepository(connection);
                return function.GetPermissions(filter);
            });
        }

        /// <summary>
        /// 查询权限
        /// </summary>
        /// <param name="permissionsFilter">查询条件</param>
        /// <returns>权限</returns>
        public IEnumerable<PermissionResponse> GetPermissions(PermissionFilter permissionsFilter)
        {
            var permissions = Query(permissionsFilter, (connection, factory, filter) =>
            {
                var repository = factory.CreateRepository(connection);
                var permission = repository.CreatePermissionRepository(connection);
                var paths = permission.GetPermissionPaths(filter.ToPermissionPathFilter()).ToList();
                var permissionList = permission.GetPermissions(filter.PermissionClassify);
                return permissionList.Select(item => item.ToPermissionResponse(paths.Contains(item.Path)));
            });
            return permissions;
        }

        /// <summary>
        /// 查询权限路径
        /// </summary>
        /// <param name="permissionPathFilter">查询条件</param>
        /// <returns>权限</returns>
        public IEnumerable<string> GetPermissionPaths(PermissionPathFilter permissionPathFilter)
        {
            var permissionPaths = Query(permissionPathFilter, (connection, factory, filter) =>
            {
                var repository = factory.CreateRepository(connection);
                var permission = repository.CreatePermissionRepository(connection);
                return permission.GetPermissionPaths(filter);
            }).ToList();
            _permissionCacheService.Add(permissionPathFilter.UserId, permissionPaths, 1800);
            return permissionPaths;
        }
    }
}