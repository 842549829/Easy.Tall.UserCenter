using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Db;

namespace Easy.Tall.UserCenter.IRepository
{
    /// <summary>
    /// 用户仓储接口
    /// </summary>
    public interface IUserRepository : IRepository<string, User>
    {
        /// <summary>
        /// 验证密码是否正确
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <param name="password">密码</param>
        /// <returns>结果</returns>
        bool ValidatePassword(string id, string password);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <param name="password">密码</param>
        /// <returns>结果</returns>
        void UpdatePassword(string id, string password);
    }
}