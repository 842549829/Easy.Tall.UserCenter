using System;
using System.Collections.Generic;
using System.Linq;

namespace Rabbit.Rpc
{
    /// <summary>
    /// 服务描述符
    /// </summary>
    public class ServiceDescriptor
    {
        /// <summary>
        /// 构造函数 初始化一个新的服务描述符
        /// </summary>
        public ServiceDescriptor()
        {
            Metadata = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 服务Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 元数据
        /// </summary>
        public IDictionary<string, object> Metadata { get; set; }

        /// <summary>
        /// 获取一个元数据
        /// </summary>
        /// <typeparam name="T">元数据类型</typeparam>
        /// <param name="name">元数据名称</param>
        /// <param name="def">如果指定名称的元数据不存在则返回这个参数</param>
        /// <returns>元数据值</returns>
        public T GetMetadata<T>(string name, T def = default(T))
        {
            if (!Metadata.ContainsKey(name))
            {
                return def;
            }
            return (T)Metadata[name];
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ServiceDescriptor model))
            {
                return false;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return model.Metadata.Count == Metadata.Count && model.Metadata.All(d =>
            {
                if (!Metadata.TryGetValue(d.Key,out var value))
                {
                    return false;
                }
                if (d.Value == null && value == null)
                {
                    return true;
                }
                if (d.Value == null || value == null)
                {
                    return false;
                }
                return d.Value.Equals(value);
            });
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// 相等比较器
        /// </summary>
        /// <param name="model1">model1</param>
        /// <param name="model2">model2</param>
        /// <returns>比较结果</returns>
        public static bool operator ==(ServiceDescriptor model1, ServiceDescriptor model2)
        {
            return Equals(model1, model2);
        }

        /// <summary>
        /// 不相等比较器
        /// </summary>
        /// <param name="model1">model1</param>
        /// <param name="model2">model2</param>
        /// <returns>比较结果</returns>
        public static bool operator !=(ServiceDescriptor model1, ServiceDescriptor model2)
        {
            return !Equals(model1, model2);
        }
    }
}