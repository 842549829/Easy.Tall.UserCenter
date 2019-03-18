namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 添加用户请求实体
    /// </summary>
    public class UserAddRequest
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
    }
}