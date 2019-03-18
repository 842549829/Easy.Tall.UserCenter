using System;
using Easy.Tall.UserCenter.Entity.Interface;

namespace Easy.Tall.UserCenter.Entity.Model
{
    /// <summary>
    /// 基础时间类
    /// </summary>
    public abstract class BaseTimeEntity : BaseIdEntity, IBaseTimeEntity
    {
        /// <inheritdoc />
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        
        /// <inheritdoc />
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; } = DateTime.Now;
    }
}