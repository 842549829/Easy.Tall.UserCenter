using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Db;

namespace Easy.Tall.UserCenter.IRepository
{
    /// <summary>
    /// 企业仓储接口
    /// </summary>
    public interface IEnterpriseRepository : IRepository<string, Enterprise>
    {
        /// <summary>
        /// 判断企业账户是否存在
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns>结果</returns>
        bool ExistEnterpriseByAccount(string account);

        /// <summary>
        /// 判断企业名称是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>结果</returns>
        bool ExistEnterpriseByName(string name);
    }
}