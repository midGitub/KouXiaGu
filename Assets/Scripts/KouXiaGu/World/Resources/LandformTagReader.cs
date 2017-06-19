using KouXiaGu.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Resources
{


    public class LandformTagReader
    {

        internal LandformTagXmlSerializer LandformTagXmlSerializer = new LandformTagXmlSerializer();

        public WorldResources Read(WorldResources resources, IOperationState state)
        {
            var landformTags = LandformTagXmlSerializer.Read();
            var converter = new LandformTagConverter(landformTags);
            resources.Tags = converter;

            foreach (var landform in resources.Landform.Values)
            {
                landform.TagsMask = converter.TagsToMask(landform.Tags);
            }

            foreach (var building in resources.Building.Values)
            {
                building.TagsMask = converter.TagsToMask(building.Tags);
            }

            return resources;
        }
    }
}
