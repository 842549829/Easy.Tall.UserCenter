using Easy.Tall.UserCenter.Entity.Enum;

namespace Easy.Tall.UserCenter.Entity.Model
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// 帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 移动电话
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// 用户身份
        /// </summary>
        public IdentityType Identity { get; set; }
    }
}