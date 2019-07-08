using System;
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
    /// 分类仓储
    /// </summary>
    public class MySqlClassifyRepository : BaseRepository, IClassifyRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unit">工作单元</param>
        public MySqlClassifyRepository(IUnitOfWork unit) : base(unit)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbConnection">数据库连接字符串</param>
        public MySqlClassifyRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        public void Add(Classify entity)
        {
            var sql = "INSERT INTO `Classify` (Id,CreateTime,ModifyTime,Name,Type) VALUES(@Id,@CreateTime,@ModifyTime,@Name,@Type);";
            var result = Connection.Execute(sql, entity);
            if (result < 1)
            {
                throw new BusinessException(1, "添加分类失败");
            }
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="entity">T</param>
        /// <returns>返回结果</returns>
        public void Remove(Classify entity)
        {
            string sql = "DELETE FROM `Classify` WHERE Id=@Id;";
            int result = Connection.Execute(sql, entity);
            if (result < 1)
            {
                throw new BusinessException(1, "删除分类失败");
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        public void Update(Classify entity)
        {
            var sql = "UPDATE `Classify` Name=@Name,Describe=@Describe WHERE Id=@Id;";
            Connection.Execute(sql, entity);
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="key">id</param>
        /// <returns>返回查询单条数据</returns>
        public Classify Query(string key)
        {
            var sql = "SELECT * FROM `Classify` WHERE Id =@Id;";
            return Connection.Query<Classify>(sql, new { Id = key }).SingleOrDefault();
        }

        /// <summary>
        /// 分类分页查询
        /// </summary>
        /// <param name="userFilter">分类查询条件</param>
        /// <returns>查询数据</returns>
        public Pagination<ClassifyPaginationResponse> GetPagination(ClassifyFilter userFilter)
        {
            var sqlCondition = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(userFilter.Name))
            {
                sqlCondition.Append(" AND Name = @Name");
            }
            if (userFilter.Type.HasValue)
            {
                sqlCondition.Append(" AND Type = @Type");
            }
            var sqlConditionStr = sqlCondition.ToString();
            var condition = string.IsNullOrWhiteSpace(sqlConditionStr) ? string.Empty : sqlConditionStr.Substring(4);
            var sqlCount = $"SELECT COUNT(1) FROM `Classify` WHERE {condition};";
            var count = Connection.Query<int>(sqlCount, userFilter).SingleOrDefault();
            var sqlData = $"SELECT * FROM `Classify` WHERE {condition} ORDER BY CreateTime DESC LIMIT @PageIndex, @PageSize;";
            var data = Connection.Query<ClassifyPaginationResponse>(sqlData, userFilter);
            return new Pagination<ClassifyPaginationResponse>
            {
                Count = count,
                Data = data
            };
        }
    }
}