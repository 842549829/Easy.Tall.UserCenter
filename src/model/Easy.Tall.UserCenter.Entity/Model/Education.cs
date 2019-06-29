using System;
using Easy.Tall.UserCenter.Entity.Interface;

namespace Easy.Tall.UserCenter.Entity.Model
{
    /// <summary>
    /// 教育状况
    /// </summary>
    public class Education : BaseEntity, IBaseDateRangeEntity
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
        /// 教育种类
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        /// 教育内容
        /// </summary>
        public string Content { get; set; }
    }
}