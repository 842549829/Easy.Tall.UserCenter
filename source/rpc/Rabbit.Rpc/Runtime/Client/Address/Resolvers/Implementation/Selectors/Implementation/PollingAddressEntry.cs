using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Rabbit.Rpc.Address;

namespace Rabbit.Rpc.Runtime.Client.Address.Resolvers.Implementation.Selectors.Implementation
{
    /// <summary>
    /// 轮询的地址条目
    /// </summary>
    public class PollingAddressEntry
    {
        /// <summary>
        /// 索引
        /// </summary>
        private int _index;

        /// <summary>
        /// 锁
        /// </summary>
        private int _lock;

        /// <summary>
        /// 最大索引
        /// </summary>
        private readonly int _maxIndex;

        /// <summary>
        /// 服务地址集合
        /// </summary>
        private readonly AddressModel[] _address;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="address">服务地址集合</param>
        public PollingAddressEntry(IEnumerable<AddressModel> address)
        {
            _address = address.ToArray();
            _maxIndex = _address.Length - 1;
        }

        /// <summary>
        /// 获取服务地址
        /// </summary>
        /// <returns>服务地址</returns>
        public AddressModel GetAddress()
        {
            while (true)
            {
                //如果无法得到锁则等待
                if (Interlocked.Exchange(ref _lock,1) != 0)
                {
                    default(SpinWait).SpinOnce();
                    continue;
                }

                var address = _address[_index];
                //设置为下一个
                if (_maxIndex > _index)
                {
                    _index++;
                }
                else
                {
                    _index = 0;
                }

                //释放锁
                Interlocked.Exchange(ref _lock, 0);

                return address;
            }
        }
    }
}