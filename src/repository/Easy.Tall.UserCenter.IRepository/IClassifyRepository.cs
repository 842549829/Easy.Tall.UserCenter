using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Data;
using Easy.Tall.UserCenter.Framework.Db;

namespace Easy.Tall.UserCenter.IRepository
{
    /// <summary>
    /// 分类仓储接口
    /// </summary>
    public interface IClassifyRepository : IRepository<string, Classify>
    {
        /// <summary>
        /// 分类分页查询
        /// </summary>
        /// <param name="classifyFilter">分类查询条件</param>
        /// <returns>查询数据</returns>
        Pagination<ClassifyPaginationResponse> GetPagination(ClassifyFilter classifyFilter);
    }
}