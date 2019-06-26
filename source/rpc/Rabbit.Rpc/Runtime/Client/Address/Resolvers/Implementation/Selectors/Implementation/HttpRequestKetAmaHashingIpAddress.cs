using System;

namespace Rabbit.Rpc.Runtime.Client.Address.Resolvers.Implementation.Selectors.Implementation
{
    /// <summary>
    /// 哈希源地址IP
    /// </summary>
    public class HttpRequestKetAmaHashingIpAddress : IKetAmaHashingIpAddress
    {
        /// <summary>
        /// _ipOptionFunc
        /// </summary>
        private readonly Func<string> _ipOptionFunc;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ipOptionFunc">Ip地址</param>
        public HttpRequestKetAmaHashingIpAddress(Func<string> ipOptionFunc)
        {
            _ipOptionFunc = ipOptionFunc;
        }

        /// <inheritdoc />
        /// <summary>
        /// 获取客户端Ip
        /// </summary>
        /// <returns>Ip地址</returns>
        public string GetClientIp()
        {
            return _ipOptionFunc();
        }
    }
}