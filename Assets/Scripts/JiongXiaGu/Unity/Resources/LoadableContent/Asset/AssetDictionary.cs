using System;
using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资产池,贴图,模型等资产弱引用池(线程安全);
    /// </summary>
    public class AssetDictionary : WeakReferenceObjectDictionary
    {
        internal static AssetDictionary Default { get; private set; } = new AssetDictionary();
        internal static TaskScheduler DefalutTaskScheduler => UnityTaskScheduler.TaskScheduler;

        /// <summary>
        /// 读取到对应资源;
        /// </summary>
        public T Load<T>(AssetLoader<T> loader, LoadableContent content, AssetInfo assetInfo)
            where T :class
        {
            if (loader == null)
                throw new ArgumentNullException(nameof(loader));
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            string key = GetKey<T>(content, assetInfo);
            return Load(key, GetLoader(loader, content, assetInfo));
        }

        /// <summary>
        /// 重新读取到对应资源;
        /// </summary>
        public T Reload<T>(AssetLoader<T> loader, LoadableContent content, AssetInfo assetInfo)
            where T : class
        {
            if (loader == null)
                throw new ArgumentNullException(nameof(loader));
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            string key = GetKey<T>(content, assetInfo);
            return Reload(key, GetLoader(loader, content, assetInfo));
        }

        /// <summary>
        /// 异步读取到对应资源;
        /// </summary>
        public Task<T> LoadAsync<T>(AssetLoader<T> loader, LoadableContent content, AssetInfo assetInfo, CancellationToken token)
            where T : class
        {
            if (loader == null)
                throw new ArgumentNullException(nameof(loader));
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            string key = GetKey<T>(content, assetInfo);
            return LoadAsync(key, GetLoader(loader, content, assetInfo), token, DefalutTaskScheduler);
        }

        /// <summary>
        /// 重新读取到对应资源;
        /// </summary>
        public Task<T> ReloadAsync<T>(AssetLoader<T> loader, LoadableContent content, AssetInfo assetInfo, CancellationToken token = default(CancellationToken))
            where T : class
        {
            if (loader == null)
                throw new ArgumentNullException(nameof(loader));
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            string key = GetKey<T>(content, assetInfo);
            return ReloadAsync(key, GetLoader(loader, content, assetInfo));
        }


        public string GetKey<T>(LoadableContent content, AssetInfo assetInfo)
        {
            return GetKey<T>(content.Description, assetInfo);
        }

        public string GetKey<T>(LoadableContentDescription description, AssetInfo assetInfo)
        {
            const string KeySeparator = ":";
            string key;

            switch (assetInfo.From)
            {
                case AssetLoadModes.AssetBundle:
                    key = string.Join(KeySeparator, typeof(T).FullName, typeof(LoadableContent).FullName, description.ID, assetInfo.AssetBundleName, assetInfo.Name);
                    break;

                case AssetLoadModes.File:
                    key = string.Join(KeySeparator, typeof(T).FullName, typeof(LoadableContent).FullName, description.ID, assetInfo.Name);
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
