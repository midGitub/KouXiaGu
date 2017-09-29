using System.Collections.Concurrent;
using System.Collections.Generic;

namespace JiongXiaGu
{

    /// <summary>
    /// 字典变化记录器(线程安全);
    /// </summary>
    public class DictionaryChangedRecorder<TKey, TValue> : IDictionaryObserver<TKey, TValue>
    {
        public DictionaryChangedRecorder()
        {
            RecordeQueue = new ConcurrentQueue<RecordeItem<TKey, TValue>>();
        }

        /// <summary>
        /// 根据修改先后顺序存入合集;
        /// </summary>
        internal ConcurrentQueue<RecordeItem<TKey, TValue>> RecordeQueue { get; private set; }

        /// <summary>
        /// 尝试移除并返回并发队列开头处的对象;
        /// </summary>
        public bool TryDequeue(out RecordeItem<TKey, TValue> result)
        {
            return RecordeQueue.TryDequeue(out result);
        }

        void IDictionaryObserver<TKey, TValue>.OnAdded(TKey key, TValue newValue)
        {
            var item = new RecordeItem<TKey, TValue>(key);
            RecordeQueue.Enqueue(item);
        }

        void IDictionaryObserver<TKey, TValue>.OnRemoved(TKey key, TValue originalValue)
        {
            var item = new RecordeItem<TKey, TValue>(key, originalValue);
            RecordeQueue.Enqueue(item);
        }

        void IDictionaryObserver<TKey, TValue>.OnUpdated(TKey key, TValue originalValue, TValue newValue)
        {
            var item = new RecordeItem<TKey, TValue>(key, originalValue);
            RecordeQueue.Enqueue(item);
        }

        void IDictionaryObserver<TKey, TValue>.OnClear(IDictionary<TKey, TValue> dictionary)
        {
            foreach (var pair in dictionary)
            {
                var item = new RecordeItem<TKey, TValue>(pair.Key, pair.Value);
                RecordeQueue.Enqueue(item);
            }
        }
    }
}
