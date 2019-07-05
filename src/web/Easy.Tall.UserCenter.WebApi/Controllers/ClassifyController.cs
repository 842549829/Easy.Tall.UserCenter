using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Framework.Attribute;
using Easy.Tall.UserCenter.Framework.Data;
using Easy.Tall.UserCenter.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Easy.Tall.UserCenter.WebApi.Controllers
{
    /// <summary>
    /// 分类控制器
    /// </summary>
    public class ClassifyController : AuthController
    {
        /// <summary>
        /// 分类服务
        /// </summary>
        private readonly IClassifyService _classifyService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="classifyService">分类服务</param>
        public ClassifyController(IClassifyService classifyService)
        {
            _classifyService = classifyService;
        }

        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="classifyAddRequest">分类信息</param>
        /// <returns>结果</returns>
        [HttpPost]
        [Permission("/UserCenter/Classify/Add")]
        public ActionResult<Result<bool>> Add([FromBody] ClassifyAddRequest classifyAddRequest)
        {
            return Ok(_classifyService.Add(classifyAddRequest));
        }

        /// <summary>
        /// 根据主键Id修改分类
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>结果</returns>
        [HttpDelete("id")]
        [Permission("/UserCenter/Classify/Remove")]
        public ActionResult<Result<bool>> Remove(string id)
        {
            return Ok(_classifyService.Remove(id));
        }

        /// <summary>
        /// 修改分类
        /// </summary>
        /// <param name="classifyUpdateRequest">分类信息</param>
        /// <returns>结果</returns>
        [HttpPut]
        [Permission("/UserCenter/Classify/Update")]
        public ActionResult<Result<bool>> Update([FromBody] ClassifyUpdateRequest classifyUpdateRequest)
        {
            return Ok(_classifyService.Update(classifyUpdateRequest));
        }

        /// <summary>
        /// 分类分页
        /// </summary>
        /// <param name="classifyFilter">查询条件</param>
        /// <returns>数据</returns>
        [HttpPost("page")]
        [Permission("/UserCenter/Classify/Query")]
        public ActionResult<Pagination<ClassifyPaginationResponse>> GetPagination([FromBody] ClassifyFilter classifyFilter)
        {
            return Ok(_classifyService.GetPagination(classifyFilter));
        }
    }
}