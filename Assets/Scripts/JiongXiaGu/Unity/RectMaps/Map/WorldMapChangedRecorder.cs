using System;
using System.Collections.Generic;

namespace JiongXiaGu.Unity.RectMaps
{
    /// <summary>
    /// 记录地图变化;
    /// </summary>
    public class WorldMapChangedRecorder<TKey, Tvalue> : IDictionaryObserver<TKey, Tvalue>, IDisposable
    {
        public WorldMapChangedRecorder(IObservableDictionary<TKey, Tvalue> observableMap)
        {
            ChangedPositions = new HashSet<TKey>();
            unsubscriber = observableMap.Subscribe(this);
        }

        public WorldMapChangedRecorder(IObservableDictionary<TKey, Tvalue> observableMap, IEnumerable<TKey> changedPositions)
        {
            ChangedPositions = new HashSet<TKey>(changedPositions);
            unsubscriber = observableMap.Subscribe(this);
        }

        IDisposable unsubscriber;
        public HashSet<TKey> ChangedPositions { get; private set; }

        void IDictionaryObserver<TKey, Tvalue>.OnAdded(TKey key, Tvalue newValue)
        {
            ChangedPositions.Add(key);
        }

        void IDictionaryObserver<TKey, Tvalue>.OnRemoved(TKey key, Tvalue originalValue)
        {
            ChangedPositions.Remove(key);
        }

        void IDictionaryObserver<TKey, Tvalue>.OnUpdated(TKey key, Tvalue originalValue, Tvalue newValue)
        {
            ChangedPositions.Add(key);
        }

        public void Clear()
        {
            ChangedPositions.Clear();
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
