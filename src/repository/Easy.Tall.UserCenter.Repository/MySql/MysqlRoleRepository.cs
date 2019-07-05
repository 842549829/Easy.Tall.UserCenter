using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Data;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.IRepository;

namespace Easy.Tall.UserCenter.Repository.MySql
{
    /// <summary>
    /// 角色仓储
    /// </summary>
    public class MysqlRoleRepository : BaseRepository, IRoleRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unit">工作单元</param>
        public MysqlRoleRepository(IUnitOfWork unit) : base(unit)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbConnection">数据库连接字符串</param>
        public MysqlRoleRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        public void Add(Role entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="entity">T</param>
        /// <returns>返回结果</returns>
        public void Remove(Role entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        public void Update(Role entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="key">id</param>
        /// <returns>返回查询单条数据</returns>
        public Role Query(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 角色分页查询
        /// </summary>
        /// <param name="roleFilter">角色查询条件</param>
        /// <returns>查询数据</returns>
        public Pagination<RolePaginationResponse> GetPagination(RoleFilter roleFilter)
        {
            throw new NotImplementedException();
        }
    }
}