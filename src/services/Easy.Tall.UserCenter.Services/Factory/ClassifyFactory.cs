using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.Services.Factory
{
    /// <summary>
    /// 分类工厂
    /// </summary>
    public static class ClassifyFactory
    {
        /// <summary>
        /// 创建分类
        /// </summary>
        /// <param name="classifyAddRequest">添加分类信息</param>
        /// <returns>分类</returns>
        public static Classify ToClassify(this ClassifyAddRequest classifyAddRequest)
        {
            return new Classify
            {
               Name = classifyAddRequest.Name,
               Type = classifyAddRequest.Type
            };
        }

        /// <summary>
        /// 创建分类
        /// </summary>
        /// <param name="classifyUpdateRequest">修改分类信息</param>
        /// <returns>分类</returns>
        public static Classify ToClassify(this ClassifyUpdateRequest classifyUpdateRequest)
        {
            return new Classify
            {
                Name = classifyUpdateRequest.Name,
                Id = classifyUpdateRequest.Id
            };
        }
    }
}