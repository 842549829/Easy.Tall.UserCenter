using System;
using System.Linq;
using System.Threading.Tasks;
using Rabbit.Rpc.Address;

namespace Rabbit.Rpc.Runtime.Client.Address.Resolvers.Implementation.Selectors.Implementation
{
    /// <summary>
    /// 一个随机地址选择器
    /// </summary>
    public class RandomAddressSelector : AddressSelectorBase
    {
        /// <summary>
        /// 随机范围
        /// </summary>
        private readonly Func<int, int, int> _generate;

        /// <summary>
        /// 初始化一个以Random生成随机数的随机地址选择器。
        /// </summary>
        public RandomAddressSelector()
        {
            var random = new Random();
            _generate = (min, max) => random.Next(min, max);
        }

        /// <summary>
        /// 初始化一个自定义的随机地址选择器。
        /// </summary>
        /// <param name="generate">随机数生成委托，第一个参数为最小值，第二个参数为最大值（不可以超过该值）。</param>
        public RandomAddressSelector(Func<int, int, int> generate)
        {
            _generate = generate ?? throw new ArgumentNullException(nameof(generate));
        }

        /// <summary>
        /// 选择一个地址。
        /// </summary>
        /// <param name="context">地址选择上下文。</param>
        /// <returns>地址模型。</returns>
        protected override Task<AddressModel> SelectAsync(AddressSelectContext context)
        {
            var address = context.Address.ToArray();
            var length = address.Length;

            var index = _generate(0, length);
            return Task.FromResult(address[index]);
        }
    }
}