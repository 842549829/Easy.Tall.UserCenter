﻿namespace Easy.Tall.UserCenter.Framework.Data
{
    /// <summary>
    /// 结果
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }

    /// <summary>
    /// 结果
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 异常消息
        /// </summary>
        public string Msg { get; set; }
    }
}