using System.Data;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.IRepository;

namespace Easy.Tall.UserCenter.Repository.MySql
{
    /// <summary>
    /// 仓储
    /// </summary>
    public class MySqlRepository : IRepository.IRepository
    {
        /// <summary>
        /// 创建用户仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <returns>用户仓储</returns>
        public IUserRepository CreateUserRepository(IUnitOfWork unitOfWork)
        {
            return new MySqlUserRepository(unitOfWork);
        }

        /// <summary>
        /// 创建用户仓储
        /// </summary>
        /// <param name="dbConnection">数据库链接</param>
        /// <returns>用户仓储</returns>
        public IUserRepository CreateUserRepository(IDbConnection dbConnection)
        {
            return new MySqlUserRepository(dbConnection);
        }

        /// <summary>
        /// 创建分类仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <returns>分类仓储</returns>
        public IClassifyRepository CreateClassifyRepository(IUnitOfWork unitOfWork)
        {
            return new MySqlClassifyRepository(unitOfWork);
        }

        /// <summary>
        /// 创建分类仓储
        /// </summary>
        /// <param name="dbConnection">数据库链接</param>
        /// <returns>分类仓储</returns>
        public IClassifyRepository CreateClassifyRepository(IDbConnection dbConnection)
        {
            return new MySqlClassifyRepository(dbConnection);
        }

        /// <summary>
        /// 创建角色仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <returns>角色仓储</returns>
        public IRoleRepository CreateRoleRepository(IUnitOfWork unitOfWork)
        {
            return new MySqlRoleRepository(unitOfWork);
        }

        /// <summary>
        /// 创建角色仓储
        /// </summary>
        /// <param name="dbConnection">数据库链接</param>
        /// <returns>角色仓储</returns>
        public IRoleRepository CreateRoleRepository(IDbConnection dbConnection)
        {
            return new MySqlRoleRepository(dbConnection);
        }

        /// <summary>
        /// 创建权限仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <returns>权限仓储</returns>
        public IPermissionRepository CreatePermissionRepository(IUnitOfWork unitOfWork)
        {
            return new MySqlPermissionRepository(unitOfWork);
        }

        /// <summary>
        /// 创建权限仓储
        /// </summary>
        /// <param name="dbConnection">数据库链接</param>
        /// <returns>权限仓储</returns>
        public IPermissionRepository CreatePermissionRepository(IDbConnection dbConnection)
        {
            return new MySqlPermissionRepository(dbConnection);
        }

        /// <summary>
        /// 创建角色权限仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <returns>角色权限仓储</returns>
        public IRolePermissionRelationRepository CreateRolePermissionRelationRepository(IUnitOfWork unitOfWork)
        {
            return new MySqlRolePermissionRelationRepository(unitOfWork);
        }

        /// <summary>
        /// 创建角色权限仓储
        /// </summary>
        /// <param name="dbConnection">数据库链接</param>
        /// <returns>角色权限仓储</returns>
        public IRolePermissionRelationRepository CreateRolePermissionRelationRepository(IDbConnection dbConnection)
        {
            return new MySqlRolePermissionRelationRepository(dbConnection);
        }
    }
}