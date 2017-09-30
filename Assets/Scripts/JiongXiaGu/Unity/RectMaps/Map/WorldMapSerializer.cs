using JiongXiaGu.Unity.Resources.Archives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using JiongXiaGu.Unity.Resources;

namespace JiongXiaGu.Unity.RectMaps
{

    public class WorldMapSerializer : DataSerializer<MapData, ArchiveData, WorldMap>
    {
        public WorldMapSerializer(IResourceReader<MapData> resourceSerializer, ISerializer<ArchiveData> archiveSerializer, string archiveName) : base(resourceSerializer, archiveSerializer, archiveName)
        {
        }

        protected override WorldMap Convert(MapData source, ArchiveData archive)
        {
            return new WorldMap(source, archive);
        }

        protected override ArchiveData ConvertArchive(WorldMap result)
        {
            return result.GetArchiveData();
        }
    }
}
