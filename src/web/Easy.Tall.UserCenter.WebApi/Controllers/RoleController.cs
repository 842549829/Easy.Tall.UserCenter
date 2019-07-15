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
        /// 构造函数
        /// </summary>
        /// <param name="roleService">角色服务</param>
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
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
        [HttpPost("page")]
        [Permission("/UserCenter/Role/Query")]
        public ActionResult<Pagination<RolePaginationResponse>> GetPagination([FromBody]RoleFilter roleFilter)
        {
            return Ok(_roleService.GetPagination(roleFilter));
        }
    }
}