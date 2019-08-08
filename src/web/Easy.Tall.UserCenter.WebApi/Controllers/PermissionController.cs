using System.Collections.Generic;
using Easy.Tall.UserCenter.Entity.Enum;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Attribute;
using Easy.Tall.UserCenter.Framework.Data;
using Easy.Tall.UserCenter.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Easy.Tall.UserCenter.WebApi.Controllers
{
    /// <summary>
    /// 权限
    /// </summary>
    [Permission("/UserCenter/Permission")]
    public class PermissionController : AuthController
    {
        /// <summary>
        /// 权限服务
        /// </summary>
        private readonly IPermissionService _permissionService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="permissionService">权限服务</param>
        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="permissionAddRequest">添加信息</param>
        /// <returns>结果</returns>
        [HttpPost]
        [Permission("/UserCenter/Permission/Add")]
        public ActionResult<ApiResult<bool>> Add([FromBody] PermissionAddRequest permissionAddRequest)
        {
            return Ok(_permissionService.Add(permissionAddRequest));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="permissionRemoveRequest">删除信息</param>
        /// <returns>结果</returns>
        [HttpDelete]
        [Permission("/UserCenter/Permission/Remove")]
        public ActionResult<ApiResult<bool>> Remove([FromBody]PermissionRemoveRequest permissionRemoveRequest)
        {
            return Ok(_permissionService.Remove(permissionRemoveRequest));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="permissionUpdateRequest">修改信息</param>
        /// <returns>结果</returns>
        [HttpPut]
        [Permission("/UserCenter/Permission/Update")]
        public ActionResult<ApiResult<bool>> Update([FromBody]PermissionUpdateRequest permissionUpdateRequest)
        {
            return Ok(_permissionService.Update(permissionUpdateRequest));
        }

        /// <summary>
        /// 查询权限
        /// </summary>
        /// <returns>结果</returns>
        [HttpGet]
        [Permission("/UserCenter/Permission/Query")]
        public ActionResult<IEnumerable<Permission>> Get()
        {
            return Ok(_permissionService.GetPermissions(PermissionClassify.UserCenter));
        }

        /// <summary>
        /// 查询权限
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns>结果</returns>
        [HttpGet("{roleId}")]
        [Permission("/UserCenter/Permission/Query")]
        public ActionResult<IEnumerable<PermissionResponse>> Get(string roleId)
        {
            return Ok(_permissionService.GetPermissionsByRoleId(new PermissionFilter { Id = roleId, PermissionClassify = PermissionClassify.UserCenter }));
        }

        /// <summary>
        /// 编辑权限
        /// </summary>
        /// <param name="permissionEditRequest">编辑信息</param>
        /// <returns>结果</returns>
        [HttpPut("edit")]
        [Permission("/UserCenter/Permission/Edit")]
        public ActionResult<ApiResult<bool>> Edit([FromBody]PermissionEditRequest permissionEditRequest)
        {
            return Ok(_permissionService.Edit(permissionEditRequest));
        }
    }
}