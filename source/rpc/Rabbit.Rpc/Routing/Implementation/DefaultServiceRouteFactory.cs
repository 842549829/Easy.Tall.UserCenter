using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rabbit.Rpc.Address;
using Rabbit.Rpc.Serialization;

namespace Rabbit.Rpc.Routing.Implementation
{
    /// <summary>
    /// 一个默认的服务路由工厂实现。
    /// </summary>
    public class DefaultServiceRouteFactory : IServiceRouteFactory
    {
        /// <summary>
        /// 序列化器
        /// </summary>
        private readonly ISerializer<string> _serializer;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serializer">序列化器</param>
        public DefaultServiceRouteFactory(ISerializer<string> serializer)
        {
            _serializer = serializer;
        }

        /// <summary>
        /// 根据服务路由描述符创建服务路由。
        /// </summary>
        /// <param name="descriptors">服务路由描述符。</param>
        /// <returns>服务路由集合。</returns>
        public Task<IEnumerable<ServiceRoute>> CreateServiceRoutesAsync(IEnumerable<ServiceRouteDescriptor> descriptors)
        {
            if (descriptors == null)
            {
                throw new ArgumentNullException(nameof(descriptors));
            }

            descriptors = descriptors.ToArray();
            var routes = new List<ServiceRoute>(descriptors.Count());
            routes.AddRange(descriptors.Select(descriptor => new ServiceRoute
            {
                Address = CreateAddress(descriptor.AddressDescriptors),
                ServiceDescriptor = descriptor.ServiceDescriptor
            }));

            return Task.FromResult(routes.AsEnumerable());
        }

        /// <summary>
        /// 创建地址模型
        /// </summary>
        /// <param name="descriptors">服务描述</param>
        /// <returns>地址模型</returns>
        private IEnumerable<AddressModel> CreateAddress(IEnumerable<ServiceAddressDescriptor> descriptors)
        {
            if (descriptors == null)
            {
                yield break;
            }

            foreach (var descriptor in descriptors)
            {
                var addressType = Type.GetType(descriptor.Type);
                yield return (AddressModel)_serializer.Deserialize(descriptor.Value, addressType);
            }
        }
    }
}