using System;
using Easy.Tall.UserCenter.Framework.Data;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 企业分页信息
    /// </summary>
    public class EnterpriseFilter : PageFilter
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 创建开始时间
        /// </summary>
        public DateTime? CreateTimeStart { get; set; }

        /// <summary>
        /// 创建结束时间
        /// </summary>
        public DateTime? CreateTimeEnd { get; set; }
    }
}