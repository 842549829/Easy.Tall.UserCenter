using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Encrypt;

namespace Easy.Tall.UserCenter.Services.Factory
{
    /// <summary>
    /// 用户工厂
    /// </summary>
    public static class UserFactory
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userAddRequest">添加用户信息</param>
        /// <returns>用户</returns>
        public static User ToUser(this UserAddRequest userAddRequest)
        {
            return new User
            {
                IsAdmin = false,
                Nickname = userAddRequest.Nickname,
                Password = MD5Encrypt.Encrypt(userAddRequest.Password).ToUpper(),
                Account = userAddRequest.Account
            };
        }
    }
}