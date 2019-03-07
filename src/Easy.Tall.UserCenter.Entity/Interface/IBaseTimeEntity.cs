using System;

namespace Easy.Tall.UserCenter.Entity.Interface
{
    /// <summary>
    /// 基础时间接口
    /// </summary>
    public interface IBaseTimeEntity
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        DateTime ModifyTime { get; set; }
    }
}