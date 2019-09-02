using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Framework.Attribute;
using Easy.Tall.UserCenter.Framework.Data;
using Easy.Tall.UserCenter.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Easy.Tall.UserCenter.WebApi.Controllers
{
    /// <summary>
    /// 企业控制器
    /// </summary>
    [Permission("/UserCenter/Enterprise")]
    public class EnterpriseController : AuthController
    {
        /// <summary>
        /// 企业服务
        /// </summary>
        private readonly IEnterpriseService _enterpriseService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="enterpriseService">企业服务</param>
        public EnterpriseController(IEnterpriseService enterpriseService)
        {
            _enterpriseService = enterpriseService;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>数据</returns>
        [HttpGet]
        [Permission("/UserCenter/Enterprise/Query")]
        public ActionResult<Pagination<EnterprisePaginationResponse>> GetPagination([FromQuery]EnterpriseFilter filter)
        {
            return Ok(_enterpriseService.GetPagination(filter));
        }

        /// <summary>
        /// 添加企业
        /// </summary>
        /// <param name="request">请求信息</param>
        /// <returns>结果</returns>
        [HttpPost]
        [Permission("/UserCenter/Enterprise/Add")]
        public ActionResult<Result<bool>> Add([FromBody]EnterpriseAddRequest request)
        {
            return Ok(_enterpriseService.Add(request));
        }
    }
}