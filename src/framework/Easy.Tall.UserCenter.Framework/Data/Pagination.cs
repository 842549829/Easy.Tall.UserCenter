using System.Collections.Generic;

namespace Easy.Tall.UserCenter.Framework.Data
{
    /// <summary>
    /// 分页数据
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class Pagination<T>
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public IEnumerable<T> Data { get; set; }
    }
}