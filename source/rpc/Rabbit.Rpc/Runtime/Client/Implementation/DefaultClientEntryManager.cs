using System;
using System.Collections.Generic;
using System.Linq;

namespace Rabbit.Rpc.Runtime.Client.Implementation
{
    /// <summary>
    /// 一个默认的客户端服务条目管理
    /// </summary>
    public class DefaultClientEntryManager : IClientEntryManager
    {
        /// <summary>
        /// 服务条目
        /// </summary>
        private readonly IEnumerable<ServiceEntry> _clientEntries;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="providers">服务条目提供程序</param>
        public DefaultClientEntryManager(IEnumerable<IClientEntryProvider> providers)
        {
            var list = new List<ServiceEntry>();
            foreach (var provider in providers)
            {
                var entries = provider.GetEntries().ToArray();
                foreach (var entry in entries)
                {
                    if (list.Any(i => i.Descriptor.Id == entry.Descriptor.Id))
                    {
                        throw new InvalidOperationException($"本地包含多个Id为：{entry.Descriptor.Id} 的服务条目。");
                    }
                }
                list.AddRange(entries);
            }
            _clientEntries = list.ToArray();
        }

        /// <summary>
        /// 获取服务条目集合
        /// </summary>
        /// <returns>服务条目集合</returns>
        public IEnumerable<ServiceEntry> GetEntries()
        {
            return _clientEntries;
        }
    }
}