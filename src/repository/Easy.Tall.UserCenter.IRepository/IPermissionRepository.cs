using System.Collections.Generic;
using Easy.Tall.UserCenter.Entity.Enum;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Db;

namespace Easy.Tall.UserCenter.IRepository
{
    /// <summary>
    /// 权限
    /// </summary>
    public interface IPermissionRepository : IRepository<string, Permission>
    {
        /// <summary>
        /// 查询权限
        /// </summary>
        /// <param name="parentId">父级Id</param>
        /// <returns>权限</returns>
        IEnumerable<Permission> GetPermissions(string parentId);

        /// <summary>
        /// 查询权限
        /// </summary>
        /// <param name="permissionClassify">分类</param>
        /// <returns>权限</returns>
        IEnumerable<Permission> GetPermissions(PermissionClassify permissionClassify);

        /// <summary>
        /// 查询权限路径
        /// </summary>
        /// <param name="permissionPathFilter">条件</param>
        /// <returns>权限</returns>
        IEnumerable<string> GetPermissionPaths(PermissionPathFilter permissionPathFilter);

        /// <summary>
        /// 删除节点包含所有的子节点
        /// </summary>
        /// <param name="id">id</param>
        void RemoveChildren(string id);

        /// <summary>
        /// 是否包含子节点
        /// </summary>
        /// <param name="parentId">上级父Id</param>
        /// <returns>结果</returns>
        bool ContainsChildren(string parentId);

        /// <summary>
        /// 是否使用该分类
        /// </summary>
        /// <param name="classifyId">分类Id</param>
        /// <returns>结果</returns>
        bool ContainsClassifyType(string classifyId);
    }
}