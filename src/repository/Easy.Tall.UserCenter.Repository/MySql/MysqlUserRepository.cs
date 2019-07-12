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
    /// 用户仓储接口
    /// </summary>
    public class MySqlUserRepository : BaseRepository, IUserRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unit">工作单元</param>
        public MySqlUserRepository(IUnitOfWork unit) : base(unit)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbConnection">数据库连接字符串</param>
        public MySqlUserRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        public void Add(User entity)
        {
            var sql = "INSERT INTO `User` (Id,CreateTime,ModifyTime,Account,Password,Nickname,Mobile,Mail,Identity) VALUES(@Id,@CreateTime,@ModifyTime,@Account,@Password,@Nickname,@Mobile,@Mail,@Identity);";
            var result = Connection.Execute(sql, entity, Transaction);
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
            string sql = "DELETE FROM `User` WHERE Id=@Id;";
            int result = Connection.Execute(sql, entity, Transaction);
            if (result < 1)
            {
                throw new BusinessException(1, "删除用户失败");
            }
        }

        /// <summary>
        /// 修改昵称
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        public void Update(User entity)
        {
            string sql = "UPDATE `User` SET = Nickname=@Nickname WHERE Id=@Id;";
            Connection.Execute(sql, entity, Transaction);
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="key">id</param>
        /// <returns>返回查询单条数据</returns>
        public User Query(string key)
        {
            string sql = "SELECT * FROM `User` WHERE Id=@Id;";
            return Connection.QueryFirst<User>(sql, new User { Id = key }, Transaction);
        }

        /// <summary>
        /// 验证密码是否正确
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <param name="password">密码</param>
        /// <returns>结果</returns>
        public bool ValidatePassword(string id, string password)
        {
            var sql = "SELECT COUNT(0) FROM `User` WHERE Id=@Id AND Password=@Password;";
            return Connection.Query<int>(sql, new User { Id = id, Password = password }, Transaction).SingleOrDefault() > 0;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <param name="password">密码</param>
        /// <returns>结果</returns>
        public void UpdatePassword(string id, string password)
        {
            string sql = "UPDATE `User` SET = Password=@Password WHERE Id=@Id;";
            Connection.Execute(sql, new User { Id = id, Password = password }, Transaction);
        }

        /// <summary>
        /// 用户分页查询
        /// </summary>
        /// <param name="userFilter">用户查询条件</param>
        /// <returns>查询数据</returns>
        public Pagination<UserPaginationResponse> GetPagination(UserFilter userFilter)
        {
            var sqlCondition = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(userFilter.Account))
            {
                sqlCondition.Append(" AND Account = @Account");
            }
            if (!string.IsNullOrWhiteSpace(userFilter.Nickname))
            {
                sqlCondition.Append(" AND Nickname = @Nickname");
            }
            var sqlConditionStr = sqlCondition.ToString();
            var condition = string.IsNullOrWhiteSpace(sqlConditionStr) ? string.Empty : sqlConditionStr.Substring(4);
            var sqlCount = $"SELECT COUNT(1) FROM `User` WHERE {condition};";
            var count = Connection.Query<int>(sqlCount, userFilter).SingleOrDefault();
            var sqlData = $"SELECT * FROM `User` WHERE {condition} ORDER BY Identity DESC, CreateTime DESC LIMIT @PageIndex, @PageSize;";
            var data = Connection.Query<UserPaginationResponse>(sqlData, userFilter, Transaction);
            return new Pagination<UserPaginationResponse>
            {
                Count = count,
                Data = data
            };
        }
    }
}