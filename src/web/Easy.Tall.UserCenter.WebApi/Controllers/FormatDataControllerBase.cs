using Easy.Tall.UserCenter.Framework.Data;
using Microsoft.AspNetCore.Mvc;

namespace Easy.Tall.UserCenter.WebApi.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// 控制基础类
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public abstract class FormatDataControllerBase : ControllerBase
    {
        /// <summary>
        /// Ok
        /// </summary>
        /// <param name="value">value</param>
        /// <returns>OkObjectResult</returns>
        [NonAction]
        public virtual OkObjectResult Ok<T>(T value)
        {
            var obj = CreateResult(0, value);
            return base.Ok(obj);
        }

        /// <summary>
        /// Ok
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="value">value</param>
        /// <returns>OkObjectResult</returns>
        [NonAction]
        public virtual OkObjectResult Ok(int code, string value)
        {
            var obj = CreateResult(code, value);
            return base.Ok(obj);
        }

        /// <summary>
        /// BadRequest
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="msg">msg</param>
        /// <returns>BadRequestObjectResult</returns>
        [NonAction]
        public virtual BadRequestObjectResult BadRequest(int code, string msg)
        {
            var obj = CreateErrorResult<string>(code, msg);
            return base.BadRequest(obj);
        }

        /// <summary>
        /// NotFound
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="msg">msg</param>
        /// <returns>NotFoundObjectResult</returns>
        [NonAction]
        public virtual NotFoundObjectResult NotFound(int code, string msg)
        {
            var obj = CreateErrorResult<string>(code, msg);
            return base.NotFound(obj);
        }

        /// <summary>
        /// ServerException
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="msg">msg</param>
        /// <returns>ObjectResult</returns>
        [NonAction]
        public virtual ObjectResult ServerException(int code, string msg)
        {
            var obj = CreateErrorResult<string>(code, msg);
            return StatusCode(500, obj);
        }

        /// <summary>
        /// UnProcessableEntity
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="msg">msg</param>
        /// <returns>UnProcessableEntityObjectResult</returns>
        [NonAction]
        public virtual UnprocessableEntityObjectResult UnProcessableEntity(int code, string msg)
        {
            var obj = CreateErrorResult<string>(code, msg);
            return base.UnprocessableEntity(obj);
        }

        /// <summary>
        /// Conflict
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="msg">msg</param>
        /// <returns>ConflictObjectResult</returns>
        [NonAction]
        public virtual ConflictObjectResult Conflict(int code, string msg)
        {
            var obj = CreateErrorResult<string>(code, msg);
            return base.Conflict(obj);
        }

        /// <summary>
        /// CreateAPIResult
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="code">code</param>
        /// <param name="value">value</param>
        /// <returns>APIResult</returns>
        [NonAction]
        public virtual Result<T> CreateResult<T>(int code, T value)
        {
            return new Result<T>
            {
                Code = code,
                Data = value
            };
        }

        /// <summary>
        /// CreateErrorResult
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="msg">msg</param>
        /// <returns>APIResult</returns>
        [NonAction]
        public virtual Result<T> CreateErrorResult<T>(int code, string msg)
        {
            return new Result<T>
            {
                Code = code,
                Msg = msg
            };
        }
    }
}