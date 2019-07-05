using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Framework.Data;

namespace Easy.Tall.UserCenter.IServices
{
    /// <summary>
    /// 分类
    /// </summary>
    public interface IClassifyService
    {
        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="classifyAddRequest">分类信息</param>
        /// <returns>结果</returns>
        Result<bool> Add(ClassifyAddRequest classifyAddRequest);

        /// <summary>
        /// 根据主键Id删除分类
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>结果</returns>
        Result<bool> Remove(string id);

        /// <summary>
        /// 修改分类
        /// </summary>
        /// <param name="classifyUpdateRequest">分类信息</param>
        /// <returns>结果</returns>
        Result<bool> Update(ClassifyUpdateRequest classifyUpdateRequest);

        /// <summary>
        /// 分类分页
        /// </summary>
        /// <param name="classifyFilter">查询条件</param>
        /// <returns>数据</returns>
        Pagination<ClassifyPaginationResponse> GetPagination(ClassifyFilter classifyFilter);
    }
}