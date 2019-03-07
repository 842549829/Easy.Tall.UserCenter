using System.Data;

namespace Easy.Tall.UserCenter.Framework.Db
{
    /// <summary>
    /// 创建数据库链接
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// 根据链接字符串创建数据库链接
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IDbConnection CreateDbConnection(string name);
    }
}