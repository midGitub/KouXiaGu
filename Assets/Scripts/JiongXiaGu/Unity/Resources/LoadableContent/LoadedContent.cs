using JiongXiaGu.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 内容加载委托;
    /// </summary>
    public delegate void ContentLoader(LoadableContent content, ITypeDictionary info, CancellationToken token);

    /// <summary>
    /// 读取的内容合集;
    /// </summary>
    public class LoadedContent
    {
        /// <summary>
        /// 线程安全的字典结构;
        /// </summary>
        private readonly IDictionary<LoadableContent, Content> dictionary;

        public LoadedContent()
        {
            dictionary = new Dictionary<LoadableContent, Content>();
        }

        /// <summary>
        /// 读取到合集内容,若已经存在,或则正在读取则直接返回;
        /// </summary>
        public Task ReadAsync(IEnumerable<ContentLoader> loaders, LoadableContent loadableContent)
        {
            Content content;

            if (!dictionary.TryGetValue(loadableContent, out content))
            {
                content = new Content();
                var cancellation = content.CancellationTokenSource = new CancellationTokenSource();
                ConcurrentTypeDictionary info = new ConcurrentTypeDictionary();
                var readTask = content.Task = Task.Run(() => InternalReadParallel(loaders, loadableContent, info, cancellation.Token));
                dictionary.Add(loadableContent, content);
            }

            return content.Task;
        }

        /// <summary>
        /// 并发读取;
        /// </summary>
        private void InternalReadParallel(IEnumerable<ContentLoader> loaders, LoadableContent loadableContent, ConcurrentTypeDictionary info, CancellationToken token)
        {
            ParallelOptions options = new ParallelOptions()
            {
                CancellationToken = token,
            };

            Parallel.ForEach(loaders, options, delegate (ContentLoader loader)
            {
                loader.Invoke(loadableContent, info, token);
            });
        }

        private struct Content
        {
            public CancellationTokenSource CancellationTokenSource { get; set; }
            public Task Task { get; set; }
        }
    }
}
