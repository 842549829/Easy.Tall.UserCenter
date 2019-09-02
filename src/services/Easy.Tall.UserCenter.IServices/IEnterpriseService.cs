using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Framework.Data;

namespace Easy.Tall.UserCenter.IServices
{
    /// <summary>
    /// 企业
    /// </summary>
    public interface IEnterpriseService
    {
        /// <summary>
        /// 添加企业
        /// </summary>
        /// <param name="request">企业信息</param>
        /// <returns>添加结果</returns>
        Result<bool> Add(EnterpriseAddRequest request);

        /// <summary>
        /// 企业信息分页查询
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>结果</returns>
        Pagination<EnterprisePaginationResponse> GetPagination(EnterpriseFilter filter);
    }
}