using Easy.Tall.UserCenter.Entity.Enum;
using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 权限查询条件
    /// </summary>
    public class PermissionFilter : BaseIdEntity
    {
        /// <summary>
        /// 所属分类
        /// </summary>
        public PermissionClassify PermissionClassify { get; set; }
    }
}