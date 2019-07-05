using System.Data;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.IRepository;

namespace Easy.Tall.UserCenter.Repository.MySql
{
    /// <summary>
    /// 仓储
    /// </summary>
    public class MysqlRepository : IRepository.IRepository
    {
        /// <summary>
        /// 创建用户仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <returns>用户仓储</returns>
        public IUserRepository CreateUserRepository(IUnitOfWork unitOfWork)
        {
            return new MysqlUserRepository(unitOfWork);
        }

        /// <summary>
        /// 创建用户仓储
        /// </summary>
        /// <param name="dbConnection">数据库链接</param>
        /// <returns>用户仓储</returns>
        public IUserRepository CreateUserRepository(IDbConnection dbConnection)
        {
            return new MysqlUserRepository(dbConnection);
        }

        /// <summary>
        /// 创建分类仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <returns>分类仓储</returns>
        public IClassifyRepository CreateClassifyRepository(IUnitOfWork unitOfWork)
        {
            return new MysqlClassifyRepository(unitOfWork);
        }

        /// <summary>
        /// 创建分类仓储
        /// </summary>
        /// <param name="dbConnection">数据库链接</param>
        /// <returns>分类仓储</returns>
        public IClassifyRepository CreateClassifyRepository(IDbConnection dbConnection)
        {
            return new MysqlClassifyRepository(dbConnection);
        }

        /// <summary>
        /// 创建角色仓储
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <returns>角色仓储</returns>
        public IRoleRepository CreateRoleRepository(IUnitOfWork unitOfWork)
        {
            return new MysqlRoleRepository(unitOfWork);
        }

        /// <summary>
        /// 创建角色仓储
        /// </summary>
        /// <param name="dbConnection">数据库链接</param>
        /// <returns>角色仓储</returns>
        public IRoleRepository CreateRoleRepository(IDbConnection dbConnection)
        {
            return new MysqlRoleRepository(dbConnection);
        }
    }
}