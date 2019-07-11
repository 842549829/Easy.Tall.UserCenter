using Easy.Tall.UserCenter.Entity.Enum;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 权限查询条件
    /// </summary>
    public class PermissionFilter
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 所属分类
        /// </summary>
        public PermissionClassify PermissionClassify { get; set; }
    }
}