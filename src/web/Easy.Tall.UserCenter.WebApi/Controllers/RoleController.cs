using System.Collections.Generic;
using Easy.Tall.UserCenter.Entity.Enum;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Framework.Attribute;
using Easy.Tall.UserCenter.Framework.Data;
using Easy.Tall.UserCenter.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Easy.Tall.UserCenter.WebApi.Controllers
{
    /// <summary>
    /// 角色控制器
    /// </summary>
    [Permission("/UserCenter/Role")]
    public class RoleController : AuthController
    {
        /// <summary>
        /// 角色服务
        /// </summary>
        private readonly IRoleService _roleService;

        /// <summary>
        /// 权限服务
        /// </summary>
        private readonly IPermissionService _permissionService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="roleService">角色服务</param>
        /// <param name="permissionService">角色权限</param>
        public RoleController(IRoleService roleService,
            IPermissionService permissionService)
        {
            _roleService = roleService;
            _permissionService = permissionService;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="roleAddRequest">添加信息</param>
        /// <returns>结果</returns>
        [HttpPost]
        [Permission("/UserCenter/Role/Add")]
        public ActionResult<Result<bool>> Add([FromBody] RoleAddRequest roleAddRequest)
        {
            return Ok(_roleService.Add(roleAddRequest));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="roleRemoveRequest">删除信息</param>
        /// <returns>结果</returns>
        [HttpDelete]
        [Permission("/UserCenter/Role/Remove")]
        public ActionResult<Result<bool>> Remove([FromBody]RoleRemoveRequest roleRemoveRequest)
        {
            return Ok(_roleService.Remove(roleRemoveRequest));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="roleUpdateRequest">修改信息</param>
        /// <returns>结果</returns>
        [HttpPut]
        [Permission("/UserCenter/Role/Update")]
        public ActionResult<Result<bool>> Update([FromBody]RoleUpdateRequest roleUpdateRequest)
        {
            return Ok(_roleService.Update(roleUpdateRequest));
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="roleFilter">查询条件</param>
        /// <returns>数据</returns>
        [HttpPost("/page")]
        [Permission("/UserCenter/Role/Query")]
        public ActionResult<Pagination<RolePaginationResponse>> GetPagination([FromBody]RoleFilter roleFilter)
        {
            return Ok(_roleService.GetPagination(roleFilter));
        }

        /// <summary>
        /// 查询角色分组
        /// </summary>
        /// <returns>结果</returns>
        [HttpGet("group")]
        [Permission("/UserCenter/Permission/Query")]
        public ActionResult<IEnumerable<RoleGroupByResponse>> GetRoleGroupByResponses()
        {
            return Ok(_roleService.GetRoleGroupByResponses());
        }

        /// <summary>
        /// 查询权限
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns>结果</returns>
        [HttpGet("roleId")]
        [Permission("/UserCenter/Permission/Query")]
        public ActionResult<Result<bool>> Get(string roleId)
        {
            return Ok(_permissionService.GetPermissionsByRoleId(new PermissionFilter
            {
                Id = roleId,
                PermissionClassify = PermissionClassify.UserCenter
            }));
        }

        /// <summary>
        /// 编辑权限
        /// </summary>
        /// <param name="permissionEditRequest">编辑信息</param>
        /// <returns>结果</returns>
        [HttpPut("edit")]
        [Permission("/UserCenter/Permission/Edit")]
        public ActionResult<Result<bool>> EditPermission([FromBody]PermissionEditRequest permissionEditRequest)
        {
            return Ok(_permissionService.Edit(permissionEditRequest));
        }
    }
}