namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 用户修改密码
    /// </summary>
    public class UserUpdatePasswordRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }
    }
}