using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Rabbit.Rpc.Runtime.Client.Address.Resolvers.Implementation.Selectors.Implementation
{
    /// <inheritdoc />
    /// <summary>
    /// 哈希源地址IP
    /// </summary>
    public class DefaultKetAmaHashingIpAddress : IKetAmaHashingIpAddress
    {
        /// <inheritdoc />
        /// <summary>
        /// 获取客户端Ip
        /// </summary>
        /// <returns>Ip地址</returns>
        public string GetClientIp()
        {
            List<string> ips = new List<string>();
            //得到本机名 
            string hostname = Dns.GetHostName();
            //解析主机名称或IP地址的system.net.iphostentry实例。
            IPHostEntry localhost = Dns.GetHostEntry(hostname);
            foreach (IPAddress item in localhost.AddressList)
            {
                //判断是否是内网IPv4地址
                if (item.AddressFamily == AddressFamily.InterNetwork)
                {
                    ips.Add(item.MapToIPv4().ToString());
                }
            }
            return string.Join(string.Empty, ips);
        }
    }
}