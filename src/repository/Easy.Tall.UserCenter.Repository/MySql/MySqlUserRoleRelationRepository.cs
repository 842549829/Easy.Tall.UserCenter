using System.Collections.Generic;
using System.Data;
using Dapper;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.IRepository;

namespace Easy.Tall.UserCenter.Repository.MySql
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class MySqlUserRoleRelationRepository : BaseRepository, IUserRoleRelationRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unit">工作单元</param>
        public MySqlUserRoleRelationRepository(IUnitOfWork unit) : base(unit)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbConnection">数据库连接字符串</param>
        public MySqlUserRoleRelationRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="userRoleRelation">用户角色</param>
        public void AddRange(IEnumerable<UserRoleRelation> userRoleRelation)
        {
            string sql = "INSERT INTO UserRoleRelation (RoleId,UserId) VALUES (@RoleId,@UserId);";
            Connection.Execute(sql, userRoleRelation, Transaction);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userRoleRelation">用户角色</param>
        public void RemoveRange(IEnumerable<UserRoleRelation> userRoleRelation)
        {
            string sql = "DELETE FROM UserRoleRelation WHERE RoleId =@RoleId AND UserId=@UserId;";
            Connection.Execute(sql, userRoleRelation, Transaction);
        }

        /// <summary>
        /// 根据角色Id查询用户角色
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns>结果</returns>
        public IEnumerable<UserRoleRelation> QueryByRoleId(string roleId)
        {
            var sql = "SELECT * FROM UserRoleRelation WHERE RoleId=@RoleId;";
            return Connection.Query<UserRoleRelation>(sql, new { RoleId = roleId }, Transaction);
        }

        /// <summary>
        /// 根据用户Id查询用户角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>结果</returns>
        public IEnumerable<UserRoleRelation> QueryByUserId(string userId)
        {
            var sql = "SELECT * FROM UserRoleRelation WHERE UserId=@UserId;";
            return Connection.Query<UserRoleRelation>(sql, new { UserId = userId }, Transaction);
        }
    }
}