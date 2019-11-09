using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Easy.Tall.UserCenter.Framework.Configuration.Consul
{
    /// <summary>
    /// json转化器
    /// </summary>
    public class JsonConfigurationFileParser
    {
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private JsonConfigurationFileParser() { }

        /// <summary>
        /// 数据对象
        /// </summary>
        private readonly IDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 队列
        /// </summary>
        private readonly Stack<string> _context = new Stack<string>();

        /// <summary>
        /// 当前路径
        /// </summary>
        private string _currentPath;

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="input">流</param>
        /// <returns>字段</returns>
        public static IDictionary<string, string> Parse(Stream input)
            => new JsonConfigurationFileParser().ParseStream(input);

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>字典</returns>
        public static IDictionary<string, string> Parse(string input)
            => new JsonConfigurationFileParser().ParseString(input);

        /// <summary>
        /// 解析字符串
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>字典</returns>
        private IDictionary<string, string> ParseString(string input)
        {
            _data.Clear();

            var jsonConfig = JObject.Parse(input);

            VisitJObject(jsonConfig);

            return _data;
        }

        /// <summary>
        /// 解析流
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>字典</returns>
        private IDictionary<string, string> ParseStream(Stream input)
        {
            _data.Clear();
            var _reader = new JsonTextReader(new StreamReader(input)) { DateParseHandling = DateParseHandling.None };

            var jsonConfig = JObject.Load(_reader);

            VisitJObject(jsonConfig);

            return _data;
        }

        /// <summary>
        /// JObject
        /// </summary>
        /// <param name="jObject">jObject</param>
        private void VisitJObject(JObject jObject)
        {
            foreach (var property in jObject.Properties())
            {
                EnterContext(property.Name);
                VisitProperty(property);
                ExitContext();
            }
        }

        /// <summary>
        /// Property
        /// </summary>
        /// <param name="property">property</param>
        private void VisitProperty(JProperty property)
        {
            VisitToken(property.Value);
        }

        /// <summary>
        /// VisitToken
        /// </summary>
        /// <param name="token">token</param>
        private void VisitToken(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    VisitJObject(token.Value<JObject>());
                    break;

                case JTokenType.Array:
                    VisitArray(token.Value<JArray>());
                    break;

                case JTokenType.Integer:
                case JTokenType.Float:
                case JTokenType.String:
                case JTokenType.Boolean:
                case JTokenType.Bytes:
                case JTokenType.Raw:
                case JTokenType.Null:
                    VisitPrimitive(token.Value<JValue>());
                    break;

                default:
                    throw new FormatException();
            }
        }

        /// <summary>
        /// VisitArray
        /// </summary>
        /// <param name="array">array</param>
        private void VisitArray(JArray array)
        {
            for (int index = 0; index < array.Count; index++)
            {
                EnterContext(index.ToString());
                VisitToken(array[index]);
                ExitContext();
            }
        }

        /// <summary>
        /// VisitPrimitive
        /// </summary>
        /// <param name="data">data</param>
        private void VisitPrimitive(JValue data)
        {
            var key = _currentPath;

            if (_data.ContainsKey(key))
            {
                throw new FormatException(key);
            }
            _data[key] = data.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// EnterContext
        /// </summary>
        /// <param name="context">context</param>
        private void EnterContext(string context)
        {
            _context.Push(context);
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }

        /// <summary>
        /// ExitContext
        /// </summary>
        private void ExitContext()
        {
            _context.Pop();
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }
    }
}
