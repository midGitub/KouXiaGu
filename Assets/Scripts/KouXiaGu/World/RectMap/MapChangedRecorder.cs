using KouXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.RectMap
{

    /// <summary>
    /// 记录地图变化;
    /// </summary>
    public class MapChangedRecorder<TKey, TVaule> : IDictionaryObserver<TKey, TVaule>, IDisposable
    {
        public MapChangedRecorder(IObservableDictionary<TKey, TVaule> observableMap)
        {
            ChangedPositions = new HashSet<TKey>();
            unsubscriber = observableMap.Subscribe(this);
        }

        public MapChangedRecorder(IObservableDictionary<TKey, TVaule> observableMap, IEnumerable<TKey> changedPositions)
        {
            ChangedPositions = new HashSet<TKey>(changedPositions);
            unsubscriber = observableMap.Subscribe(this);
        }

        IDisposable unsubscriber;
        public HashSet<TKey> ChangedPositions { get; private set; }

        void IDictionaryObserver<TKey, TVaule>.OnAdded(TKey key, TVaule newValue)
        {
            ChangedPositions.Add(key);
        }

        void IDictionaryObserver<TKey, TVaule>.OnRemoved(TKey key, TVaule originalValue)
        {
            ChangedPositions.Remove(key);
        }

        void IDictionaryObserver<TKey, TVaule>.OnUpdated(TKey key, TVaule originalValue, TVaule newValue)
        {
            ChangedPositions.Add(key);
        }

        void IDictionaryObserver<TKey, TVaule>.OnClear()
        {
            ChangedPositions.Clear();
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
