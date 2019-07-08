
using System.Data;
using Easy.Tall.UserCenter.Framework.Db;

namespace Easy.Tall.UserCenter.IRepository
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// 创建用户仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <returns>用户仓储</returns>
        IUserRepository CreateUserRepository(IUnitOfWork unitOfWork);

        /// <summary>
        /// 创建用户仓储
        /// </summary>
        /// <param name="dbConnection">数据库链接</param>
        /// <returns>用户仓储</returns>
        IUserRepository CreateUserRepository(IDbConnection dbConnection);

        /// <summary>
        /// 创建分类仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <returns>分类仓储</returns>
        IClassifyRepository CreateClassifyRepository(IUnitOfWork unitOfWork);

        /// <summary>
        /// 创建分类仓储
        /// </summary>
        /// <param name="dbConnection">数据库链接</param>
        /// <returns>分类仓储</returns>
        IClassifyRepository CreateClassifyRepository(IDbConnection dbConnection);

        /// <summary>
        /// 创建角色仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <returns>角色仓储</returns>
        IRoleRepository CreateRoleRepository(IUnitOfWork unitOfWork);

        /// <summary>
        /// 创建角色仓储
        /// </summary>
        /// <param name="dbConnection">数据库链接</param>
        /// <returns>角色仓储</returns>
        IRoleRepository CreateRoleRepository(IDbConnection dbConnection);

        /// <summary>
        /// 创建权限仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <returns>权限仓储</returns>
        IPermissionRepository CreatePermissionRepository(IUnitOfWork unitOfWork);

        /// <summary>
        /// 创建权限仓储
        /// </summary>
        /// <param name="dbConnection">数据库链接</param>
        /// <returns>权限仓储</returns>
        IPermissionRepository CreatePermissionRepository(IDbConnection dbConnection);
    }
}