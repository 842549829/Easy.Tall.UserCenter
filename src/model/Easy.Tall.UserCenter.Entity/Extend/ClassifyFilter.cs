using Easy.Tall.UserCenter.Entity.Enum;
using Easy.Tall.UserCenter.Framework.Data;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 分类分页查询条件
    /// </summary>
    public class ClassifyFilter : PageFilter
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public ClassifyType? Type { get; set; }
    }
}