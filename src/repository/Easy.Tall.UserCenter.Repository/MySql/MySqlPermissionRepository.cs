﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using Easy.Tall.UserCenter.Entity.Enum;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.IRepository;
using Dapper;
using Easy.Tall.UserCenter.Entity.Extend;

namespace Easy.Tall.UserCenter.Repository.MySql
{
    /// <summary>
    /// 权限
    /// </summary>
    public class MySqlPermissionRepository : BaseRepository, IPermissionRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unit">工作单元</param>
        public MySqlPermissionRepository(IUnitOfWork unit) : base(unit)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbConnection">数据库连接字符串</param>
        public MySqlPermissionRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        public void Add(Permission entity)
        {
            var sql = "INSERT INTO `Permission` (Id,CreateTime,ModifyTime,Name,ParentId,Icon,`Type`,Sort,Path,`Describe`,Flag,Classify) VALUES(@Id,@CreateTime,@ModifyTime,@Name,@ParentId,@Icon,@Type,@Sort,@Path,@Describe,@Flag,@Classify);";
            Connection.Execute(sql, entity, Transaction);
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="entity">T</param>
        /// <returns>返回结果</returns>
        public void Remove(Permission entity)
        {
            var sql = "DELETE FROM `Permission` WHERE Id=@Id;";
            Connection.Execute(sql, entity, Transaction);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        public void Update(Permission entity)
        {
            var sql = "UPDATE `Permission` SET Icon=@Icon,Sort=@Sort,Path=@Path,`Describe`=@Describe WHERE Id=@Id";
            Connection.Execute(sql, entity, Transaction);
        }

        /// <summary>
        /// 查询权限
        /// </summary>
        /// <param name="parentId">父级Id</param>
        /// <returns>权限</returns>
        public IEnumerable<Permission> GetPermissions(string parentId)
        {
            var sql = @"WITH RECURSIVE _children AS
                        (
                         SELECT fun.* FROM `Permission` fun WHERE fun.ParentId=@ParentId
                            UNION ALL
                         SELECT fun.* FROM _children,`Permission` fun WHERE fun.ParentId=_children.Id
                        )
                        SELECT * FROM _children;";
            return Connection.Query<Permission>(sql, new { ParentId = parentId }, Transaction);
        }

        /// <summary>
        /// 查询权限
        /// </summary>
        /// <param name="permissionClassify">分类</param>
        /// <returns>权限</returns>
        public IEnumerable<Permission> GetPermissions(PermissionClassify permissionClassify)
        {
            var sql = @"WITH RECURSIVE _children AS
                        (
                         SELECT per.* FROM `Permission` per WHERE (per.ParentId IS NULL OR per.ParentId = '0') AND (per.Classify =@Classify)
                            UNION ALL
                         SELECT per.* FROM _children,`Permission` per WHERE per.ParentId=_children.Id
                        )
                        SELECT * FROM _children;";
            return Connection.Query<Permission>(sql, new { Classify = permissionClassify }, Transaction);
        }

        /// <summary>
        /// 查询权限
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns>权限</returns>
        public IEnumerable<Permission> GetPermissionByUserId(PermissionFilter filter)
        {
            var sql = "SELECT * FROM Permission AS P INNER JOIN (SELECT DISTINCT PermissionId FROM RolePermissionRelation AS R WHERE R.RoleId IN (SELECT RoleId FROM UserRoleRelation As U WHERE U.UserId =@UserId)) AS T ON P.Id = T.PermissionId WHERE P.Classify = @Classify;";
            return Connection.Query<Permission>(sql, new { Classify = filter.PermissionClassify, UserId = filter.Id }, Transaction);
        }

        /// <summary>
        /// 查询权限路径
        /// </summary>
        /// <param name="permissionClassify">分类</param> 
        /// <returns>权限</returns>
        public IEnumerable<string> GetPermissionPath(PermissionClassify permissionClassify)
        {
            var sql = "SELECT PermissionId FROM Permission AS WHERE P.Classify = @Classify";
            return Connection.Query<string>(sql, new { Classify = permissionClassify }, Transaction);
        }

        /// <summary>
        /// 查询权限路径
        /// </summary>
        /// <param name="permissionFilter">条件</param> 
        /// <returns>权限</returns>
        public IEnumerable<string> GetPermissionPathByUserId(PermissionFilter permissionFilter)
        {
            var sql = "SELECT DISTINCT PermissionId FROM Permission AS P INNER JOIN RolePermissionRelation AS R ON P.Id = R.PermissionId WHERE P.Classify = @Classify AND R.RoleId IN (SELECT RoleId FROM UserRoleRelation WHERE UserId = @UserId)";
            return Connection.Query<string>(sql, permissionFilter, Transaction);
        }

        /// <summary>
        /// 查询权限路径
        /// </summary>
        /// <param name="permissionFilter">条件</param>
        /// <returns>权限</returns>
        public IEnumerable<string> GetPermissionPathByRoleId(PermissionFilter permissionFilter)
        {
            var sql = "SELECT DISTINCT PermissionId FROM Permission AS P INNER JOIN RolePermissionRelation AS R ON P.Id = R.PermissionId WHERE P.Classify = @Classify AND R.RoleId = @RoleId;";
            return Connection.Query<string>(sql, permissionFilter, Transaction);
        }

        /// <summary>
        /// 删除节点包含所有的子节点
        /// </summary>
        /// <param name="id">id</param>
        public void RemoveChildren(string id)
        {
            var sql = @"WITH RECURSIVE _children AS
                        (
                         SELECT fun.* FROM `Permission` fun WHERE fun.ParentId=@ParentId
                            UNION ALL
                         SELECT fun.* FROM _children,`Permission` fun WHERE fun.ParentId=_children.Id
                        )
                       DELETE FROM `Function` WHERE Id IN (SELECT Id FROM _children);";
            Connection.Execute(sql, new { ParentId = id }, Transaction);
        }

        /// <summary>
        /// 是否包含子节点
        /// </summary>
        /// <param name="parentId">上级父Id</param>
        /// <returns>结果</returns>
        public bool ContainsChildren(string parentId)
        {
            var sql = "SELECT COUNT(1) FROM `Permission` WHERE ParentId=@ParentId;";
            var count = Connection.Query<int>(sql, new { ParentId = parentId }, Transaction).SingleOrDefault();
            return count > 0;
        }

        /// <summary>
        /// 是否使用该分类
        /// </summary>
        /// <param name="classifyId">分类Id</param>
        /// <returns>结果</returns>
        public bool ContainsClassifyType(string classifyId)
        {
            var sql = "SELECT COUNT(1) FROM `Permission` WHERE ClassifyId=@ClassifyId;";
            var count = Connection.Query<int>(sql, new { ClassifyId = classifyId }, Transaction).SingleOrDefault();
            return count > 0;
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="key">id</param>
        /// <returns>返回查询单条数据</returns>
        public Permission Query(string key)
        {
            var sql = "SELECT COUNT(1) FROM `Permission` WHERE Id=@Id;";
            return Connection.Query<Permission>(sql, new { Id = key }, Transaction).SingleOrDefault();
        }
    }
}