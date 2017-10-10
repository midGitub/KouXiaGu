using JiongXiaGu.Collections;
using JiongXiaGu.Concurrent;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace JiongXiaGu.Unity.Archives
{

    /// <summary>
    /// 游戏场景归档数据(线程安全);
    /// </summary>
    public class SceneArchivalData : IEnumerable<IDataArchival>
    {
        readonly List<IDataArchival> archivalData;
        ReaderWriterLockSlim readerWriterLock;

        public SceneArchivalData()
        {
            archivalData = new List<IDataArchival>();
            readerWriterLock = new ReaderWriterLockSlim();
        }

        public int Count
        {
            get { return archivalData.Count; }
        }

        /// <summary>
        /// 添加状态信息;
        /// </summary>
        public void Add<T>(T item)
            where T : class, IDataArchival
        {
            using (readerWriterLock.WriteLock())
            {
                if (!archivalData.Contains(i => i is T))
                {
                    archivalData.Add(item);
                }
            }
        }

        /// <summary>
        /// 移除指定状态信息;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public bool Remove<T>()
            where T : class, IDataArchival
        {
            using (readerWriterLock.WriteLock())
            {
                return archivalData.Remove(item => item is T);
            }
        }

        /// <summary>
        /// 获取到指定信息,若不存在则返回NULL;
        /// </summary>
        public T Get<T>()
            where T : class, IDataArchival
        {
            using (readerWriterLock.ReadLock())
            {
                return archivalData.Find(item => item is T) as T;
            }
        }

        public IEnumerator<IDataArchival> GetEnumerator()
        {
            using (readerWriterLock.WriteLock())
            {
                return archivalData.ToList().GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
