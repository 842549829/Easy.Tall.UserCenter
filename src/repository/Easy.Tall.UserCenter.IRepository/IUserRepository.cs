using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Data;
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

        /// <summary>
        /// 根据账号查询用户
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns>用户信息</returns>
        User QueryUserByAccount(string account);

        /// <summary>
        /// 用户分页查询
        /// </summary>
        /// <param name="userFilter">用户查询条件</param>
        /// <returns>查询数据</returns>
        Pagination<UserPaginationResponse> GetPagination(UserFilter userFilter);

        /// <summary>
        /// 修改用户身份类型
        /// </summary>
        /// <param name="user">用户信息</param>
        void UpdateIdentityType(User user);
    }
}