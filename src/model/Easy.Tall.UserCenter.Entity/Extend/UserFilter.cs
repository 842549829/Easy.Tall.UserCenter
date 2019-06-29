using Easy.Tall.UserCenter.Framework.Data;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 用户分页查询条件
    /// </summary>
    public class UserFilter : PageFilter
    {
        /// <summary>
        /// 帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }
    }
}