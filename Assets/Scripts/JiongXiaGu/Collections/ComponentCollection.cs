using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace JiongXiaGu.Collections
{

    /// <summary>
    /// 组件合集;(线程安全)
    /// </summary>
    public class ComponentCollection<TComponent> : IReadOnlyCollection<TComponent>, IEnumerable<TComponent>
        where TComponent : class
    {
        private readonly List<TComponent> components;
        private readonly ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

        public ComponentCollection()
        {
            components = new List<TComponent>();
        }

        public ComponentCollection(int capacity)
        {
            components = new List<TComponent>(capacity);
        }

        /// <summary>
        /// 元素总数;
        /// </summary>
        public int Count
        {
            get { return components.Count; }
        }

        /// <summary>
        /// 确认是否相同;
        /// </summary>
        private bool Equals<T>(object item)
        {
            return item is T;
        }

        /// <summary>
        /// 添加元素,若合集内已包含此元素类型,则返回异常 ArgumentException;
        /// </summary>
        public void Add<T>(T item)
             where T : class, TComponent
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            using (readerWriterLockSlim.UpgradeableReadLock())
            {
                if (components.Find(obj => obj is T) != null)
                {
                    throw new ArgumentException(string.Format("合集已存在类型[{0}]的元素;", nameof(T)));
                }
                else
                {
                    using (readerWriterLockSlim.WriteLock())
                    {
                        components.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// 添加元素,若合集内已包含此元素类型则更新它;
        /// </summary>
        public AddOrUpdateStatus AddOrUpdate<T>(T item)
              where T : class, TComponent
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            using (readerWriterLockSlim.UpgradeableReadLock())
            {
                int index = components.FindIndex(obj => Equals<T>(obj));
                if (index >= 0)
                {
                    using (readerWriterLockSlim.WriteLock())
                    {
                        components[index] = item;
                        return AddOrUpdateStatus.Updated;
                    }
                }
                else
                {
                    using (readerWriterLockSlim.WriteLock())
                    {
                        components.Add(item);
                        return AddOrUpdateStatus.Added;
                    }
                }
            }
        }

        /// <summary>
        /// 确认合集内是否存在相同的元素;
        /// </summary>
        public bool Contains<T>()
              where T : class, TComponent
        {
            using (readerWriterLockSlim.ReadLock())
            {
                return components.Find(item => Equals<T>(item)) != null;
            }
        }

        /// <summary>
        /// 获取到首个相同类型的元素,若未能获取到则返回 null;
        /// </summary>
        public T Find<T>()
             where T : class, TComponent
        {
            return Find<T>(item => Equals<T>(item));
        }

        /// <summary>
        /// 获取到首个符合要求的元素,若未能获取到则返回 null;
        /// </summary>
        public T Find<T>(Predicate<TComponent> match)
            where T : class, TComponent
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            using (readerWriterLockSlim.ReadLock())
            {
                return components.Find(match) as T;
            }
        }

        /// <summary>
        /// 获取到最后一个相同类型的元素,若未能获取到则返回 null;
        /// </summary>
        public T FindLast<T>()
             where T : class, TComponent
        {
            return FindLast<T>(item => Equals<T>(item));
        }

        /// <summary>
        /// 获取到最后一个符合要求的元素,若未能获取到则返回 null;
        /// </summary>
        public T FindLast<T>(Predicate<TComponent> match)
             where T : class, TComponent
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            using (readerWriterLockSlim.ReadLock())
            {
                return components.FindLast(match) as T;
            }
        }

        /// <summary>
        /// 获取到所有相同类型的元素,若不存在则返回空合集;
        /// </summary>
        public List<T> FindAll<T>()
              where T : class, TComponent
        {
            return FindAll<T>(item => Equals<T>(item));
        }

        /// <summary>
        /// 获取到所有符合要求的元素,若不存在则返回空合集;
        /// </summary>
        public List<T> FindAll<T>(Predicate<TComponent> match)
              where T : class, TComponent
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            using (readerWriterLockSlim.ReadLock())
            {
                List<T> items = new List<T>();
                foreach (var component in components)
                {
                    if (match(component))
                    {
                        var item = component as T;
                        items.Add(item);
                    }
                }
                return items;
            }
        }

        /// <summary>
        /// 移除首个相同类型的元素,若移除成功则返回true,否者返回false;
        /// </summary>
        public bool Remove<T>()
              where T : class, TComponent
        {
            return Remove(obj => Equals<T>(obj));
        }

        /// <summary>
        /// 移除首个符合要求的元素,若移除成功则返回true,否者返回false;
        /// </summary>
        public bool Remove(Predicate<TComponent> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            using (readerWriterLockSlim.UpgradeableReadLock())
            {
                int index = components.FindIndex(match);
                if (index >= 0)
                {
                    using (readerWriterLockSlim.WriteLock())
                    {
                        components.RemoveAt(index);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 移除所有指定类型的元素,并返回移除的元素总数;
        /// </summary>
        public int RemoveAll<T>()
              where T : class, TComponent
        {
            return RemoveAll(obj => Equals<T>(obj));
        }

        /// <summary>
        /// 移除所有符合要求的元素,并返回移除的元素总数;
        /// </summary>
        public int RemoveAll(Predicate<TComponent> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            using (readerWriterLockSlim.WriteLock())
            {
                return components.RemoveAll(match);
            }
        }

        /// <summary>
        /// 返回合集的复制,当合集发生变化时,返回值不会随着改变;
        /// </summary>
        public IEnumerator<TComponent> GetEnumerator()
        {
            using (readerWriterLockSlim.WriteLock())
            {
                var collection = components.ToList();
                return collection.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
