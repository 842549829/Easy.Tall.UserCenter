using System;
namespace Rabbit.Rpc.Exceptions
{
    /// <summary>
    ///  RPC服务异常（由服务端转发至客户端的异常信息）。
    /// </summary>
    public class RpcRemoteServiceException : Exception
    {
        /// <summary>
        /// 初始化一个新的Rpc异常实例。
        /// </summary>
        /// <param name="code">错误代码。</param>
        /// <param name="message">异常消息。</param>
        public RpcRemoteServiceException(int code, string message)
            : base(message)
        {
            base.HResult = code;
        }
    }
}