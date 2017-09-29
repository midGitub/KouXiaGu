using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.VoidableOperations;
using JiongXiaGu.Grids;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 可撤销的加入操作;
    /// </summary>
    public class MapAddOperation : DictionaryAdd<RectCoord, MapNode>
    {
        public MapAddOperation(GameMap map, RectCoord key, MapNode value) : base(map.ObservableMap, key, value)
        {
        }

    }

    /// <summary>
    /// 可撤销的移除操作;
    /// </summary>
    public class MapRemoveOperation : DictionaryRemove<RectCoord, MapNode>
    {
        public MapRemoveOperation(GameMap map, RectCoord key) : base(map.ObservableMap, key)
        {
        }

    }

    /// <summary>
    /// 可撤销的加入操作;
    /// </summary>
    public class MapSetValueOperation : DictionarySetValue<RectCoord, MapNode>
    {
        public MapSetValueOperation(GameMap map, RectCoord key, MapNode newValue) : base(map.ObservableMap, key, newValue)
        {
        }
    }
}
