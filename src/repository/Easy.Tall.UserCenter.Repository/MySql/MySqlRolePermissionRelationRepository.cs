using System.Collections.Generic;
using System.Data;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.IRepository;
using Dapper;

namespace Easy.Tall.UserCenter.Repository.MySql
{
    /// <summary>
    /// 角色权限
    /// </summary>
    public class MySqlRolePermissionRelationRepository : BaseRepository, IRolePermissionRelationRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unit">工作单元</param>
        public MySqlRolePermissionRelationRepository(IUnitOfWork unit) : base(unit)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbConnection">数据库连接字符串</param>
        public MySqlRolePermissionRelationRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        /// <summary>
        /// 添加角色权限
        /// </summary>
        /// <param name="rolePermissionRelations">角色权限</param>
        public void AddRange(IEnumerable<RolePermissionRelation> rolePermissionRelations)
        {
            string sql = "INSERT INTO RolePermissionRelation (RoleId,PermissionId) VALUES (@RoleId,@PermissionId);";
            Connection.Execute(sql, rolePermissionRelations, Transaction);
        }

        /// <summary>
        /// 删除角色权限
        /// </summary>
        /// <param name="rolePermissionRelations">角色权限</param>
        public void RemoveRange(IEnumerable<RolePermissionRelation> rolePermissionRelations)
        {
            string sql = "DELETE FROM RolePermissionRelation WHERE RoleId =@RoleId AND PermissionId=@PermissionId;";
            Connection.Execute(sql, rolePermissionRelations, Transaction);
        }

        /// <summary>
        /// 根据角色Id查询角色权限
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns>结果</returns>
        public IEnumerable<RolePermissionRelation> QueryByRoleId(string roleId)
        {
            var sql = "SELECT * FROM RolePermissionRelation WHERE RoleId=@RoleId;";
            return Connection.Query<RolePermissionRelation>(sql, new { RoleId = roleId }, Transaction);
        }
    }
}