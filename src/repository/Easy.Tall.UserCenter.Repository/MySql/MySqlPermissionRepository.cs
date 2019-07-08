using System.Data;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Data;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.IRepository;
using Dapper;

namespace Easy.Tall.UserCenter.Repository.MySql
{
    /// <summary>
    /// 权限仓储
    /// </summary>
    public class MySqlPermissionRepository : BaseRepository, IPermissionRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unit">工作单元</param>
        public MySqlPermissionRepository(IUnitOfWork unit) : base(unit)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbConnection">数据库连接字符串</param>
        public MySqlPermissionRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        public void Add(Permission entity)
        {
            var sql = "INSERT INTO Permission (Id,CreateTime,ModifyTime,Name,ClassifyId,`Describe`) VALUES (@Id,@CreateTime,@ModifyTime,@Name,@ClassifyId,@Describe);";
            Connection.Execute(sql, entity);
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="entity">T</param>
        /// <returns>返回结果</returns>
        public void Remove(Permission entity)
        {
            var sql = "DELETE FROM Permission WHERE Id=@Id;";
            Connection.Execute(sql, entity);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        public void Update(Permission entity)
        {
            var sql = "UPDATE Permission Name=@Name,ClassifyId=@ClassifyId,`Describe`=@Describe WHERE Id=@Id;";
            Connection.Execute(sql, entity);
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="key">id</param>
        /// <returns>返回查询单条数据</returns>
        public Permission Query(string key)
        {
            var sql = "SELECT * FROM Permission WHERE Id=@Id;";
            return Connection.QuerySingleOrDefault<Permission>(sql, new { Id = key });
        }

        /// <summary>
        /// 权限分页查询
        /// </summary>
        /// <param name="permissionFilter">权限查询条件</param>
        /// <returns>查询数据</returns>
        public Pagination<PermissionPaginationResponse> GetPagination(PermissionFilter permissionFilter)
        {
            throw new System.NotImplementedException();
        }
    }
}