using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Easy.Tall.UserCenter.Framework.Configuration.Consul
{
    /// <summary>
    /// Options数据监听
    /// </summary>
    /// <typeparam name="TOptions">TOptions</typeparam>
    public class OptionsMonitorSource<TOptions> : IOptionsSnapshot<TOptions>, IOptionsMonitor<TOptions> where TOptions : class, new()
    {
        /// <summary>
        /// Options监听缓存
        /// </summary>
        private readonly IOptionsMonitorCache<TOptions> _cache;

        /// <summary>
        /// Options创建工厂
        /// </summary>
        private readonly IOptionsFactory<TOptions> _factory;

        /// <summary>
        /// 变更事件
        /// </summary>
        internal event Action<TOptions, string> OnOptionsChange;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="factory">Options创建工厂</param>
        /// <param name="sources">ChangeTokenSource</param>
        /// <param name="cache">Options监听缓存</param>
        public OptionsMonitorSource(
          IOptionsFactory<TOptions> factory,
          IEnumerable<IOptionsChangeTokenSource<TOptions>> sources,
          IOptionsMonitorCache<TOptions> cache)
        {
            _factory = factory;
            _cache = cache;
            foreach (var source1 in sources)
            {
                var source = source1;
                ChangeToken.OnChange(() => source.GetChangeToken(), InvokeChanged, source.Name);
            }
        }

        /// <summary>
        /// 变更事件
        /// </summary>
        /// <param name="name">名称</param>
        private void InvokeChanged(string name)
        {
            name = name ?? Options.DefaultName;
            var options = Get(name);
            var newOptions = _factory.Create(name);
            foreach (var item in options.GetType().GetProperties())
            {
                if (item.SetMethod == null)
                {
                    continue;
                }
                item.SetValue(options, item.GetValue(newOptions));
            }
            OnOptionsChange?.Invoke(options, name);
        }

        /// <summary>
        /// 当前值
        /// </summary>
        public TOptions CurrentValue => Get(Options.DefaultName);

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>结果</returns>
        public virtual TOptions Get(string name)
        {
            name = name ?? Options.DefaultName;
            return _cache.GetOrAdd(name, () => _factory.Create(name));
        }

        /// <summary>
        /// 变更事件
        /// </summary>
        /// <param name="listener">监听委托</param>
        /// <returns>资源释放接口</returns>
        public IDisposable OnChange(Action<TOptions, string> listener)
        {
            var trackerDisposable = new ChangeTrackerDisposable(this, listener);
            OnOptionsChange += trackerDisposable.OnChange;
            return trackerDisposable;
        }

        /// <summary>
        /// 值
        /// </summary>
        public TOptions Value => Get(Options.DefaultName);

        /// <summary>
        /// 变更事件跟踪
        /// </summary>
        internal class ChangeTrackerDisposable : IDisposable
        {
            /// <summary>
            /// 监听委托
            /// </summary>
            private readonly Action<TOptions, string> _listener;

            /// <summary>
            /// Options数据监听
            /// </summary>
            private readonly OptionsMonitorSource<TOptions> _monitor;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="monitor">Options数据监听</param>
            /// <param name="listener">监听委托</param>
            public ChangeTrackerDisposable(
                OptionsMonitorSource<TOptions> monitor,
              Action<TOptions, string> listener)
            {
                _listener = listener;
                _monitor = monitor;
            }

            /// <summary>
            /// 变更事件
            /// </summary>
            /// <param name="options">TOptions</param>
            /// <param name="name">名称</param>
            public void OnChange(TOptions options, string name)
            {
                _listener(options, name);
            }

            /// <summary>
            /// 资源释放
            /// </summary>
            public void Dispose()
            {
                _monitor.OnOptionsChange -= OnChange;
            }
        }
    }
}