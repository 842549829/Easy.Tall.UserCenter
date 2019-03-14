using System;

namespace Easy.Tall.UserCenter.Framework.Json
{
    /// <summary>
    /// 转换成字符串的类型
    /// </summary>
    [Flags]
    public enum NumberConverterShip
    {
        /// <summary>
        /// 长整数
        /// </summary>
        Int64 = 1,

        /// <summary>
        /// 无符号长整数
        /// </summary>
        UInt64 = 2,

        /// <summary>
        /// 浮点数
        /// </summary>
        Single = 4,

        /// <summary>
        /// 双精度浮点数
        /// </summary>
        Double = 8,

        /// <summary>
        /// 大数字
        /// </summary>
        Decimal = 16
    }
}