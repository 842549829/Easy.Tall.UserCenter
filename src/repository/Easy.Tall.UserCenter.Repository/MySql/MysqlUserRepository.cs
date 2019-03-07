using System;
using System.Data;
using Dapper;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.Framework.Exceptions;
using Easy.Tall.UserCenter.IRepository;

namespace Easy.Tall.UserCenter.Repository.MySql
{
    /// <summary>
    /// 用户仓储接口
    /// </summary>
    public class MysqlUserRepository : BaseRepository, IUserRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unit">工作单元</param>
        public MysqlUserRepository(IUnitOfWork unit) : base(unit)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbConnection">数据库连接字符串</param>
        public MysqlUserRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        public void Add(User entity)
        {
            string sql = "INSERT INTO `User` (Id,CreateTime,ModifyTime) VALUES(@Id,@CreateTime,@ModifyTime);";
            int result = Connection.Execute(sql, entity);
            if (result < 1)
            {
                throw new BusinessException(1, "添加用户失败");
            }
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="entity">T</param>
        /// <returns>返回结果</returns>
        public void Remove(User entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        public void Update(User entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="key">id</param>
        /// <returns>返回查询单条数据</returns>
        public User Query(string key)
        {
            throw new NotImplementedException();
        }
    }
}