using Easy.Tall.UserCenter.Entity.Enum;

namespace Easy.Tall.UserCenter.Entity.Model
{
    /// <summary>
    /// 部门
    /// </summary>
    public class Department : BaseEntity
    {
        /// <summary>
        /// 上级部门Id
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属企业Id
        /// </summary>
        public string EnterpriseId { get; set; }

        /// <summary>
        /// 部门类型
        /// </summary>
        public DepartmentType Type { get; set; }
    }
}