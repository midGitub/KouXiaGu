using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RectMaps
{


    public class MapSearcher : ContentSearcher<MapFileInfo>
    {
        [PathDefinition(PathDefinitionType.DataDirectory, "地图资源目录;")]
        internal const string directoryName = "Maps";

        private MapSerializer mapSerializer = new MapSerializer();

        protected override string DirectoryName
        {
            get { return directoryName; }
        }

        protected override string SearchPattern
        {
            get { return "Map_*.zip"; }
        }

        protected override MapFileInfo Deserialize(Content content, string entry)
        {
            using (var stream = content.GetInputStream(entry))
            {
                MapDescription description = mapSerializer.DeserializeDesc(stream);
                MapFileInfo mapFileInfo = new MapFileInfo(description, content, entry);
                return mapFileInfo;
            }
        }
    }
}
