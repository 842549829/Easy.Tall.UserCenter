using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 角色删除
    /// </summary>
    public class RoleRemoveRequest : BaseIdEntity
    {
        /// <summary>
        /// 是否删除关联关系
        /// </summary>
        public bool IsRelation { get; set; }
    }
}