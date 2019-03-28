using Microsoft.Extensions.Logging;
using Rabbit.Rpc.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rabbit.Rpc.Convertibles.Implementation
{
    /// <inheritdoc />
    /// <summary>
    /// 一个默认的类型转换服务。
    /// </summary>
    public class DefaultTypeConvertibleService : ITypeConvertibleService
    {
        /// <summary>
        /// 类型转换提供程序委托
        /// </summary>
        private readonly IEnumerable<TypeConvertDelegate> _converters;

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<DefaultTypeConvertibleService> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="providers">类型转换提供程序</param>
        /// <param name="logger">日志记录器</param>
        public DefaultTypeConvertibleService(IEnumerable<ITypeConvertibleProvider> providers, ILogger<DefaultTypeConvertibleService> logger)
        {
            _logger = logger;
            providers = providers.ToArray();
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug($"发现了以下类型转换提供程序：{string.Join(",", providers.Select(p => p.ToString()))}。");
            }
            _converters = providers.SelectMany(p => p.GetConverters()).ToArray();
        }

        /// <summary>
        /// 转化
        /// </summary>
        /// <param name="instance">需要转换的实例</param>
        /// <param name="conversionType">转换的类型</param>
        /// <returns>转换之后的类型，如果无法转换则返回null</returns>
        public object Convert(object instance, Type conversionType)
        {
            if (conversionType == null)
            {
                throw new ArgumentNullException(nameof(conversionType));
            }
            if (conversionType.GetTypeInfo().IsInstanceOfType(instance))
            {
                return instance;
            }
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug($"准备将 {instance.GetType()} 转换为：{conversionType}。");
            }

            object result = null;
            try
            {
                foreach (var converter in _converters)
                {
                    if (converter(instance, conversionType, out result))
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                var exception = new RpcException($"无法将实例：{instance}转换为{conversionType}。", e);
                _logger.LogError(exception, $"将 {instance.GetType()} 转换成 {conversionType} 时发生了错误。");
                throw;
            }
            return result;
        }
    }
}