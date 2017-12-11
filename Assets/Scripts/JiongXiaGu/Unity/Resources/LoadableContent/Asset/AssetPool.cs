using System;
using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资产池,贴图,模型等资产池(线程安全);
    /// </summary>
    public class AssetPool : WeakReferenceObjectPool
    {
        internal static AssetPool Default { get; private set; } = new AssetPool();
        internal static TaskScheduler DefalutTaskScheduler => UnityTaskScheduler.TaskScheduler;

        /// <summary>
        /// 读取到对应资源;
        /// </summary>
        public T Load<T>(AssetLoader<T> assetReader, LoadableContent content, AssetInfo assetInfo)
            where T :class
        {
            if (assetReader == null)
                throw new ArgumentNullException(nameof(assetReader));
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            string key = GetKey<T>(content, assetInfo);
            return Load(key, GetLoader(assetReader, content, assetInfo));
        }

        /// <summary>
        /// 重新读取到对应资源;
        /// </summary>
        public T Reload<T>(AssetLoader<T> assetReader, LoadableContent content, AssetInfo assetInfo)
            where T : class
        {
            if (assetReader == null)
                throw new ArgumentNullException(nameof(assetReader));
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            string key = GetKey<T>(content, assetInfo);
            return Reload(key, GetLoader(assetReader, content, assetInfo));
        }

        /// <summary>
        /// 异步读取到对应资源;
        /// </summary>
        public Task<T> LoadAsync<T>(AssetLoader<T> assetReader, LoadableContent content, AssetInfo assetInfo, CancellationToken token)
            where T : class
        {
            if (assetReader == null)
                throw new ArgumentNullException(nameof(assetReader));
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            string key = GetKey<T>(content, assetInfo);
            return LoadAsync(key, GetLoader(assetReader, content, assetInfo), token, DefalutTaskScheduler);
        }

        /// <summary>
        /// 重新读取到对应资源;
        /// </summary>
        public Task<T> ReloadAsync<T>(AssetLoader<T> assetReader, LoadableContent content, AssetInfo assetInfo, CancellationToken token = default(CancellationToken))
            where T : class
        {
            if (assetReader == null)
                throw new ArgumentNullException(nameof(assetReader));
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            string key = GetKey<T>(content, assetInfo);
            return ReloadAsync(key, GetLoader(assetReader, content, assetInfo));
        }


        public string GetKey<T>(LoadableContent content, AssetInfo assetInfo)
        {
            return GetKey<T>(content.Description, assetInfo);
        }

        public string GetKey<T>(LoadableContentDescription description, AssetInfo assetInfo)
        {
            string key;

            switch (assetInfo.From)
            {
                case AssetLoadModes.AssetBundle:
                    key = GetKey<T, LoadableContent>(description.ID, assetInfo.AssetBundleName, assetInfo.Name);
                    break;

                case AssetLoadModes.File:
                    key = GetKey<T, LoadableContent>(description.ID, assetInfo.Name);
                    break;

                default:
                    throw new NotSupportedException(assetInfo.From.ToString());
            }

            return key;
        }

        private Func<T> GetLoader<T>(AssetLoader<T> assetReader, LoadableContent content, AssetInfo assetInfo)
            where T : class
        {
            return () => assetReader.Load(content, assetInfo);
        }
    }
}
