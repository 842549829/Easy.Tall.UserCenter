using System.Data;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Easy.Tall.UserCenter.Framework.Db
{
    /// <summary>
    /// 创建数据库链接
    /// </summary>
    public class DbConnectionFactory: IDbConnectionFactory
    {
        /// <summary>
        /// 配置文件
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration">配置文件</param>
        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 根据链接字符串创建数据库链接
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IDbConnection CreateDbConnection(string name)
        {
            var connectionString = _configuration[$"ConnectionStrings:{name}:ConnectionString"];
            var providerName = _configuration[$"ConnectionStrings:{name}:ProviderName"];
            IDbConnection conn;
            switch (providerName)
            {
                case "Mysql.Data.MySqlClient":
                    conn = new MySqlConnection(connectionString);
                    break;
                default:
                    conn = new MySqlConnection(connectionString);
                    break;
            }
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            return conn;
        }
    }
}