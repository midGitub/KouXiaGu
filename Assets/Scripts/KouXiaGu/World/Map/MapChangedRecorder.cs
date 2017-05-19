using KouXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{


    /// <summary>
    /// 记录地图变化;
    /// </summary>
    class MapChangedRecorder<TKey, TVaule> : IDictionaryObserver<TKey, TVaule>
    {
        public MapChangedRecorder(ObservableDictionary<TKey, TVaule> observableMap)
        {
            this.observableMap = observableMap;
            changedPositions = new HashSet<TKey>();
            observableMap.Subscribe(this);
        }

        readonly ObservableDictionary<TKey, TVaule> observableMap;
        readonly HashSet<TKey> changedPositions;

        public ICollection<TKey> ChangedPositions
        {
            get { return changedPositions; }
        }

        void IDictionaryObserver<TKey, TVaule>.OnAdded(TKey key, TVaule newValue)
        {
            changedPositions.Add(key);
        }

        void IDictionaryObserver<TKey, TVaule>.OnRemoved(TKey key, TVaule originalValue)
        {
            changedPositions.Remove(key);
        }

        void IDictionaryObserver<TKey, TVaule>.OnUpdated(TKey key, TVaule originalValue, TVaule newValue)
        {
            return;
        }

        /// <summary>
        /// 获取到发生变化的节点结构;
        /// </summary>
        public Dictionary<TKey, TVaule> GetChangedData()
        {
            Dictionary<TKey, TVaule> changedData = new Dictionary<TKey, TVaule>();
            foreach (var position in changedPositions)
            {
                TVaule node = observableMap[position];
                changedData.Add(position, node);
            }
            return changedData;
        }
    }
}
