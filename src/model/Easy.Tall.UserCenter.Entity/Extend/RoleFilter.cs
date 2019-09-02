using Easy.Tall.UserCenter.Framework.Data;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 角色查询条件
    /// </summary>
    public class RoleFilter : PageFilter
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属分组
        /// </summary>
        public string ClassifyId { get; set; }
    }
}