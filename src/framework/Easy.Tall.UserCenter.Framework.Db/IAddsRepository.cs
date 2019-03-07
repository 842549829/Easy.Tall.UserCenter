using System;
using System.Collections.Generic;

namespace Easy.Tall.UserCenter.Framework.Db
{
    /// <summary>
    /// IAddsRepository
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public interface IAddsRepository<in T> : IDisposable
    {
        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity">entity</param>
        void Add(IEnumerable<T> entity);
    }
}