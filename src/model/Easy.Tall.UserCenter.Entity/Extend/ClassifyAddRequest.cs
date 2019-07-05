using Easy.Tall.UserCenter.Entity.Enum;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 添加分类实体
    /// </summary>
    public class ClassifyAddRequest
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