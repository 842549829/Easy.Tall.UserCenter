using Easy.Tall.UserCenter.Entity.Enum;
using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 权限
    /// </summary>
    public class PermissionResponse : BaseIdEntity
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

        /// <summary>
        /// 是否拥有该权限
        /// </summary>
        public bool IsChecked { get; set; }
    }
}