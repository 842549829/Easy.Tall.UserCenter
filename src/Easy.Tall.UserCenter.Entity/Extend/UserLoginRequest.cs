using System;
using System.ComponentModel.DataAnnotations;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 用户登录请求参数
    /// </summary>
    public class UserLoginRequest
    {
        /// <summary>
        /// 帐号
        /// </summary>
        [Required(ErrorMessage = "登录帐号不允许为空")]
        [StringLength(20, ErrorMessage = "字段帐户必须是一个最大长度为20的字符串")]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不允许为空")]
        [StringLength(36, ErrorMessage = "密码帐户必须是一个最大长度为36的字符串")]
        public string Password { get; set; }
    }
}