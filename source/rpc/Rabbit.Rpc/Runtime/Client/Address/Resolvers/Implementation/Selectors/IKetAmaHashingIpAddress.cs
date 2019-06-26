using System;
namespace Rabbit.Rpc.Runtime.Client.Address.Resolvers.Implementation.Selectors
{
    /// <summary>
    /// 哈希源地址
    /// </summary>
    public interface IKetAmaHashingIpAddress
    {
        /// <summary>
        /// 获取客户端Ip
        /// </summary>
        /// <returns>Ip地址</returns>
        string GetClientIp();
    }
}