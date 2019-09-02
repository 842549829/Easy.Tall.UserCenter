using Easy.Tall.UserCenter.Entity.Enum;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Constant;
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
                Identity = IdentityType.Normal,
                Nickname = userAddRequest.Nickname,
                Password = MD5Encrypt.Encrypt(userAddRequest.Password).ToUpper(),
                Account = userAddRequest.Account
            };
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="request">添加用户信息</param>
        /// <returns>用户</returns>
        public static User ToUser(this EnterpriseAddRequest request)
        {
            return new User
            {
                Account = request.Account,
                Password = MD5Encrypt.Encrypt(AppSettingsSection.DefaultPassword).ToUpper(),
                Mail = request.Mail,
                Mobile = request.ContactMobile,
                Nickname = request.Name,
                Identity = IdentityType.Admin
            };
        }
    }
}