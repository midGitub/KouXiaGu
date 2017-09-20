using KouXiaGu.Resources.Archives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using KouXiaGu.Resources;

namespace KouXiaGu.World.RectMap
{

    public class WorldMapSerializer : ArchiveSerializer<MapData, MapData, WorldMap>
    {
        public WorldMapSerializer(IResourceSerializer<MapData> resourceSerializer, ISerializer<MapData> archiveSerializer, string archiveName) : base(resourceSerializer, archiveSerializer, archiveName)
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
