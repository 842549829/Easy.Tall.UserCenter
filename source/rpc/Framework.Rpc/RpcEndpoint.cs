using System;
using System.Net;

namespace Framework.Rpc
{
    /// <summary>
    /// Consul终端
    /// </summary>
    public class RpcEndpoint
    {
        /// <summary>
        /// 协议{http}
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 创建IPEndPoint
        /// </summary>
        /// <returns>IPEndPoint</returns>
        public IPEndPoint ToIPEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse(Address), Port);
        }

        /// <summary>
        /// 拼接成url地址
        /// </summary>
        /// <returns>url</returns>
        public Uri ToUri()
        {
            return new Uri($"{Schema}{Address}:{Port}");
        }
    }
}