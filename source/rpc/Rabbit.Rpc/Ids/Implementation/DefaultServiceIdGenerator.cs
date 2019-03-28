﻿using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Rabbit.Rpc.Ids.Implementation
{
    /// <summary>
    /// 一个默认的服务Id生成器
    /// </summary>
    public class DefaultServiceIdGenerator : IServiceIdGenerator
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<DefaultServiceIdGenerator> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public DefaultServiceIdGenerator(ILogger<DefaultServiceIdGenerator> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 生成一个服务Id
        /// </summary>
        /// <param name="method">本地方法信息</param>
        /// <returns>对应方法的唯一服务Id</returns>
        public string GenerateServiceId(MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            var type = method.DeclaringType;
            if (type == null)
            {
                throw new ArgumentNullException(nameof(method.DeclaringType), "方法的定义类型不能为空。");
            }
            var id = $"{type.FullName}.{method.Name}[{method.GetGenericArguments().Length}]({string.Join(",", method.GetParameters().ToList().ConvertAll(t => t.ParameterType.ToString()))})";
            _logger.LogDebug($"为方法：{method}生成服务Id：{id}。");
            return id;
        }
    }
}