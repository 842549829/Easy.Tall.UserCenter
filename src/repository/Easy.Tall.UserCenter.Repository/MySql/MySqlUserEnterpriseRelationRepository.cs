using System.Data;
using Dapper;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.Framework.Exceptions;
using Easy.Tall.UserCenter.IRepository;

namespace Easy.Tall.UserCenter.Repository.MySql
{
    /// <summary>
    /// 用户企业关系
    /// </summary>
    public class MySqlUserEnterpriseRelationRepository : BaseRepository, IUserEnterpriseRelationRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unit">工作单元</param>
        public MySqlUserEnterpriseRelationRepository(IUnitOfWork unit) : base(unit)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbConnection">数据库连接字符串</param>
        public MySqlUserEnterpriseRelationRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        public void Add(UserEnterpriseRelation entity)
        {
            var sql = "INSERT INTO `UserEnterpriseRelation` (UserId,EnterpriseId) VALUES(@UserId,@EnterpriseId);";
            var result = Connection.Execute(sql, entity, Transaction);
            if (result < 1)
            {
                throw new BusinessException(1, "添加用户企业关系失败");
            }
        }
    }
}