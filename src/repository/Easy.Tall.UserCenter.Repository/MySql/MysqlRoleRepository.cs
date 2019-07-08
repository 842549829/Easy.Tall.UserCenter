using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Data;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.Framework.Exceptions;
using Easy.Tall.UserCenter.IRepository;

namespace Easy.Tall.UserCenter.Repository.MySql
{
    /// <summary>
    /// 角色仓储
    /// </summary>
    public class MySqlRoleRepository : BaseRepository, IRoleRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unit">工作单元</param>
        public MySqlRoleRepository(IUnitOfWork unit) : base(unit)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbConnection">数据库连接字符串</param>
        public MySqlRoleRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        public void Add(Role entity)
        {
            var sql = "INSERT INTO `Role` (Id,CreateTime,ModifyTime,Name,ClassifyId,Describe) VALUES(@Id,@CreateTime,@ModifyTime,@Name,@ClassifyId,@Describe);";
            var result = Connection.Execute(sql, entity);
            if (result < 1)
            {
                throw new BusinessException(1, "添加角色失败");
            }
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="entity">T</param>
        /// <returns>返回结果</returns>
        public void Remove(Role entity)
        {
            string sql = "DELETE FROM `Role` WHERE Id=@Id;";
            int result = Connection.Execute(sql, entity);
            if (result < 1)
            {
                throw new BusinessException(1, "删除角色失败");
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        public void Update(Role entity)
        {
            var sql = "UPDATE `Role` Name=@Name,ClassifyId=@ClassifyId,Describe=@Describe WHERE Id=@Id; ";
            Connection.Execute(sql, entity);
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="key">id</param>
        /// <returns>返回查询单条数据</returns>
        public Role Query(string key)
        {
            var sql = "SELECT * FROM `Role` WHERE Id =@Id;";
            return Connection.Query<Role>(sql, new { Id = key }).SingleOrDefault();
        }

        /// <summary>
        /// 角色分页查询
        /// </summary>
        /// <param name="roleFilter">角色查询条件</param>
        /// <returns>查询数据</returns>
        public Pagination<RolePaginationResponse> GetPagination(RoleFilter roleFilter)
        {
            var sqlCondition = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(roleFilter.Name))
            {
                sqlCondition.Append(" AND Name = @Name");
            }
            if (!string.IsNullOrWhiteSpace(roleFilter.ClassifyId))
            {
                sqlCondition.Append(" AND ClassifyId = @ClassifyId");
            }
            var sqlConditionStr = sqlCondition.ToString();
            var condition = string.IsNullOrWhiteSpace(sqlConditionStr) ? string.Empty : sqlConditionStr.Substring(4);
            var sqlCount = $"SELECT COUNT(1) FROM `Role` WHERE {condition};";
            var count = Connection.Query<int>(sqlCount, roleFilter).SingleOrDefault();
            var sqlData = $"SELECT *,(SELECT `Name` FROM Classify WHERE Id = ClassifyId) AS Classify FROM Role WHERE {condition} ORDER BY CreateTime DESC LIMIT @PageIndex, @PageSize;";
            var data = Connection.Query<RolePaginationResponse>(sqlData, roleFilter);
            return new Pagination<RolePaginationResponse>
            {
                Count = count,
                Data = data
            };
        }
    }
}