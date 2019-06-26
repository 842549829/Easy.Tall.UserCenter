using System.Linq;
using System.Threading.Tasks;
using Rabbit.Rpc.Address;

namespace Rabbit.Rpc.Runtime.Client.Address.Resolvers.Implementation.Selectors.Implementation
{
    /// <summary>
    /// 哈希源地址选择器
    /// </summary>
    public class KetAmaHashingAddressSelector : AddressSelectorBase
    {
        /// <summary>
        /// 哈希源地址IP地址
        /// </summary>
        private readonly IKetAmaHashingIpAddress _ketAmaHashingIpAddress;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ketAmaHashingIpAddress">哈希源地址客IP地址接口</param>
        public KetAmaHashingAddressSelector(IKetAmaHashingIpAddress ketAmaHashingIpAddress)
        {
            _ketAmaHashingIpAddress = ketAmaHashingIpAddress;
        }

        /// <summary>
        /// 选择一个地址。
        /// </summary>
        /// <param name="context">地址选择上下文。</param>
        /// <returns>地址模型。</returns>
        protected override Task<AddressModel> SelectAsync(AddressSelectContext context)
        {
            var address = context.Address.ToArray();
            var nodes = from item in address
                select item.ToString();
            var ketAmaHashingNodeLocator = new KetAmaHashingNodeLocator(nodes);
            var primary = ketAmaHashingNodeLocator.GetPrimary(_ketAmaHashingIpAddress.GetClientIp());
            return Task.FromResult(address.FirstOrDefault(item => item.ToString() == primary));
        }
    }
}