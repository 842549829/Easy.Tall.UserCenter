using System;
using System.Collections.Generic;

namespace Easy.Tall.UserCenter.IServices
{
    /// <summary>
    /// 获取枚举信息
    /// </summary>
    public interface IEnumAttributeService
    {
        /// <summary>
        /// 获取枚举字典
        /// </summary>
        /// <returns>枚举描述字典</returns>
        IDictionary<int, string> GetDictionary<T>() where T : Enum;
    }
}