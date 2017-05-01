using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World.Map;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 监视地图变化;
    /// </summary>
    public class WorldMapWatcher<T> : IDictionaryObserver<T, MapNode>
    {
        void IDictionaryObserver<T, MapNode>.OnAdded(T key, MapNode newValue)
        {
            throw new NotImplementedException();
        }

        void IDictionaryObserver<T, MapNode>.OnRemoved(T key, MapNode originalValue)
        {
            throw new NotImplementedException();
        }

        void IDictionaryObserver<T, MapNode>.OnUpdated(T key, MapNode originalValue, MapNode newValue)
        {
            throw new NotImplementedException();
        }
    }

}
