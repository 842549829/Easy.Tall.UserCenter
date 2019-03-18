namespace Easy.Tall.UserCenter.Entity.Model
{
    /// <summary>
    /// 企业
    /// </summary>
    public class Enterprise: BaseEntity
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
        /// 组织机构代码
        /// </summary>
        public string OrganizingCode { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 联系人手机
        /// </summary>
        public string ContactMobile { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 企业法人代表
        /// </summary>
        public string Person { get; set; }

        /// <summary>
        /// 企业类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 上级企业Id
        /// </summary>
        public string ParentId { get; set; }
    }
}