using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using UniRx;

namespace KouXiaGu.Collections
{

    /// <summary>
    /// 可以监视变化的字典结构;
    /// </summary>
    [ProtoContract]
    public class ObservableDictionary<TKey, TValue> : IObservableDictionary<TKey, TValue>
    {

        public ObservableDictionary()
        {
            this.dictionary = new Dictionary<TKey, TValue>();
            this.observers = new List<IObserver<DictionaryChange<TKey, TValue>>>();
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            this.dictionary = new Dictionary<TKey, TValue>(dictionary);
            this.observers = new List<IObserver<DictionaryChange<TKey, TValue>>>();
        }

        public ObservableDictionary(int capacity)
        {
            this.dictionary = new Dictionary<TKey, TValue>(capacity);
            this.observers = new List<IObserver<DictionaryChange<TKey, TValue>>>();
        }


        Dictionary<TKey, TValue> dictionary;

        List<IObserver<DictionaryChange<TKey, TValue>>> observers;

        public TValue this[TKey key]
        {
            get { return this.dictionary[key]; }
            set
            {
                TValue original;
                if (this.dictionary.TryGetValue(key, out original))
                {
                    this.dictionary[key] = value;
                    TrackUpdate(key, original, value);
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
        }

        public int Count
        {
            get { return this.dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((IDictionary<TKey, TValue>)this.dictionary).IsReadOnly; }
        }

        public ICollection<TKey> Keys
        {
            get { return this.dictionary.Keys; }
        }

        public ICollection<TValue> Values
        {
            get {  return this.dictionary.Values; }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// 加入到合集内,若成功加入合集则通知所有订阅者;
        /// </summary>
        public void Add(TKey key, TValue value)
        {
            this.dictionary.Add(key, value);
            TrackAdd(key, value);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public bool Remove(TKey key)
        {
            TValue original;
            if (this.dictionary.TryGetValue(key, out original))
            {
                this.dictionary.Remove(key);
                TrackRemove(key, original);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.dictionary.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return this.dictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }

        public void Clear()
        {
            this.dictionary.Clear();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)this.dictionary).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        public IDisposable Subscribe(IObserver<DictionaryChange<TKey, TValue>> observer)
        {
            if (observer == null)
                throw new ArgumentNullException("订阅者为null;");

            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                Unsubscriber unsubscriber = new Unsubscriber(observers, observer);
                return unsubscriber;
            }
            else
            {
                throw new ArgumentException("已经订阅;");
            }
        }

        public bool Contains(IObserver<DictionaryChange<TKey, TValue>> observer)
        {
            return observers.Contains(observer);
        }

        void TrackAdd(TKey key, TValue newValue)
        {
            var change = new DictionaryChange<TKey, TValue>(Operation.Add, key, default(TValue), newValue);
            TrackChange(change);
        }

        void TrackRemove(TKey key, TValue original)
        {
            var change = new DictionaryChange<TKey, TValue>(Operation.Remove, key, original, default(TValue));
            TrackChange(change);
        }

        void TrackUpdate(TKey key, TValue original, TValue newValue)
        {
            var change = new DictionaryChange<TKey, TValue>(Operation.Update, key, original, newValue);
            TrackChange(change);
        }

        void TrackChange(DictionaryChange<TKey, TValue> change)
        {
            foreach (var observer in observers)
            {
                observer.OnNext(change);
            }
        }

        public void EndTransmission()
        {
            foreach (var observer in observers.ToArray())
            {
                observer.OnCompleted();
            }
            observers.Clear();
        }

        class Unsubscriber : IDisposable
        {
            ICollection<IObserver<DictionaryChange<TKey, TValue>>> observers;
            IObserver<DictionaryChange<TKey, TValue>> observer;

            public Unsubscriber(ICollection<IObserver<DictionaryChange<TKey, TValue>>> observers, IObserver<DictionaryChange<TKey, TValue>> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }

            public void Dispose()
            {
                observers.Remove(observer);
            }
        }

    }

}
