using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Options;

namespace Framework.Rpc
{
    /// <summary>
    /// Consul配置信息提供接口
    /// </summary>
    public class ServiceDiscoveryOptionProvider : IServiceDiscoveryOptionProvider
    {
        /// <summary>
        /// 端口号(静态表示全局唯一)
        /// </summary>
        private static int _port;

        /// <summary>
        /// 静态对象锁
        /// </summary>
        private static readonly object _locker = new object();

        /// <summary>
        /// Consul配置信息
        /// </summary>
        public ServiceDiscoveryOptions Option { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option">配置信息</param>
        public ServiceDiscoveryOptionProvider(IOptions<ServiceDiscoveryOptions> option)
        {
            Option = option.Value ?? new ServiceDiscoveryOptions()
            {
                ListenAddress = "127.0.0.1",
                EndPoint = new RpcEndpoint
                {
                    Address = "127.0.0.1",
                    Port = 8500,
                    Schema = "http://"
                },
                ServiceId = "Service",
            };

            Option.ListenAddress = GetHostAddress(Option.ListenAddress);
            Option.ListenPort = GetHostPort(Option.ListenPort);
        }

        /// <summary>
        /// 获取地址
        /// </summary>
        /// <param name="listenAddress">listenAddress</param>
        /// <returns>string</returns>
        private string GetHostAddress(string listenAddress)
        {
            if (!string.IsNullOrWhiteSpace(listenAddress))
            {
                var separator = listenAddress.IndexOf("/", StringComparison.CurrentCulture);
                if (separator > 0)
                {
                    var baseAddress = listenAddress.Substring(0, separator);
                    if (int.TryParse(listenAddress.Substring(separator + 1), out int maskBits))
                    {
                        return GetHostAddress(baseAddress, maskBits);
                    }

                    return baseAddress;
                }
            }

            return listenAddress;
        }

        /// <summary>
        /// 获取地址
        /// </summary>
        /// <param name="baseAddress">baseAddress</param>
        /// <param name="maskBits">maskBits</param>
        /// <returns>string</returns>
        private string GetHostAddress(string baseAddress, int maskBits)
        {
            if (IPAddress.TryParse(baseAddress, out var ip))
            {
                var subAddress = GetSubAddressBytes(ip, maskBits);
                var addressList = NetworkInterface.GetAllNetworkInterfaces()
                    .Select(p => p.GetIPProperties())
                    .SelectMany(p => p.UnicastAddresses)
                    .Where(p => p.Address.AddressFamily == ip.AddressFamily)
                    .Select(p => p.Address);

                foreach (var _ip in addressList)
                {
                    var _subAddress = GetSubAddressBytes(_ip, maskBits);
                    if (Equals(subAddress, _subAddress))
                    {
                        return _ip.ToString();
                    }
                }
            }

            return baseAddress;
        }

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="subAddress1">地址1</param>
        /// <param name="subAddress2">地址2</param>
        /// <returns>结果</returns>
        private bool Equals(byte[] subAddress1, byte[] subAddress2)
        {
            if (subAddress1.Length != subAddress2.Length)
            {
                return false;
            }

            for (var i = 0; i < subAddress1.Length; i++)
            {
                if (subAddress1[i] != subAddress2[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取地址
        /// </summary>
        /// <param name="ip">ip</param>
        /// <param name="maskBits">maskBits</param>
        /// <returns>byte</returns>
        private byte[] GetSubAddressBytes(IPAddress ip, int maskBits)
        {
            var addressBytes = ip.GetAddressBytes();
            int mod = (int)Math.Floor(maskBits / 8m),
               remainder = maskBits % 8;
            if (remainder > 0)
            {
                addressBytes[mod] &= (byte)(0xFF << (8 - remainder));
                mod += 1;
            }

            for (; mod < addressBytes.Length; mod++)
            {
                addressBytes[mod] = 0x00;
            }

            return addressBytes;
        }

        /// <summary>
        ///  获取一个端口号
        /// </summary>
        /// <returns>端口号</returns>
        private int GetIdlePort()
        {
            //确保只有一个端口
            lock (_locker)
            {
                if (_port != 0)
                {
                    return _port;
                }
            }

            //获取本地计算机的网络连接和通信统计数据的信息            

            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            //返回本地计算机上的所有Tcp监听程序            
            IPEndPoint[] ipsTCP = ipGlobalProperties.GetActiveTcpListeners();
            //返回本地计算机上的所有UDP监听程序            
            IPEndPoint[] ipsUDP = ipGlobalProperties.GetActiveUdpListeners();
            //返回本地计算机上的Internet协议版本4(IPV4 传输控制协议(TCP)连接的信息。            
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();
            var usedPorts = new List<int>();

            usedPorts.AddRange(ipsTCP.Select(d => d.Port));
            usedPorts.AddRange(ipsUDP.Select(d => d.Port));
            usedPorts.AddRange(tcpConnInfoArray.Select(d => d.LocalEndPoint.Port));
            var notUsed = Enumerable.Range(10000, 65530).Except(usedPorts);
            lock (_locker)
            {
                _port = notUsed.ElementAt(new Random().Next(55530));
            }
            lock (_locker)
            {
                return _port;
            }
        }

        /// <summary>
        /// 获取一个端口号
        /// </summary>
        /// <param name="listenPort">端口号</param>
        /// <returns>端口号</returns>
        private int GetHostPort(int listenPort)
        {
            if (listenPort == 0)
            {
                return GetIdlePort();
            }
            return Math.Abs(listenPort);
        }
    }
}