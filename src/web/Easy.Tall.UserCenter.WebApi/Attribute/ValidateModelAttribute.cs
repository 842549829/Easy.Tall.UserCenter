using System.Linq;
using Easy.Tall.UserCenter.Entity.Extend;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Easy.Tall.UserCenter.WebApi.Attribute
{
    /// <summary>
    /// 注册验证过滤器
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 重写验证
        /// </summary>
        /// <param name="context">当前上下文</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var error = context.ModelState.SelectMany(m => m.Value.Errors).FirstOrDefault();
                context.Result = new BadRequestObjectResult(new Result<string> { Msg = error?.ErrorMessage, Code = 400 });
                //new ObjectResult(new Result<string> { Msg = error?.ErrorMessage, Code = 409 }) { StatusCode = 409 };
            }
        }
    }
}