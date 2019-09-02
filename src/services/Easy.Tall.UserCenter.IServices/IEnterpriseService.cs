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
    }
}