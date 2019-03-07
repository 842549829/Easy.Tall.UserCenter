﻿using System.Data;
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
    }
}