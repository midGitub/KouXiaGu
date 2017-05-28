using KouXiaGu.Navigation;
using KouXiaGu.Terrain3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Resources
{

    /// <summary>
    /// 基础资源序列化;
    /// </summary>
    public class BasicResourceSerializer : IReader<BasicResource>
    {
        internal BasicTerrainResourceSerializer BasicTerrainReader = new BasicTerrainResourceSerializer();
        internal TerrainResourceReader TerrainReader = new TerrainResourceReader();

        public BasicResource Read()
        {
            var item = new BasicResource();
            BasicTerrainResource basicTerrain = BasicTerrainReader.Read();
            item.Terrain = TerrainReader.Read(basicTerrain);
            item.Navigation = new NavigationResource(basicTerrain);
            return item;
        }
    }
}
