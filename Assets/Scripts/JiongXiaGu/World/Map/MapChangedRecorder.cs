using JiongXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.World.Map
{

    /// <summary>
    /// 记录地图变化;
    /// </summary>
    class MapChangedRecorder<TKey, TValue> : IDictionaryObserver<TKey, TValue>, IDisposable
    {
        public MapChangedRecorder(IObservableDictionary<TKey, TValue> observableMap)
        {
            ChangedPositions = new HashSet<TKey>();
            unsubscriber = observableMap.Subscribe(this);
        }

        IDisposable unsubscriber;
        public HashSet<TKey> ChangedPositions { get; private set; }

        void IDictionaryObserver<TKey, TValue>.OnAdded(TKey key, TValue newValue)
        {
            ChangedPositions.Add(key);
        }

        void IDictionaryObserver<TKey, TValue>.OnRemoved(TKey key, TValue originalValue)
        {
            ChangedPositions.Remove(key);
        }

        void IDictionaryObserver<TKey, TValue>.OnUpdated(TKey key, TValue originalValue, TValue newValue)
        {
            ChangedPositions.Add(key);
        }

        void IDictionaryObserver<TKey, TValue>.OnClear(IDictionary<TKey, TValue> dictionary)
        {
        }

        /// <summary>
        /// 取消订阅;
        /// </summary>
        public void Dispose()
        {
            if (unsubscriber != null)
            {
                unsubscriber.Dispose();
                unsubscriber = null;
            }
        }
    }
}
