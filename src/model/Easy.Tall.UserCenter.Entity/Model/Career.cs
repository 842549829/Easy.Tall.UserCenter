using System;
using Easy.Tall.UserCenter.Entity.Interface;

namespace Easy.Tall.UserCenter.Entity.Model
{
    /// <summary>
    /// 职业生涯
    /// </summary>
    public class Career : BaseEntity, IBaseDateRangeEntity
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 工作职位
        /// </summary>
        public string Jobs { get; set; }

        /// <summary>
        /// 工作内容
        /// </summary>
        public string Content { get; set; }
    }
}