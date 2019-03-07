using System;

namespace Easy.Tall.UserCenter.Framework.Exceptions
{
    /// <summary>
    /// 业务错误
    /// </summary>
    public class BusinessException : Exception
    {
        /// <summary>
        /// 异常代码
        /// </summary>
        public int Code => HResult;

        /// <summary>
        /// 构造函数，默认Code为1
        /// </summary>
        public BusinessException()
        {
            HResult = 1;
        }

        /// <summary>
        /// 构造函数，默认Code为1
        /// </summary>
        /// <param name="message">错误消息</param>
        public BusinessException(string message) : base(message)
        {
            HResult = 1;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code">错误代码，如果code小于1则code设置为1</param>
        public BusinessException(int code) : base()
        {
            HResult = code < 1 ? 1 : code;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code">错误代码，如果code小于1则code设置为1</param>
        /// <param name="message">错误消息</param>
        public BusinessException(int code, string message) : base(message)
        {
            HResult = code < 1 ? 1 : code;
        }
    }
}