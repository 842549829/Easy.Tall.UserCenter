
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
    }
}