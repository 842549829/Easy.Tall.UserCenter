using System.Data;
using Easy.Tall.UserCenter.IRepository;
using Easy.Tall.UserCenter.Repository.MySql;
using MySql.Data.MySqlClient;

namespace Easy.Tall.UserCenter.Repository
{
    /// <summary>
    /// 仓储工厂接口
    /// </summary>
    public class RepositoryFactory : IRepositoryFactory
    {
        /// <summary>
        /// 创建仓储
        /// </summary>
        /// <param name="dbConnection">数据库链接</param>
        /// <returns>仓储</returns>
        public IRepository.IRepository CreateRepository(IDbConnection dbConnection)
        {
            if (dbConnection is MySqlConnection)
            {
                return new MysqlRepository();
            }
            return new MysqlRepository();
        }
    }
}