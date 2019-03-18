using System.Data;
using System.Linq;
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
            string sql = "DELETE FROM `User` WHERE Id=@Id;";
            int result = Connection.Execute(sql, entity);
            if (result < 1)
            {
                throw new BusinessException(1, "删除用户失败");
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        public void Update(User entity)
        {
            string sql = "UPDATE `User` SET = Nickname=@Nickname WHERE Id=@Id;";
            Connection.Execute(sql, entity);
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="key">id</param>
        /// <returns>返回查询单条数据</returns>
        public User Query(string key)
        {
            string sql = "SELECT * FROM `User` WHERE Id=@Id;";
            return Connection.QueryFirst<User>(sql, new User { Id = key });
        }

        /// <summary>
        /// 验证密码是否正确
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <param name="password">密码</param>
        /// <returns>结果</returns>
        public bool ValidatePassword(string id, string password)
        {
            string sql = "SELECT COUNT(0) FROM `User` WHERE Id=@Id AND Password=@Password;";
            return Connection.Query<User>(sql, new User { Id = id, Password = password }).Any();
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
            Connection.Execute(sql, new User { Id = id, Password = password });
        }
    }
}