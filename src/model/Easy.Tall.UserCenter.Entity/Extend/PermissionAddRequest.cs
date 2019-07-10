using Easy.Tall.UserCenter.Entity.Enum;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 添加权限实体
    /// </summary>
    public class PermissionAddRequest
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 上级Id
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 上级图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public PermissionType Type { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 路径
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
        public PermissionClassify Classify { get; set; }
    }
}