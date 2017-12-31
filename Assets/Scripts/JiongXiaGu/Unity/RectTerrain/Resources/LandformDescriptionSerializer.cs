//using JiongXiaGu.Unity.Resources;
//using System.Collections.Generic;
//using System.IO;

//namespace JiongXiaGu.Unity.RectTerrain
//{

//    public class LandformDescriptionSerializer : ContentSerializer<DescriptionCollection<LandformDescription>>
//    {
//        private readonly XmlSerializer<DescriptionCollection<LandformDescription>> serializer = new XmlSerializer<DescriptionCollection<LandformDescription>>();
//        public override ISerializer<DescriptionCollection<LandformDescription>> Serializer => serializer;

//        [PathDefinition(PathDefinitionType.Data, "地形资源定义")]
//        public override string RelativePath
//        {
//            get { return @"Terrain\Landform" + serializer.FileExtension; }
//        }
//    }
//}
