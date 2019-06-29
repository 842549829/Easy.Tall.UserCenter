namespace Easy.Tall.UserCenter.Entity.Model
{
    /// <summary>
    /// 权限
    /// </summary>
    public class Permission : BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分组Id
        /// </summary>
        public string ClassifyId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
    }
}