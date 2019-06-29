using Easy.Tall.UserCenter.Entity.Enum;
using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 用户分页数据
    /// </summary>
    public class UserPaginationResponse : BaseEntity
    {
        /// <summary>
        /// 帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 用户身份
        /// </summary>
        public IdentityType Identity { get; set; }
    }
}