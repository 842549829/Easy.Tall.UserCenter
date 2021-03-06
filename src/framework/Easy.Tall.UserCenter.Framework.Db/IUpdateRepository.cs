﻿using System;

namespace Easy.Tall.UserCenter.Framework.Db
{
    /// <summary>
    ///  IUpdateRepository{更新接口}
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public interface IUpdateRepository<in T> : IDisposable
    {
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        void Update(T entity);
    }
}