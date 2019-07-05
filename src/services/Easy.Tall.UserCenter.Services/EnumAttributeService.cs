using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Easy.Tall.UserCenter.IServices;

namespace Easy.Tall.UserCenter.Services
{
    /// <summary>
    /// 获取枚举信息
    /// </summary>
    public class EnumAttributeService: IEnumAttributeService
    {
        /// <summary>
        /// 获取枚举字典
        /// </summary>
        /// <returns>枚举描述字典</returns>
        public IDictionary<int, string> GetDictionary<T>() where T : Enum
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new Exception($"类型 {type.FullName} 不支持此操作");
            }
            var values = Enum.GetValues(type);
            var dict = new Dictionary<int, string>(values.Length);
            foreach (var value in values)
            {
                dict[GetValue((Enum)value)] = GetDisplayName((Enum)value);
            }
            return dict;
        }

        /// <summary>
        /// 获取描述名称
        /// </summary>
        /// <param name="obj">枚举</param>
        /// <returns>描述</returns>
        public static string GetDisplayName(Enum obj)
        {
            var type = obj.GetType();
            var name = Enum.GetName(type, obj);
            var fieldInfo = type.GetField(name);
            if (fieldInfo == null)
            {
                return name;
            }
            var description = fieldInfo.GetCustomAttribute<DescriptionAttribute>(true);
            if (description != null)
            {
                return description.Description;
            }
            var display = fieldInfo.GetCustomAttribute<DisplayNameAttribute>(true);
            if (display != null)
            {
                return display.DisplayName;
            }
            return name;
        }

        /// <summary>
        /// 将枚举转换为数字
        /// </summary>
        /// <param name="obj">枚举</param>
        /// <returns>值</returns>
        public static int GetValue(Enum obj)
        {
            return Convert.ToInt32(obj);
        }
    }
}