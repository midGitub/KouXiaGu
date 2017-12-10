using System;
using System.Collections.Generic;
using System.Threading;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 弱引用池;
    /// </summary>
    public abstract class WeakReferenceObjectPool
    {
        protected Dictionary<string, WeakReferenceObject> ObjectCollection { get; private set; } = new Dictionary<string, WeakReferenceObject>();
        protected ReaderWriterLockSlim ObjectCollectionLock { get; private set; } = new ReaderWriterLockSlim();

        protected T GetOrLoad<T>(string key, AssetLoadOptions options, Func<T> loader)
            where T : class
        {
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
                            return InternalReplace(key, loader);
                        }
                    }
                    else
                    {
                        if ((options & AssetLoadOptions.RereadOrReplace) > AssetLoadOptions.None)
                        {
                            return InternalReplace(key, loader);
                        }
                        else
                        {
                            throw new ArgumentException("资源格式错误");
                        }
                    }
                }
                else
                {
                    return InternalAdd(key, loader);
                }
            }
        }

        /// <summary>
        /// 读取数据,并且将它加入到资源合集;
        /// </summary>
        private T InternalReplace<T>(string key, Func<T> loader)
            where T : class
        {
            T asset = loader.Invoke();
            var weakReferenceObject = new WeakReferenceObject<T>(asset);
            using (ObjectCollectionLock.WriteLock())
            {
                ObjectCollection[key] = weakReferenceObject;
            }
            return asset;
        }

        /// <summary>
        /// 读取数据,并且将它加入到资源合集;
        /// </summary>
        private T InternalAdd<T>(string key, Func<T> loader)
            where T : class
        {
            T asset = loader.Invoke();
            var weakReferenceObject = new WeakReferenceObject<T>(asset);
            using (ObjectCollectionLock.WriteLock())
            {
                ObjectCollection.Add(key, weakReferenceObject);
            }
            return asset;
        }


        /// <summary>
        /// 当对资源发起请求时调用;
        /// </summary>
        protected void RequestObject(WeakReferenceObject obj)
        {
            throw new NotImplementedException();
        }
    }
}
