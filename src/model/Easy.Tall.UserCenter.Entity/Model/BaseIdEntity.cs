using System;
using Easy.Tall.UserCenter.Entity.Interface;

namespace Easy.Tall.UserCenter.Entity.Model
{
    /// <summary>
    /// Id基础类
    /// </summary>
    public abstract class BaseIdEntity : IBaseIdEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}