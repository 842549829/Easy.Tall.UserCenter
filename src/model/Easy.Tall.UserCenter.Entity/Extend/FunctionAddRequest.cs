using Easy.Tall.UserCenter.Entity.Enum;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 添加功能实体
    /// </summary>
    public class FunctionAddRequest
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
        /// 上级功能图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public FunctionType Type { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 功能路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 标记
        /// </summary>
        public long Flag { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public FunctionClassify Classify { get; set; }
    }
}