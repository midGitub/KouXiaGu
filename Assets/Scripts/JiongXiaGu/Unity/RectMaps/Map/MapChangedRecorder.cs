using JiongXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 地图记录;
    /// </summary>
    public class MapChangedRecorder: IObserver<DictionaryEvent<RectCoord, MapNode>>
    {
        /// <summary>
        /// 发生变化的坐标;
        /// </summary>
        public HashSet<RectCoord> ChangedPositions { get; private set; }

        public MapChangedRecorder()
        {
            ChangedPositions = new HashSet<RectCoord>();
        }

        public MapChangedRecorder(IEnumerable<RectCoord> positions)
        {
            ChangedPositions = new HashSet<RectCoord>(positions);
        }

        void IObserver<DictionaryEvent<RectCoord, MapNode>>.OnNext(DictionaryEvent<RectCoord, MapNode> value)
        {
            ChangedPositions.Add(value.Key);
        }

        void IObserver<DictionaryEvent<RectCoord, MapNode>>.OnCompleted()
        {
            throw new NotImplementedException();
        }

        void IObserver<DictionaryEvent<RectCoord, MapNode>>.OnError(Exception error)
        {
            throw new NotImplementedException();
        }
    }
}
