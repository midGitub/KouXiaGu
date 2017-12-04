using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JiongXiaGu.Unity.RectTerrain
{

    public class LandformDescrSerializer : ContentSerializer<List<LandformDescription>>
    {
        private XmlSerializer<List<LandformDescription>> serializer = new XmlSerializer<List<LandformDescription>>();

        public override string RelativePath
        {
            get { return @"Terrain\Landform" + serializer.FileExtension; }
        }

        public override List<LandformDescription> Deserialize(Stream stream)
        {
            return serializer.Deserialize(stream);
        }

        public override void Serialize(Stream stream, List<LandformDescription> item)
        {
            serializer.Serialize(stream, item);
        }
    }
}
