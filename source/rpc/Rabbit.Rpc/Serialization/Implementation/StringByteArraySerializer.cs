﻿using System;
using System.Text;

namespace Rabbit.Rpc.Serialization.Implementation
{
    /// <summary>
    /// 基于string类型的byte[]序列化器
    /// </summary>
    public class StringByteArraySerializer : ISerializer<byte[]>
    {
        /// <summary>
        /// 序列化器
        /// </summary>
        private readonly ISerializer<string> _serializer;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serializer">序列化器</param>
        public StringByteArraySerializer(ISerializer<string> serializer)
        {
            _serializer = serializer;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="instance">需要序列化的对象</param>
        /// <returns>序列化之后的结果</returns>
        public byte[] Serialize(object instance)
        {
            return Encoding.UTF8.GetBytes(_serializer.Serialize(instance));
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="content">序列化内容</param>
        /// <param name="type">对象类型</param>
        /// <returns>一个对象实例</returns>
        public object Deserialize(byte[] content, Type type)
        {
            return _serializer.Deserialize(Encoding.UTF8.GetString(content), type);
        }
    }
}