using System.Data;

namespace Easy.Tall.UserCenter.IRepository
{
    /// <summary>
    /// 仓储工厂接口
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        /// 创建仓储
        /// </summary>
        /// <param name="dbConnection">数据库链接</param>
        /// <returns>仓储</returns>
        IRepository CreateRepository(IDbConnection dbConnection);
    }
}
