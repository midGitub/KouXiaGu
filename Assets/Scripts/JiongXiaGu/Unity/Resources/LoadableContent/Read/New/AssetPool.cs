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
        public T Load<T>(AssetReader<T> assetReader, LoadableContent content, AssetInfo assetInfo, AssetLoadOptions options)
            where T :class
        {
            string key = GetKey(content, assetInfo);

            using (ObjectCollectionLock.UpgradeableReadLock())
            {
                WeakReferenceObject obj;
                if (ObjectCollection.TryGetValue(key, out obj))
                {
                    WeakReferenceObject<T> weakReferenceObject = obj as WeakReferenceObject<T>;
                    if (weakReferenceObject != null)
                    {
                        T asset;
                        if (weakReferenceObject.Reference.TryGetTarget(out asset))
                        {
                            return asset;
                        }
                        else
                        {
                            return InternalReplace(key, assetReader, content, assetInfo);
                        }
                    }
                    else
                    {
                        if ((options & AssetLoadOptions.RereadOrReplace) > AssetLoadOptions.None)
                        {
                            return InternalReplace(key, assetReader, content, assetInfo);
                        }
                        else
                        {
                            throw new ArgumentException("资源格式错误");
                        }
                    }
                }
                else
                {
                    return InternalAdd(key, assetReader, content, assetInfo);
                }
            }
        }

        private string GetKey(LoadableContent content, AssetInfo assetInfo)
        {
            return GetKey(content.Description, assetInfo);
        }

        private string GetKey(LoadableContentDescription description, AssetInfo assetInfo)
        {
            const string separator = ":";
            string key;

            switch (assetInfo.From)
            {
                case LoadMode.AssetBundle:
                    key = string.Join(separator, description.ID, assetInfo.AssetBundleName, assetInfo.Name);
                    break;

                case LoadMode.File:
                    key = string.Join(separator, description.ID, assetInfo.Name);
                    break;

                default:
                    throw new NotSupportedException(assetInfo.From.ToString());
            }

            return key;
        }

        /// <summary>
        /// 读取数据,并且将它加入到资源合集;
        /// </summary>
        private T InternalAdd<T>(string key, AssetReader<T> assetReader, LoadableContent content, AssetInfo assetInfo)
            where T : class
        {
            T asset = assetReader.Load(content, assetInfo);
            var weakReferenceObject = assetReader.AsWeakReferenceObject(asset);
            using (ObjectCollectionLock.WriteLock())
            {
                ObjectCollection.Add(key, weakReferenceObject);
            }
            return asset;
        }

        /// <summary>
        /// 读取数据,并且将它替换到资源合集;
        /// </summary>
        private T InternalReplace<T>(string key, AssetReader<T> assetReader, LoadableContent content, AssetInfo assetInfo)
            where T : class
        {
            T asset = assetReader.Load(content, assetInfo);
            var weakReferenceObject = assetReader.AsWeakReferenceObject(asset);
            using (ObjectCollectionLock.WriteLock())
            {
                ObjectCollection[key] = weakReferenceObject;
            }
            return asset;
        }
    }
}
