using System;
using System.Collections.Generic;
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
        internal static TaskScheduler TaskScheduler => UnityTaskScheduler.TaskScheduler;

        /// <summary>
        /// 读取到对应资源;
        /// </summary>
        public T Load<T>(AssetReader<T> assetReader, LoadableContent content, AssetInfo assetInfo)
            where T :class
        {
            string key = GetKey(content, assetInfo);
            return Load(key, () => assetReader.Load(content, assetInfo));
        }

        public string GetKey(LoadableContent content, AssetInfo assetInfo)
        {
            return GetKey(content.Description, assetInfo);
        }

        public string GetKey(LoadableContentDescription description, AssetInfo assetInfo)
        {
            const string separator = ":";
            string key;

            switch (assetInfo.From)
            {
                case AssetLoadModes.AssetBundle:
                    key = string.Join(separator, description.ID, assetInfo.AssetBundleName, assetInfo.Name);
                    break;

                case AssetLoadModes.File:
                    key = string.Join(separator, description.ID, assetInfo.Name);
                    break;

                default:
                    throw new NotSupportedException(assetInfo.From.ToString());
            }

            return key;
        }
    }
}
