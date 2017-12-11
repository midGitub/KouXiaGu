using System;
using System.Collections.Generic;
using JiongXiaGu.Collections;
using System.Threading;
using System.Linq;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 弱引用对象池;(线程安全)
    /// </summary>
    public class WeakReferenceObjectPool
    {
        internal readonly Dictionary<string, WeakReferenceObject> objectCollection;
        internal readonly ReaderWriterLockSlim objectCollectionLock;

        /// <summary>
        /// 对象总数;
        /// </summary>
        public int Count => objectCollection.Count;

        public WeakReferenceObjectPool()
        {
            objectCollection = new Dictionary<string, WeakReferenceObject>();
            objectCollectionLock = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// 添加内容到合集;
        /// </summary>
        public void Add<T>(string key, WeakReferenceObject<T> value)
            where T : class
        {
            using (objectCollectionLock.UpgradeableReadLock())
            {
                if (objectCollection.ContainsKey(key))
                {
                    using (objectCollectionLock.WriteLock())
                    {
                        objectCollection.Add(key, value);
                    }
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        /// <summary>
        /// 添加或更新合集内容;
        /// </summary>
        public AddOrUpdateStatus AddOrUpdate<T>(string key, WeakReferenceObject<T> value)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException(nameof(key));

            using (objectCollectionLock.WriteLock())
            {
                if (objectCollection.ContainsKey(key))
                {
                    objectCollection[key] = value;
                    return AddOrUpdateStatus.Updated;
                }
                else
                {
                    objectCollection.Add(key, value);
                    return AddOrUpdateStatus.Added;
                }
            }
        }

        /// <summary>
        /// 移除对应资源引用;
        /// </summary>
        public bool Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException(nameof(key));

            using (objectCollectionLock.UpgradeableReadLock())
            {
                if (objectCollection.ContainsKey(key))
                {
                    using (objectCollectionLock.WriteLock())
                    {
                        return objectCollection.Remove(key);
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 确认是否存在该资源,若资源类型不同 或 已经被回收 都返回 false;
        /// </summary>
        public bool Contains<T>(string key)
            where T :class
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException(nameof(key));

            using (objectCollectionLock.ReadLock())
            {
                WeakReferenceObject obj;
                if (objectCollection.TryGetValue(key, out obj))
                {
                    WeakReferenceObject<T> weakReferenceObject = obj as WeakReferenceObject<T>;
                    if (weakReferenceObject != null)
                    {
                        return weakReferenceObject.IsAlive();
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 获取到对应资源,若不存在则返回异常;
        /// </summary>
        public T Get<T>(string key)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException(nameof(key));

            using (objectCollectionLock.ReadLock())
            {
                WeakReferenceObject obj;
                if (objectCollection.TryGetValue(key, out obj))
                {
                    WeakReferenceObject<T> weakReferenceObject = obj as WeakReferenceObject<T>;
                    if (weakReferenceObject != null)
                    {
                        T asset;
                        if (weakReferenceObject.TryGetTarget(out asset))
                        {
                            return asset;
                        }
                    }
                }
                throw new KeyNotFoundException(key);
            }
        }

        /// <summary>
        /// 读取到资源,若已经存在则直接返回;
        /// </summary>
        public T Load<T>(string key, Func<T> loader)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException(nameof(key));
            if (loader == null)
                throw new ArgumentNullException(nameof(loader));

            using (objectCollectionLock.UpgradeableReadLock())
            {
                WeakReferenceObject obj;
                if (objectCollection.TryGetValue(key, out obj))
                {
                    WeakReferenceObject<T> weakReferenceObject = obj as WeakReferenceObject<T>;
                    if (weakReferenceObject == null)
                    {
                        throw new ArgumentException(string.Format("合集内已经存在相同key[{0}],不同资源类型的实例", key));
                    }
                    else
                    {
                        T asset;
                        if (weakReferenceObject.TryGetTarget(out asset))
                        {
                            return asset;
                        }
                        else
                        {
                            return InternalReplace(key, obj, loader);
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
        /// 重新读取到资源;
        /// </summary>
        public T Reload<T>(string key, Func<T> loader)
            where T :class
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException(nameof(key));
            if (loader == null)
                throw new ArgumentNullException(nameof(loader));

            using (objectCollectionLock.UpgradeableReadLock())
            {
                WeakReferenceObject obj;
                if (objectCollection.TryGetValue(key, out obj))
                {
                    WeakReferenceObject<T> weakReferenceObject = obj as WeakReferenceObject<T>;
                    if (weakReferenceObject == null)
                    {
                        return InternalReplace(key, loader);
                    }
                    else
                    {
                        return InternalReplace(key, obj, loader);
                    }
                }
                else
                {
                    return InternalAdd(key, loader);
                }
            }
        }

        /// <summary>
        /// 获取到合集快照;
        /// </summary>
        public KeyValuePair<string, WeakReferenceObject>[] Copy()
        {
            using (objectCollectionLock.ReadLock())
            {
                return objectCollection.ToArray();
            }
        }

        /// <summary>
        /// 清空合集;
        /// </summary>
        public void Clear()
        {
            objectCollection.Clear();
        }

        /// <summary>
        /// 清除所有已经被回收的对象,返回移除的对象个数;(不用经常调用)
        /// </summary>
        public int ClearInvalidObject(CancellationToken token = default(CancellationToken))
        {
            int removed = 0;

            token.ThrowIfCancellationRequested();

            string[] keys;
            using (objectCollectionLock.ReadLock())
            {
                keys = objectCollection.Keys.ToArray();
            }

            token.ThrowIfCancellationRequested();

            foreach (var key in keys)
            {
                using (objectCollectionLock.UpgradeableReadLock())
                {
                    WeakReferenceObject weakReferenceObject;
                    if (objectCollection.TryGetValue(key, out weakReferenceObject))
                    {
                        if (!weakReferenceObject.IsAlive())
                        {
                            using (objectCollectionLock.WriteLock())
                            {
                                objectCollection.Remove(key);
                                removed++;
                            }
                        }
                    }
                }

                token.ThrowIfCancellationRequested();
            }

            return removed;
        }

        /// <summary>
        /// 读取数据,并且替换合集对应资源;
        /// </summary>
        private T InternalReplace<T>(string key, Func<T> loader)
            where T : class
        {
            T asset = loader.Invoke();
            var weakReferenceObject = new WeakReferenceObject<T>(asset);
            using (objectCollectionLock.WriteLock())
            {
                objectCollection[key] = weakReferenceObject;
            }
            return asset;
        }

        /// <summary>
        /// 读取数据,并且替换合集对应资源;
        /// </summary>
        private T InternalReplace<T>(string key, WeakReferenceObject old, Func<T> loader)
            where T : class
        {
            T asset = loader.Invoke();
            var weakReferenceObject = new WeakReferenceObject<T>(asset, old);
            using (objectCollectionLock.WriteLock())
            {
                objectCollection[key] = weakReferenceObject;
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
            using (objectCollectionLock.WriteLock())
            {
                objectCollection.Add(key, weakReferenceObject);
            }
            return asset;
        }
    }
}
