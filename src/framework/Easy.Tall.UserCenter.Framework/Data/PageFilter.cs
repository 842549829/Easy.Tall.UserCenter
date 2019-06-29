using System.Collections.Generic;

namespace Easy.Tall.UserCenter.Framework.Data
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class PageFilter
    {
        /// <summary>
        /// 页码(默认第一页)
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 页大小(默认10页)
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}