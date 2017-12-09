using JiongXiaGu.Unity.Resources;
using System.Collections.Generic;
using System.IO;

namespace JiongXiaGu.Unity.RectTerrain
{

    public class LandformDescrSerializer : ContentSerializer<List<LandformDescription>>
    {
        private readonly XmlSerializer<List<LandformDescription>> serializer = new XmlSerializer<List<LandformDescription>>();

        protected override ISerializer<List<LandformDescription>> Serializer
        {
            get { return serializer; }
        }

        [PathDefinition(PathDefinitionType.Data, "地形资源定义")]
        public override string RelativePath
        {
            get { return @"Terrain\Landform" + serializer.FileExtension; }
        }
    }
}
