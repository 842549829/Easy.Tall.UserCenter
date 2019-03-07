namespace Easy.Tall.UserCenter.Entity.Model
{
    /// <summary>
    /// 社会关系
    /// </summary>
    public class SocialRelation : BaseEntity
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 关系
        /// </summary>
        public string Relation { get; set; }

        /// <summary>
        /// 工作性质
        /// </summary>
        public string Job { get; set; }

        /// <summary>
        /// 联系手机号
        /// </summary>
        public string Mobile { get; set; }
    }
}