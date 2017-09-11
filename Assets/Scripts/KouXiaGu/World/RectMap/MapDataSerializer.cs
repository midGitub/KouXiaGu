using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KouXiaGu.RectTerrain;
using KouXiaGu.Resources;

namespace KouXiaGu.World.RectMap
{

    /// <summary>
    /// 地图数据读取;
    /// </summary>
    class MapDataSerializer : ResourceSerializer<MapData, MapData>
    {
        public MapDataSerializer(ISerializer<MapData> serializer, ResourceSearcher resourceSearcher) : base(serializer, resourceSearcher)
        {
        }

        protected override MapData Convert(List<MapData> sources)
        {
            throw new NotImplementedException();
        }

        protected override MapData Convert(MapData result)
        {
            return result;
        }
    }
}
