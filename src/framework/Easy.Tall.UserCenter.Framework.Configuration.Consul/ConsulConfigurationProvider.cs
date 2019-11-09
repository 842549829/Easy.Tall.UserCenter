using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Easy.Tall.UserCenter.Framework.Configuration.Consul
{
    /// <summary>
    /// ConsulConfigurationProvider
    /// </summary>
    public class ConsulConfigurationProvider : ConfigurationProvider, IObserver
    {
        /// <summary>
        /// consul配置
        /// </summary>
        private ConsulAgentConfiguration Configuration { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration">consul配置</param>
        public ConsulConfigurationProvider(ConsulAgentConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 加载(或重新加载)此提供程序的数据
        /// </summary>
        public override void Load()
        {
            QueryConsulAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// 读取consul配置
        /// </summary>
        /// <returns>异步操作</returns>
        private async Task QueryConsulAsync()
        {
            using (var client = new ConsulClient(options =>
            {
                options.WaitTime = Configuration.ClientConfiguration.WaitTime;
                options.Token = Configuration.ClientConfiguration.Token;
                options.Datacenter = Configuration.ClientConfiguration.Datacenter;
                options.Address = Configuration.ClientConfiguration.Address;
            }))
            {
                var result = await client.KV.List(Configuration.QueryOptions.Folder, new QueryOptions
                {
                    Token = Configuration.ClientConfiguration.Token,
                    Datacenter = Configuration.ClientConfiguration.Datacenter
                });
                if (result.Response == null || !result.Response.Any())
                {
                    return;
                }

                SetData(result.Response.ToList());
                //foreach (var item in result.Response)
                //{
                //    item.Key = item.Key.TrimFolderPrefix(Configuration.QueryOptions.Folder);
                //    if (string.IsNullOrWhiteSpace(item.Key))
                //    {
                //       continue; 
                //    }
                //    SetData(item);
                //}
            }
        }

        //private void SetData(KVPair item)
        //{
        //    var dic = Json(item.Key, ReadValue(item.Value));
        //    foreach (var d in dic)
        //    {
        //        if (Data.ContainsKey(d.Key))
        //        {
        //            Data[d.Key] = d.Value;
        //        }
        //        else
        //        {
        //            Set(d.Key, d.Value);
        //        }
        //    }
        //}

        /// <summary>
        /// json转化
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="val">val</param>
        /// <returns>json</returns>
        public IDictionary<string, string> Json(string key, string val)
        {
            var jsonStr = $"{{\"{key}\":{val}}}";
            return JsonConfigurationFileParser.Parse(jsonStr);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="bytes">bytes</param>
        /// <returns>string</returns>
        public string ReadValue(byte[] bytes)
        {
            return bytes != null && bytes.Length > 0
                ? Encoding.UTF8.GetString(bytes)
                : string.Empty;
        }

        /// <summary>
        /// 配置更改
        /// </summary>
        /// <param name="kvPairs">keyValue</param>
        /// <param name="logger">日志</param>
        public void OnChange(List<KVPair> kvPairs, ILogger logger)
        {
            SetData(kvPairs);
            //if (kvPairs == null || !kvPairs.Any())
            //{
            //    Data.Clear();
            //    OnReload();
            //    return;
            //}

            //var deleted = Data.Where(p => kvPairs.All(c => p.Key != c.Key.TrimFolderPrefix(Configuration.QueryOptions.Folder))).ToList();

            //foreach (var del in deleted)
            //{
            //    logger.LogTrace($"Remove key [{del.Key}]");
            //    Data.Remove(del.Key);
            //}

            //foreach (var item in kvPairs)
            //{
            //    item.Key = item.Key.TrimFolderPrefix(Configuration.QueryOptions.Folder);
            //    if (string.IsNullOrWhiteSpace(item.Key))
            //    {
            //        continue;
            //    }
            //    var newValue = ReadValue(item.Value);
            //    if (Data.TryGetValue(item.Key, out var oldValue))
            //    {
            //        if (oldValue == newValue)
            //        {
            //            continue;
            //        }

            //        SetData(item);
            //        logger.LogTrace($"The value of key [{item.Key}] is changed from [{oldValue}] to [{newValue}]");
            //    }
            //    else
            //    {
            //        SetData(item);
            //        logger.LogTrace($"Added key [{item.Key}][{newValue}]");
            //    }
            //    OnReload();
            //}
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="kVs">kv</param>
        private void SetData(List<KVPair> kVs)
        {
            Data.Clear();
            if (kVs == null || !kVs.Any())
            {
                OnReload();
                return;
            }
            //读取子目录
            var fo = Configuration.QueryOptions.Folders;
            var folder = new Dictionary<string, int>();
            for (var i = 0; i < fo.Length; i++)
            {
                folder.Add(fo[i], i);
            }
            //组装consul kv
            var list = new List<PrefixItem>();
            foreach (var item in kVs)
            {
                if (string.IsNullOrWhiteSpace(item.Key))
                {
                    continue;
                }
                var keyPrefix = item.Key.Substring(0, item.Key.LastIndexOf('/') + 1);
                var key = item.Key.Substring(item.Key.LastIndexOf('/') + 1);
                if (string.IsNullOrWhiteSpace(key))
                {
                    continue;
                }
                if (folder.ContainsKey(keyPrefix))
                {
                    list.Add(new PrefixItem
                    {
                        Key = key,
                        Prefix = keyPrefix,
                        Value = item.Value,
                        Index = folder[keyPrefix]
                    });
                }
            }
            //根据key分组 优先取子目录key
            var filter = list.GroupBy(d => d.Key);
            foreach (var item in filter)
            {
                var data = item.MaxElement(x => x.Index);
                var dic = Json(item.Key, ReadValue(data.Value));
                foreach (var d in dic)
                {
                    if (Data.ContainsKey(d.Key))
                    {
                        Data[d.Key] = d.Value;
                    }
                    else
                    {
                        Set(d.Key, d.Value);
                    }
                }
            }
            OnReload();
        }
    }
}