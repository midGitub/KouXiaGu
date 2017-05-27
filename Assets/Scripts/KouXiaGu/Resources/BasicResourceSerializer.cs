using KouXiaGu.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Resources
{


    public class BasicResourceSerializer : IReader<BasicResource>
    {
        internal IReader<BasicTerrainResource> TerrainReader = new BasicTerrainResourceSerializer();

        public BasicResource Read()
        {
            var item = new BasicResource();
            item.BasicTerrain = TerrainReader.Read();
            item.Navigation = new NavigationResource(item.BasicTerrain);
            return item;
        }
    }
}
