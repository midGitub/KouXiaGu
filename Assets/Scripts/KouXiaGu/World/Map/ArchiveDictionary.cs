using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;
using ProtoBuf;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 监视字典结构,同步操作;
    /// </summary>
    [ProtoContract]
    public class ArchiveDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IDictionaryObserver<TKey, TValue>
    {

        IDisposable unsubscriber;

        public bool IsSubscribed
        {
            get { return unsubscriber != null; }
        }

        public ArchiveDictionary() : base()
        {
        }

        void IDictionaryObserver<TKey, TValue>.OnAdded(TKey key, TValue newValue)
        {
            Add(key, newValue);
        }

        void IDictionaryObserver<TKey, TValue>.OnRemoved(TKey key, TValue originalValue)
        {
            Remove(key);
        }

        void IDictionaryObserver<TKey, TValue>.OnUpdated(TKey key, TValue originalValue, TValue newValue)
        {
            this.AddOrUpdate(key, newValue);
        }

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
