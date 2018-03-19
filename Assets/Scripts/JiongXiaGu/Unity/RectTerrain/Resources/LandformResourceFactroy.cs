using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RectTerrain
{

    public class LandformResourceFactroy
    {
        [ModificationSubpathInfo("地形资源描述")]
        private const string LandformDescriptionsFileName = @"Terrain\Landform.xml";

        private readonly XmlSerializer<DescriptionCollection<LandformDescription>> descriptionsSerializer = new XmlSerializer<DescriptionCollection<LandformDescription>>();

        public LandformResource Read(IEnumerable<Modification> modifications, AddMode addMode)
        {
            LandformResource resource = new LandformResource();

            foreach (var modification in modifications)
            {
                var descriptions = Read(modification);
                resource.Add(modification, descriptions.EnumerateDescription(), addMode);
            }

            return resource;
        }

        public DescriptionCollection<LandformDescription> Read(Modification modification)
        {
            if (modification == null)
                throw new ArgumentNullException(nameof(modification));

            return ReadDescriptions(modification);
        }

        public void Write(Modification modification, DescriptionCollection<LandformDescription> descriptions)
        {
            if (modification == null)
                throw new ArgumentNullException(nameof(modification));

            WriteDescriptions(modification, descriptions);
        }


        private DescriptionCollection<LandformDescription> ReadDescriptions(Modification modification)
        {
            using (var stream = modification.BaseContent.GetInputStream(LandformDescriptionsFileName))
            {
                var descriptions = descriptionsSerializer.Deserialize(stream);
                return descriptions;
            }
        }

        private void WriteDescriptions(Modification modification, DescriptionCollection<LandformDescription> descriptions)
        {
            using (var stream = modification.BaseContent.GetOutputStream(LandformDescriptionsFileName))
            {
                descriptionsSerializer.Serialize(stream, descriptions);
            }
        }
    }
}
