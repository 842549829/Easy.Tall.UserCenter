using System;
using System.Collections.Generic;
using System.Linq;
using Easy.Tall.UserCenter.Entity.Enum;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Framework.Attribute;
using Easy.Tall.UserCenter.Framework.Constant;
using Easy.Tall.UserCenter.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Easy.Tall.UserCenter.WebApi.Controllers
{
    /// <summary>
    /// Values
    /// </summary>
    [Authorize]
    [Permission("/UserCenter/Values")]
    public class ValuesController : UnAuthController
    {
        /// <summary>
        /// 权限服务
        /// </summary>
        private readonly IPermissionService _permissionService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="permissionService">权限服务</param>
        public ValuesController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        /// <summary>
        /// 查询权限
        /// </summary>
        /// <returns>结果</returns>
        [HttpGet("permissions")]
        public ActionResult<IEnumerable<PermissionResponse>> GetByUserId()
        {
            return Ok(_permissionService.GetPermissionsByUserId(new PermissionFilter { Id = UserId, PermissionClassify = PermissionClassify.UserCenter }));
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId => User.Claims.FirstOrDefault(d => string.Equals(d.Type, AppSettingsSection.Uid, StringComparison.CurrentCultureIgnoreCase))?.Value;
    }
}