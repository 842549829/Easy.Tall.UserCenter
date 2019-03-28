using System;
using System.Collections.Generic;
using System.Reflection;
using Rabbit.Rpc.Serialization;

namespace Rabbit.Rpc.Convertibles.Implementation
{
    /// <summary>
    /// 一个默认的类型转换提供程序
    /// </summary>
    public class DefaultTypeConvertibleProvider : ITypeConvertibleProvider
    {
        /// <summary>
        /// 序列化器
        /// </summary>
        private readonly ISerializer<object> _serializer;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serializer">序列化器</param>
        public DefaultTypeConvertibleProvider(ISerializer<object> serializer)
        {
            _serializer = serializer;
        }

        /// <summary>
        /// 获取类型转换器
        /// </summary>
        /// <returns>类型转换器集合</returns>
        public IEnumerable<TypeConvertDelegate> GetConverters()
        {
            yield return EnumTypeConvert;
            yield return SimpleTypeConvert;
            yield return ComplexTypeConvert;
        }

        /// <summary>
        /// 枚举转化器
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="conversionType">转化类型</param>
        /// <param name="result">转化结果</param>
        /// <returns>转化是否成功</returns>
        private static bool EnumTypeConvert(object instance, Type conversionType, out object result)
        {
            result = null;
            if (instance == null || !conversionType.GetTypeInfo().IsEnum)
            {
                return false;
            }
            result = Enum.Parse(conversionType, instance.ToString());
            return true;
        }

        /// <summary>
        /// 简单转化器
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="conversionType">转化类型</param>
        /// <param name="result">转化结果</param>
        /// <returns>转化是否成功</returns>
        private static bool SimpleTypeConvert(object instance, Type conversionType, out object result)
        {
            result = null;
            if (instance is IConvertible && typeof(IConvertible).GetTypeInfo().IsAssignableFrom(conversionType))
            {
                result = Convert.ChangeType(instance, conversionType);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 复杂转化器
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="conversionType">转换类型</param>
        /// <param name="result">转化结果</param>
        /// <returns>转化是否成功</returns>
        private bool ComplexTypeConvert(object instance, Type conversionType, out object result)
        {
            result = null;
            // 引用类型
            if (instance == null && !conversionType.IsValueType)
            {
                return true;
            }
            //可空值类型
            if (Nullable.GetUnderlyingType(conversionType) != null)
            {
                return true;
            }
            result = _serializer.Deserialize(instance, conversionType);
            return true;
        }
    }
}