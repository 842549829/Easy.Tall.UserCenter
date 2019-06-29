using Easy.Tall.UserCenter.Entity.Enum;

namespace Easy.Tall.UserCenter.Entity.Model
{
    /// <summary>
    /// 功能
    /// </summary>
    public class Function : BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 上级功能Id
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 功能类型
        /// </summary>
        public FunctionType Type { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}