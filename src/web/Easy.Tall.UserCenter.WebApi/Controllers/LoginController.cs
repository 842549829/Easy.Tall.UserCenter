using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Easy.Tall.UserCenter.WebApi.Controllers
{
    /// <summary>
    /// 用户登录
    /// </summary>
    public class LoginController : UnAuthController
    {
        /// <summary>
        /// JwtToken
        /// </summary>
        private readonly JwtTokenValidator _jwtTokenValidator;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jwtTokenValidator">jwtTokenValidator</param>
        public LoginController(JwtTokenValidator jwtTokenValidator)
        {
            _jwtTokenValidator = jwtTokenValidator;
        }

        /// <summary>
        /// 用户登录测试
        /// </summary>
        /// <returns>返回token</returns>
        [HttpGet]
        public ActionResult<Result<string>> Login()
        {
            return Ok(_jwtTokenValidator.GenerateToken("Id"));
        }
    }
}