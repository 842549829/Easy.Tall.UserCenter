using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 权限删除
    /// </summary>
    public class PermissionRemoveRequest : BaseIdEntity
    {
        /// <summary>
        /// 是否删除子节点
        /// </summary>
        public bool IsChildNodes { get; set; }
    }
}