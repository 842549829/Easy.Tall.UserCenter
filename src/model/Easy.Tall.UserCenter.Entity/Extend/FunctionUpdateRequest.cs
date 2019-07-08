using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 功能权限修改实体
    /// </summary>
    public class FunctionUpdateRequest : BaseIdEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 功能图标
        /// </summary>
        public string Icon { get; set; }

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
    }
}