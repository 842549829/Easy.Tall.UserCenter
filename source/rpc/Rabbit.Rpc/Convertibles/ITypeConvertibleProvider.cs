using System;
using System.Collections.Generic;

namespace Rabbit.Rpc.Convertibles
{
    /// <summary>
    /// 类型转换委托
    /// </summary>
    /// <param name="instance">需要转化的实例</param>
    /// <param name="conversionType">转换类型</param>
    /// <param name="result">转换后的实例</param>
    /// <returns>转换之后的类型，如果无法转换则返回null</returns>
    public delegate bool TypeConvertDelegate(object instance, Type conversionType, out object result);

    /// <summary>
    /// 一个抽象的类型转换提供程序
    /// </summary>
    public interface ITypeConvertibleProvider
    {
        /// <summary>
        /// 获取类型转换器
        /// </summary>
        /// <returns>类型转换器集合</returns>
        IEnumerator<TypeConvertDelegate> GetConverters();
    }
}