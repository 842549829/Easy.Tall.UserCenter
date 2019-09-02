using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 企业信息
    /// </summary>
    public class EnterprisePaginationResponse : BaseEntity
    {
        /// <summary>
        /// 企业账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 全称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string Abbreviation { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 企业法人代表
        /// </summary>
        public string Person { get; set; }

        /// <summary>
        /// 企业类型
        /// </summary>
        public int Type { get; set; }
    }
}