using System;

namespace Easy.Tall.UserCenter.Entity.Interface
{
    /// <summary>
    /// 日期范围
    /// </summary>
    public interface IBaseDateRangeEntity
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        DateTime StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        DateTime EndDate { get; set; }
    }
}