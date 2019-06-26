using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rabbit.Rpc.Convertibles;
using Rabbit.Rpc.Messages;
using Rabbit.Rpc.Runtime.Client;

namespace Rabbit.Rpc.ProxyGenerator.Implementation
{
    /// <summary>
    /// 服务代练抽象类
    /// </summary>
    public abstract class ServiceProxyBase
    {
        /// <summary>
        /// 远程调用服务
        /// </summary>
        private readonly IRemoteInvokeService _remoteInvokeService;

        /// <summary>
        /// 类型转换服务
        /// </summary>
        private readonly ITypeConvertibleService _typeConvertibleService;

        /// <summary>
        /// Rpc当前上下文
        /// </summary>
        public object RpcContext { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="remoteInvokeService">远程调用服务</param>
        /// <param name="typeConvertibleService">类型转换服务</param>
        /// <param name="contextAccessor">Rpc当前上下文</param>
        protected ServiceProxyBase(IRemoteInvokeService remoteInvokeService, ITypeConvertibleService typeConvertibleService, IRpcContextAccessor contextAccessor = null)
        {
            _remoteInvokeService = remoteInvokeService;
            _typeConvertibleService = typeConvertibleService;
            RpcContext = contextAccessor?.RpcContext;
        }

        /// <summary>
        /// 获取泛型类型参数
        /// </summary>
        /// <param name="genericTypes">类型</param>
        /// <returns>泛型类型参数</returns>
        private static IEnumerable<string> GetGenericTypeArguments(params Type[] genericTypes)
        {
            return genericTypes.ToList().ConvertAll(type => Type.GetTypeCode(type) == TypeCode.Object ? type.AssemblyQualifiedName : type.FullName);
        }

        /// <summary>
        /// 内部调用异步
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="parameters">参数</param>
        /// <param name="serviceId">服务Id</param>
        /// <param name="genericTypeArguments">泛型类型参数</param>
        /// <returns>一个异步任务</returns>
        private async Task<T> InternalInvokeAsync<T>(IDictionary<string, object> parameters, string serviceId, IEnumerable<string> genericTypeArguments)
        {
            var message = await _remoteInvokeService.InvokeAsync(new RemoteInvokeContext
            {
                InvokeMessage = new RemoteInvokeMessage
                {
                    Parameters = parameters,
                    ServiceId = serviceId,
                    RpcContext = RpcContext,
                    GenericParameters = genericTypeArguments
                }
            });

            if (message == null)
            {
                return default(T);
            }

            if (message.ExceptionCode != 0)
            {
                throw new Exceptions.RpcRemoteServiceException(message.ExceptionCode, message.ExceptionMessage);
            }

            var result = _typeConvertibleService.Convert(message.Result, typeof(T));

            return (T)result;
        }

        /// <summary>
        /// 远程调用。
        /// </summary>
        /// <param name="parameters">参数字典。</param>
        /// <param name="serviceId">服务Id。</param>
        /// <returns>调用任务。</returns>
        protected async Task InvokeAsync(IDictionary<string, object> parameters, string serviceId)
        {
            var message = await _remoteInvokeService.InvokeAsync(new RemoteInvokeContext
            {
                InvokeMessage = new RemoteInvokeMessage
                {
                    Parameters = parameters,
                    ServiceId = serviceId,
                    RpcContext = RpcContext
                }
            });
            //接收到远程消息，判断消息内容是否含有异常，如果有异常则客户端抛出远程异常
            if (message != null && message.ExceptionCode != 0)
            {
                throw new Exceptions.RpcRemoteServiceException(message.ExceptionCode, message.ExceptionMessage);
            }

        }

        /// <summary>
        /// 远程调用。
        /// </summary>
        /// <param name="parameters">参数字典。</param>
        /// <param name="serviceId">服务Id。</param>
        /// <returns>调用任务。</returns>
        protected async Task<T> InvokeAsync<T>(IDictionary<string, object> parameters, string serviceId)
        {
            var result = await InternalInvokeAsync<T>(parameters, serviceId, new List<string>());
            return result;

        }

        /// <summary>
        /// 远程调用。
        /// </summary>
        /// <typeparam name="Tin">输入类型。</typeparam>
        /// <typeparam name="Tout">返回类型</typeparam>
        /// <param name="parameters">参数字典。</param>
        /// <param name="serviceId">服务Id。</param>
        /// <returns>调用结果。</returns>
        /// <returns></returns>
        protected async Task<Tout> InvokeAsync<Tin, Tout>(IDictionary<string, object> parameters, string serviceId)
        {
            var genericTypeArguments = GetGenericTypeArguments(typeof(Tin));
            var result = await InternalInvokeAsync<Tout>(parameters, serviceId, genericTypeArguments);
            return result;
        }

        /// <summary>
        /// 远程调用。
        /// </summary>
        /// <typeparam name="Tin1">输入类型1。</typeparam>
        /// <typeparam name="Tin2">输入类型2。</typeparam>
        /// <typeparam name="Tout">返回类型</typeparam>
        /// <param name="parameters">参数字典。</param>
        /// <param name="serviceId">服务Id。</param>
        /// <returns>调用结果。</returns>
        /// <returns></returns>
        protected async Task<Tout> InvokeAsync<Tin1, Tin2, Tout>(IDictionary<string, object> parameters, string serviceId)
        {
            var genericTypeArguments = GetGenericTypeArguments(typeof(Tin1), typeof(Tin2));
            var result = await InternalInvokeAsync<Tout>(parameters, serviceId, genericTypeArguments);
            return result;
        }

        /// <summary>
        /// 远程调用。
        /// </summary>
        /// <typeparam name="Tin1">输入类型1。</typeparam>
        /// <typeparam name="Tin2">输入类型2。</typeparam>
        /// <typeparam name="Tin3">输入类型3。</typeparam>
        /// <typeparam name="Tout">返回类型</typeparam>
        /// <param name="parameters">参数字典。</param>
        /// <param name="serviceId">服务Id。</param>
        /// <returns>调用结果。</returns>
        /// <returns></returns>
        protected async Task<Tout> InvokeAsync<Tin1, Tin2, Tin3, Tout>(IDictionary<string, object> parameters, string serviceId)
        {
            var genericTypeArguments = GetGenericTypeArguments(typeof(Tin1), typeof(Tin2), typeof(Tin3));
            var result = await InternalInvokeAsync<Tout>(parameters, serviceId, genericTypeArguments);
            return result;
        }

        /// <summary>
        /// 远程调用。
        /// </summary>
        /// <typeparam name="Tin1">输入类型1。</typeparam>
        /// <typeparam name="Tin2">输入类型2。</typeparam>
        /// <typeparam name="Tin3">输入类型3。</typeparam>
        /// <typeparam name="Tin4">输入类型4。</typeparam>
        /// <typeparam name="Tout">返回类型</typeparam>
        /// <param name="parameters">参数字典。</param>
        /// <param name="serviceId">服务Id。</param>
        /// <returns>调用结果。</returns>
        /// <returns></returns>
        protected async Task<Tout> InvokeAsync<Tin1, Tin2, Tin3, Tin4, Tout>(IDictionary<string, object> parameters, string serviceId)
        {

            var genericTypeArguments = GetGenericTypeArguments(typeof(Tin1), typeof(Tin2), typeof(Tin3), typeof(Tin4));
            var result = await InternalInvokeAsync<Tout>(parameters, serviceId, genericTypeArguments);
            return result;
        }

        /// <summary>
        /// 远程调用。
        /// </summary>
        /// <param name="parameters">参数字典。</param>
        /// <param name="serviceId">服务Id。</param>
        /// <returns>调用结果。</returns>
        protected void Invoke(IDictionary<string, object> parameters, string serviceId)
        {
            Task.Factory.StartNew(async () =>
            {
                await InvokeAsync(parameters, serviceId);
            }).Wait();
        }

        /// <summary>
        /// 远程调用。
        /// </summary>
        /// <typeparam name="T">返回类型。</typeparam>
        /// <param name="parameters">参数字典。</param>
        /// <param name="serviceId">服务Id。</param>
        /// <returns>调用结果。</returns>
        protected T Invoke<T>(IDictionary<string, object> parameters, string serviceId)
        {
            return InvokeAsync<T>(parameters, serviceId).Result;
        }

        /// <summary>
        /// 远程调用。
        /// </summary>
        /// <typeparam name="Tin">输入类型。</typeparam>
        /// <typeparam name="Tout">返回类型。</typeparam>
        /// <param name="parameters">参数字典。</param>
        /// <param name="serviceId">服务Id。</param>
        /// <returns>调用结果。</returns>
        protected Tout Invoke<Tin, Tout>(IDictionary<string, object> parameters, string serviceId)
        {
            return InvokeAsync<Tin, Tout>(parameters, serviceId).Result;
        }

        /// <summary>
        /// 远程调用。
        /// </summary>
        /// <typeparam name="Tin1">输入类型1。</typeparam>
        /// <typeparam name="Tin2">输入类型2。</typeparam>
        /// <typeparam name="Tout">返回类型。</typeparam>
        /// <param name="parameters">参数字典。</param>
        /// <param name="serviceId">服务Id。</param>
        /// <returns>调用结果。</returns>
        protected Tout Invoke<Tin1, Tin2, Tout>(IDictionary<string, object> parameters, string serviceId)
        {
            return InvokeAsync<Tin1, Tin2, Tout>(parameters, serviceId).Result;
        }

        /// <summary>
        /// 远程调用。
        /// </summary>
        /// <typeparam name="Tin1">输入类型1。</typeparam>
        /// <typeparam name="Tin2">输入类型2。</typeparam>
        /// <typeparam name="Tin3">输入类型3。</typeparam>
        /// <typeparam name="Tout">返回类型。</typeparam>
        /// <param name="parameters">参数字典。</param>
        /// <param name="serviceId">服务Id。</param>
        /// <returns>调用结果。</returns>
        protected Tout Invoke<Tin1, Tin2, Tin3, Tout>(IDictionary<string, object> parameters, string serviceId)
        {
            return InvokeAsync<Tin1, Tin2, Tin3, Tout>(parameters, serviceId).Result;
        }

        /// <summary>
        /// 远程调用。
        /// </summary>
        /// <typeparam name="Tin1">输入类型1。</typeparam>
        /// <typeparam name="Tin2">输入类型2。</typeparam>
        /// <typeparam name="Tin3">输入类型3。</typeparam>
        /// <typeparam name="Tin4">输入类型3。</typeparam>
        /// <typeparam name="Tout">返回类型。</typeparam>
        /// <param name="parameters">参数字典。</param>
        /// <param name="serviceId">服务Id。</param>
        /// <returns>调用结果。</returns>
        protected Tout Invoke<Tin1, Tin2, Tin3, Tin4, Tout>(IDictionary<string, object> parameters, string serviceId)
        {
            return InvokeAsync<Tin1, Tin2, Tin3, Tin4, Tout>(parameters, serviceId).Result;
        }
    }
}