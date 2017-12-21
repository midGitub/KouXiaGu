using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace JiongXiaGu.Collections
{

    /// <summary>
    /// 通过阻塞方式进行同步的类型字典;
    /// </summary>
    public class ConcurrentTypeDictionary : ITypeDictionary
    {
        private TypeDictionary dictionary;
        private ReaderWriterLockSlim readerWriterLock;

        public ConcurrentTypeDictionary()
        {
            dictionary = new TypeDictionary();
            readerWriterLock = new ReaderWriterLockSlim();
        }

        public ConcurrentTypeDictionary(TypeDictionary dictionary)
        {
            this.dictionary = dictionary;
            readerWriterLock = new ReaderWriterLockSlim();
        }

        public int Count
        {
            get
            {
                using (readerWriterLock.ReadLock())
                {
                    return dictionary.Count;
                }
            }
        }

        public IEnumerable<Type> Keys
        {
            get
            {
                using (readerWriterLock.ReadLock())
                {
                    return dictionary.Keys.ToArray();
                }
            }
        }

        public IEnumerable<object> Values
        {
            get
            {
                using (readerWriterLock.ReadLock())
                {
                    return dictionary.Values.ToArray();
                }
            }
        }

        public T Get<T>()
        {
            using (readerWriterLock.ReadLock())
            {
                return dictionary.Get<T>();
            }
        }

        public void Set<T>(T item)
        {
            using (readerWriterLock.WriteLock())
            {
                dictionary.Set(item);
            }
        }

        public void Add<T>(T item)
        {
            using (readerWriterLock.WriteLock())
            {
                dictionary.Add(item);
            }
        }

        public AddOrUpdateStatus AddOrUpdate<T>(T item)
        {
            using (readerWriterLock.WriteLock())
            {
                return dictionary.AddOrUpdate(item);
            }
        }

        public bool Remove<T>()
        {
            using (readerWriterLock.WriteLock())
            {
                return dictionary.Remove<T>();
            }
        }

        public bool Contains<T>()
        {
            using (readerWriterLock.ReadLock())
            {
                return dictionary.Contains<T>();
            }
        }

        public T Find<T>()
        {
            using (readerWriterLock.ReadLock())
            {
                return dictionary.Find<T>();
            }
        }

        public object Find<T>(Func<object, bool> predicate)
        {
            using (readerWriterLock.ReadLock())
            {
                return dictionary.Find<T>(predicate);
            }
        }

        public IEnumerable<T> FindAll<T>()
        {
            using (readerWriterLock.ReadLock())
            {
                return dictionary.FindAll<T>().ToArray();
            }
        }

        public bool TryGetValue<T>(out T item)
        {
            using (readerWriterLock.ReadLock())
            {
                return dictionary.TryGetValue(out item);
            }
        }

        public void Clear()
        {
            using (readerWriterLock.WriteLock())
            {
                dictionary.Clear();
            }
        }
    }
}
