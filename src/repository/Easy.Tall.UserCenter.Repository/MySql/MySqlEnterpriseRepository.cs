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
    /// 企业仓储
    /// </summary>
    public class MySqlEnterpriseRepository : BaseRepository, IEnterpriseRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unit">工作单元</param>
        public MySqlEnterpriseRepository(IUnitOfWork unit) : base(unit)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbConnection">数据库连接字符串</param>
        public MySqlEnterpriseRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        public void Add(Enterprise entity)
        {
            var sql = "INSERT INTO `Enterprise` (Id,CreateTime,ModifyTime,Account,Name,Abbreviation,OrganizingCode,Contacts,ContactMobile,Address,Mail,Fax,Person,Type,ParentId) VALUES(@Id,@CreateTime,@ModifyTime,@Account,@Name,@Abbreviation,@OrganizingCode,@Contacts,@ContactMobile,@Address,@Mail,@Fax,@Person,@Type,@ParentId);";
            var result = Connection.Execute(sql, entity, Transaction);
            if (result < 1)
            {
                throw new BusinessException(1, "添加企业失败");
            }
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="entity">T</param>
        /// <returns>返回结果</returns>
        public void Remove(Enterprise entity)
        {

            string sql = "DELETE FROM `Enterprise` WHERE Id=@Id;";
            int result = Connection.Execute(sql, entity, Transaction);
            if (result < 1)
            {
                throw new BusinessException(1, "删除企业失败");
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        public void Update(Enterprise entity)
        {
            string sql = "UPDATE `Enterprise` SET Abbreviation=@Abbreviation WHERE Id=@Id;";
            Connection.Execute(sql, entity, Transaction);
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="key">id</param>
        /// <returns>返回查询单条数据</returns>
        public Enterprise Query(string key)
        {
            string sql = "SELECT * FROM `Enterprise` WHERE Id=@Id;";
            return Connection.QueryFirst<Enterprise>(sql, new Enterprise { Id = key }, Transaction);
        }

        /// <summary>
        /// 判断企业账户是否存在
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns>结果</returns>
        public bool ExistEnterpriseByAccount(string account)
        {
            var sql = "SELECT COUNT(0) FROM `Enterprise` WHERE Account=@Account;";
            return Connection.Query<int>(sql, new Enterprise { Account = account }, Transaction).SingleOrDefault() > 0;
        }

        /// <summary>
        /// 判断企业名称是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>结果</returns>
        public bool ExistEnterpriseByName(string name)
        {
            var sql = "SELECT COUNT(0) FROM `Enterprise` WHERE Name=@Name;";
            return Connection.Query<int>(sql, new Enterprise { Name = name }, Transaction).SingleOrDefault() > 0;
        }

        /// <summary>
        /// 企业列表分页查询
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>查询结果</returns>
        public Pagination<EnterprisePaginationResponse> GetPagination(EnterpriseFilter filter)
        {
            var sqlCondition = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(filter.KeyWord))
            {
                sqlCondition.Append(" AND Account = @KeyWord");
            }
            var sqlConditionStr = sqlCondition.ToString();
            var condition = string.IsNullOrWhiteSpace(sqlConditionStr) ? string.Empty : sqlConditionStr.Substring(4);
            var sqlCount = $"SELECT COUNT(1) FROM `Enterprise` WHERE {condition};";
            var count = Connection.Query<int>(sqlCount, filter).SingleOrDefault();
            var sqlData = $"SELECT * FROM `Enterprise` WHERE {condition} ORDER BY CreateTime DESC LIMIT @PageIndex, @PageSize;";
            var data = Connection.Query<EnterprisePaginationResponse>(sqlData, filter, Transaction);
            return new Pagination<EnterprisePaginationResponse>
            {
                Count = count,
                Data = data
            };
        }
    }
}