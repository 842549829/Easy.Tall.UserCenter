using Easy.Tall.UserCenter.Entity.Enum;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 分类分页查询
    /// </summary>
    public class ClassifyPaginationResponse : ClassifyResponse
    {
        /// <summary>
        /// 类型
        /// </summary>
        public ClassifyType Type { get; set; }
    }
}