using System;

namespace Easy.Tall.UserCenter.Framework.Db
{
    /// <summary>
    /// IQueryRepository{根据主键查询接口}.
    /// </summary>
    /// <typeparam name="TKey">Tkey</typeparam>
    /// <typeparam name="TValue">实体</typeparam>
    public interface IQueryRepository<in TKey, out TValue> : IDisposable
    {
        /// <summary>
        /// Query
        /// </summary>
        /// <param name="key">id</param>
        /// <returns>返回查询单条数据</returns>
        TValue Query(TKey key);
    }
}