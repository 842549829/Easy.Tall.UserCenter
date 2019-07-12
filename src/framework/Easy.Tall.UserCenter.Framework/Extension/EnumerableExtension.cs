using System.Collections.Generic;

namespace Easy.Tall.UserCenter.Framework.Extension
{
    /// <summary>
    /// 集合扩展
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// Add
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="e">e</param>
        /// <param name="item">item</param>
        /// <returns>e</returns>
        public static IEnumerable<T> Add<T>(this IEnumerable<T> e, T item)
        {
            foreach (var cur in e)
            {
                yield return cur;
            }
            yield return item;
        }

        /// <summary>
        /// AddRange
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="e">e</param>
        /// <param name="collection">collection</param>
        /// <returns>e</returns>
        public static IEnumerable<T> AddRange<T>(this IEnumerable<T> e, IEnumerable<T> collection)
        {
            foreach (var cur in e)
            {
                yield return cur;
            }
            foreach (var cur in collection)
            {
                yield return cur;
            }
        }
    }
}