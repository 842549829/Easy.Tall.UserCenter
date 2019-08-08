using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Framework.Data;
using Easy.Tall.UserCenter.IServices;
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
        /// 用户服务
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jwtTokenValidator">jwtTokenValidator</param>
        /// <param name="userService">用户服务</param>
        public LoginController(JwtTokenValidator jwtTokenValidator, IUserService userService)
        {
            _jwtTokenValidator = jwtTokenValidator;
            _userService = userService;
        }

        /// <summary>
        /// 用户登录测试
        /// </summary>
        /// <returns>返回token</returns>
        [HttpPost]
        public ActionResult<ApiResult<string>> Login(UserLoginRequest userLoginRequest)
        {
            var data = _userService.Login(userLoginRequest);
            if (data.Code != 0)
            {
                return Ok(data.Code, data.Msg);
            }
            return Ok(_jwtTokenValidator.GenerateToken(data.Data.Id));
        }
    }
}