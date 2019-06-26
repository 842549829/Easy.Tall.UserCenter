using System.Linq;
using Rabbit.Rpc.Messages;

namespace Rabbit.Rpc.Runtime.Server.Implementation
{
    /// <summary>
    /// 默认的服务条目定位器
    /// </summary>
    public class DefaultServiceEntryLocate : IServiceEntryLocate
    {
        /// <summary>
        /// 服务条目管理者
        /// </summary>
        private readonly IServiceEntryManager _serviceEntryManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceEntryManager">服务条目管理者</param>
        public DefaultServiceEntryLocate(IServiceEntryManager serviceEntryManager)
        {
            _serviceEntryManager = serviceEntryManager;
        }

        /// <summary>
        /// 定位服务条目
        /// </summary>
        /// <param name="invokeMessage">远程调用消息。</param>
        /// <returns>服务条目。</returns>
        public ServiceEntry Locate(RemoteInvokeMessage invokeMessage)
        {
            var serviceEntries = _serviceEntryManager.GetEntries();
            return serviceEntries.SingleOrDefault(d => d.Descriptor.Id == invokeMessage.ServiceId);
        }
    }
}