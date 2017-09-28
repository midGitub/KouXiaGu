using JiongXiaGu.Unity.Resources.Archives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using JiongXiaGu.Unity.Resources;

namespace JiongXiaGu.World.RectMap
{

    public class WorldMapSerializer : DataSerializer<MapData, MapData, WorldMap>
    {
        public WorldMapSerializer(IResourceReader<MapData> resourceSerializer, ISerializer<MapData> archiveSerializer, string archiveName) : base(resourceSerializer, archiveSerializer, archiveName)
        {
        }

        protected override WorldMap Convert(MapData source, MapData archive)
        {
            return new WorldMap(source, archive);
        }

        protected override MapData ConvertArchive(WorldMap result)
        {
            return result.GetArchivedMapData();
        }
    }
}
