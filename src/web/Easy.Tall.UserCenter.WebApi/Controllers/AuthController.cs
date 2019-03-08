using Microsoft.AspNetCore.Authorization;

namespace Easy.Tall.UserCenter.WebApi.Controllers
{
    /// <summary>
    /// 需要授权访问
    /// </summary>
    [Authorize]
    public class AuthController : FormatDataControllerBase
    {
    }
}