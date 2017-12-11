using System;
using System.Collections.Generic;
using JiongXiaGu.Collections;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

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
            ThrowIfKeyInconformity<T>(key);
            if (value == null)
                throw new ArgumentNullException(nameof(value));

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
            ThrowIfKeyInconformity<T>(key);
            if (value == null)
                throw new ArgumentNullException(nameof(value));

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
                throw new ArgumentException(key);

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
            ThrowIfKeyInconformity<T>(key);

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
            ThrowIfKeyInconformity<T>(key);

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
        /// 尝试获取到对应资源;
        /// </summary>
        public bool TryGet<T>(string key, out T value)
            where T : class
        {
            ThrowIfKeyInconformity<T>(key);

            using (objectCollectionLock.ReadLock())
            {
                WeakReferenceObject obj;
                if (objectCollection.TryGetValue(key, out obj))
                {
                    WeakReferenceObject<T> weakReferenceObject = obj as WeakReferenceObject<T>;
                    if (weakReferenceObject != null)
                    {
                        return weakReferenceObject.TryGetTarget(out value);
                    }
                }
                value = default(T);
                return false;
            }
        }

        /// <summary>
        /// 读取到资源,若已经存在则直接返回;
        /// </summary>
        public T Load<T>(string key, Func<T> loader)
            where T : class
        {
            ThrowIfKeyInconformity<T>(key);
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
                        throw InvalidType<T>(key);
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
            ThrowIfKeyInconformity<T>(key);
            if (loader == null)
                throw new ArgumentNullException(nameof(loader));

            using (objectCollectionLock.UpgradeableReadLock())
            {
                if (objectCollection.ContainsKey(key))
                {
                    return InternalReplace(key, loader);
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
        /// 尝试获取到对应类型资源,若key类型不对应则抛出异常;
        /// </summary>
        private bool InternalTryGet<T>(string key, out WeakReferenceObject<T> weakReferenceObject)
            where T : class
        {
            WeakReferenceObject obj;
            if (objectCollection.TryGetValue(key, out obj))
            {
                weakReferenceObject = obj as WeakReferenceObject<T>;
                if (weakReferenceObject == null)
                {
                    throw InvalidType<T>(key);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                weakReferenceObject = null;
                return false;
            }
        }

        /// <summary>
        /// 错误的类型异常,在合集内对象类型和请求的类型不同时抛出;
        /// </summary>
        private Exception InvalidType<T>(string key)
        {
            return new ArgumentException(string.Format("合集内已经存在相同key[{0}],当前请求类型为[{1}];请检查Key定义!", key, typeof(T).Name));
        }



        /// <summary>
        /// 异步方法在等待同步操作时默认的等待时间;
        /// </summary>
        private const int DefaultMillisecondsTimeout = 60;

        public Task<T> LoadAsync<T>(string key, Func<T> loader, CancellationToken token = default(CancellationToken))
            where T : class
        {
            ThrowIfKeyInconformity<T>(key);
            if (loader == null)
                throw new ArgumentNullException(nameof(loader));

            T value;
            if (InternalTrySynchronousGet(key, loader, out value))
            {
                return Task.FromResult(value);
            }
            else
            {
                return Task.Run(delegate ()
                {
                    return Load(key, loader);
                }, token);
            }
        }

        /// <summary>
        /// 异步读取到资源;
        /// </summary>
        public Task<T> LoadAsync<T>(string key, Func<T> loader, CancellationToken token, TaskScheduler taskScheduler)
            where T : class
        {
            ThrowIfKeyInconformity<T>(key);
            if (loader == null)
                throw new ArgumentNullException(nameof(loader));
            if (taskScheduler == null)
                throw new ArgumentNullException(nameof(taskScheduler));

            T value;
            if (InternalTrySynchronousGet(key, loader, out value))
            {
                return Task.FromResult(value);
            }
            else
            {
                return TaskHelper.Run(delegate ()
                {
                    return Load(key, loader);
                }, token, taskScheduler);
            }
        }

        public Task<T> ReloadAsync<T>(string key, Func<T> loader, CancellationToken token = default(CancellationToken))
            where T : class
        {
            ThrowIfKeyInconformity<T>(key);
            if (loader == null)
                throw new ArgumentNullException(nameof(loader));

            return Task.Run(delegate ()
            {
                return Reload(key, loader);
            }, token);
        }

        public Task<T> ReloadAsync<T>(string key, Func<T> loader, CancellationToken token, TaskScheduler taskScheduler)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException(nameof(key));
            if (loader == null)
                throw new ArgumentNullException(nameof(loader));
            if (taskScheduler == null)
                throw new ArgumentNullException(nameof(taskScheduler));

            return TaskHelper.Run(delegate ()
            {
                return Reload(key, loader);
            }, token, taskScheduler);
        }

        /// <summary>
        /// 尝试同步读取到资源;
        /// </summary>
        private bool InternalTrySynchronousGet<T>(string key, Func<T> loader, out T value)
            where T : class
        {
            if (objectCollectionLock.TryEnterReadLock(DefaultMillisecondsTimeout))
            {
                try
                {
                    WeakReferenceObject<T> weakReferenceObject;
                    if (InternalTryGet(key, out weakReferenceObject))
                    {
                        return weakReferenceObject.TryGetTarget(out value);
                    }
                }
                finally
                {
                    objectCollectionLock.ExitReadLock();
                }
            }
            value = default(T);
            return false;
        }



        internal const string KeySeparator = ":";

        /// <summary>
        /// 当Key定义不符合要求时抛出;
        /// </summary>
        public static void ThrowIfKeyInconformity<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException(string.Format("Key定义不符合要求;Key[{1}]", key));
            }

            string name = typeof(T).FullName;
            if (!key.StartsWith(typeof(T).FullName, StringComparison.Ordinal))
            {
                throw new ArgumentException(string.Format("Key定义不符合要求;请求类型为[{0}],Key[{1}]", name, key));
            }
        }

        /// <summary>
        /// 生成唯一的Key;
        /// </summary>
        /// <typeparam name="T">表示资源类型</typeparam>
        /// <typeparam name="TFrom">来自的文件</typeparam>
        /// <param name="types">其它不变的参数</param>
        public static string GetKey<T, TFrom>(string p0)
        {
            string key = string.Join(KeySeparator, typeof(T).FullName, typeof(TFrom).FullName, p0);
            return key;
        }

        /// <summary>
        /// 生成唯一的Key;
        /// </summary>
        /// <typeparam name="T">表示资源类型</typeparam>
        /// <typeparam name="TFrom">来自的文件</typeparam>
        /// <param name="types">其它不变的参数</param>
        public static string GetKey<T, TFrom>(string p0, string p1)
        {
            string key = string.Join(KeySeparator, typeof(T).FullName, typeof(TFrom).FullName, p0, p1);
            return key;
        }

        /// <summary>
        /// 生成唯一的Key;
        /// </summary>
        /// <typeparam name="T">表示资源类型</typeparam>
        /// <typeparam name="TFrom">来自的文件</typeparam>
        /// <param name="types">其它不变的参数</param>
        public static string GetKey<T, TFrom>(string p0, string p1, string p2)
        {
            string key = string.Join(KeySeparator, typeof(T).FullName, typeof(TFrom).FullName, p0, p1, p2);
            return key;
        }

        /// <summary>
        /// 生成唯一的Key;
        /// </summary>
        /// <typeparam name="T">表示资源类型</typeparam>
        /// <typeparam name="TFrom">来自的文件</typeparam>
        /// <param name="types">其它不变的参数</param>
        public static string GetKey<T, TFrom>(string p0, string p1, string p2, string p3)
        {
            string key = string.Join(KeySeparator, typeof(T).FullName, typeof(TFrom).FullName, p0, p1, p2, p3);
            return key;
        }
    }
}
