using System;
using System.Collections.Generic;
using System.Linq;

namespace Rabbit.Rpc.Runtime.Server.Implementation
{
    /// <summary>
    /// 默认的服务条目管理者
    /// </summary>
    public class DefaultServiceEntryManager : IServiceEntryManager
    {
        /// <summary>
        /// 服务条目
        /// </summary>
        private readonly IEnumerable<ServiceEntry> _serviceEntries;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="providers">服务条目提供程序</param>
        public DefaultServiceEntryManager(IEnumerable<IServiceEntryProvider> providers)
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
            _serviceEntries = list.ToArray();
        }

        /// <summary>
        /// 获取服务条目集合。
        /// </summary>
        /// <returns>服务条目集合。</returns>
        public IEnumerable<ServiceEntry> GetEntries()
        {
            return _serviceEntries;
        }
    }
}