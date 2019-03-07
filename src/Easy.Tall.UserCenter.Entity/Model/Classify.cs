using Easy.Tall.UserCenter.Entity.Enum;

namespace Easy.Tall.UserCenter.Entity.Model
{
    /// <summary>
    /// 分类
    /// </summary>
    public class Classify : BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public ClassifyType Type { get; set; }
    }
}