using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

namespace KouXiaGu.Rx
{

    public interface IObservableDictionary<TKey, TValue>
    {
        IDisposable Subscribe(IDictionaryObserver<TKey, TValue> observer);
    }


    public interface IDictionaryObserver<TKey, TValue>
    {
        void OnAdded(TKey key, TValue newValue);
        void OnRemoved(TKey key, TValue originalValue);
        void OnUpdated(TKey key, TValue originalValue, TValue newValue);
    }


    [ProtoContract]
    public class ObservableDictionary<TKey, TValue> : IObservableDictionary<TKey, TValue>, IDictionary<TKey, TValue>
    {

        Dictionary<TKey, TValue> dictionary;
        List<IDictionaryObserver<TKey, TValue>> observers;

        public TValue this[TKey key]
        {
            get { return dictionary[key]; }
            set
            {
                TValue original;
                if (dictionary.TryGetValue(key, out original))
                {
                    dictionary[key] = value;
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
            get { return dictionary.Count; }
        }

        public ICollection<TKey> Keys
        {
            get { return dictionary.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return dictionary.Values; }
        }

        public bool IsReadOnly
        {
            get { return ((IDictionary<TKey, TValue>)this.dictionary).IsReadOnly; }
        }

        public IEnumerable<IDictionaryObserver<TKey, TValue>> Observers
        {
            get { return observers; }
        }


        public ObservableDictionary()
        {
            this.dictionary = new Dictionary<TKey, TValue>();
            this.observers = new List<IDictionaryObserver<TKey, TValue>>();
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            this.dictionary = new Dictionary<TKey, TValue>(dictionary);
            this.observers = new List<IDictionaryObserver<TKey, TValue>>();
        }

        public ObservableDictionary(int capacity)
        {
            this.dictionary = new Dictionary<TKey, TValue>(capacity);
            this.observers = new List<IDictionaryObserver<TKey, TValue>>();
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
            if (dictionary.TryGetValue(key, out original))
            {
                dictionary.Remove(key);
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
            return dictionary.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        public void Clear()
        {
            foreach (var item in dictionary)
            {
                TrackRemove(item.Key, item.Value);
            }
            dictionary.Clear();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)this.dictionary).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        public IDisposable Subscribe(IDictionaryObserver<TKey, TValue> observer)
        {
            if (observer == null)
                throw new ArgumentNullException("订阅者为null;");

            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                var unsubscriber = new CollectionUnsubscriber<IDictionaryObserver<TKey, TValue>>(observers, observer);
                return unsubscriber;
            }
            else
            {
                throw new ArgumentException("已经订阅;");
            }
        }

        public bool Contains(IDictionaryObserver<TKey, TValue> observer)
        {
            return observers.Contains(observer);
        }

        IEnumerable<IDictionaryObserver<TKey, TValue>> EnumerateObservers()
        {
            return observers.ToArray();
        }

        void TrackAdd(TKey key, TValue newValue)
        {
            foreach (var observer in EnumerateObservers())
            {
                observer.OnAdded(key, newValue);
            }
        }

        void TrackRemove(TKey key, TValue original)
        {
            foreach (var observer in EnumerateObservers())
            {
                observer.OnRemoved(key, original);
            }
        }

        void TrackUpdate(TKey key, TValue original, TValue newValue)
        {
            foreach (var observer in EnumerateObservers())
            {
                observer.OnUpdated(key, original, newValue);
            }
        }

    }


    public abstract class DictionaryObserver<TKey, TValue> : IDictionaryObserver<TKey, TValue>
    {

        IDisposable unsubscriber;

        public bool IsSubscribed
        {
            get { return unsubscriber != null; }
        }

        public abstract void OnAdded(TKey key, TValue newValue);
        public abstract void OnRemoved(TKey key, TValue originalValue);
        public abstract void OnUpdated(TKey key, TValue originalValue, TValue newValue);

        public void Subscribe(IObservableDictionary<TKey, TValue> provider)
        {
            if (IsSubscribed)
                throw new ArgumentException("已经存在监视内容;");
            if (provider == null)
                throw new ArgumentNullException();

            unsubscriber = provider.Subscribe(this);
        }

        public void Unsubscribe()
        {
            if (unsubscriber != null)
            {
                unsubscriber.Dispose();
                unsubscriber = null;
            }
        }

    }

}
