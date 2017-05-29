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
        TerrainResourcesReader TerrainReader = new TerrainResourcesReader();

        public BasicResource Read()
        {
            var item = new BasicResource();
            item.Terrain = TerrainReader.Read();
            return item;
        }
    }
}
