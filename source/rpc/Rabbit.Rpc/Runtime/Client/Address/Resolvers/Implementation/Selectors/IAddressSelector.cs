﻿using System;
using System.Threading.Tasks;
using Rabbit.Rpc.Address;

namespace Rabbit.Rpc.Runtime.Client.Address.Resolvers.Implementation.Selectors
{
    /// <summary>
    /// 一个抽象的地址选择器
    /// </summary>
    public interface IAddressSelector
    {
        /// <summary>
        /// 选择一个地址
        /// </summary>
        /// <param name="context">地址选择上下文</param>
        /// <returns>地址模型</returns>
        Task<AddressModel> SelectAsync(AddressSelectContext context);
    }
}