using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KouXiaGu.World.Resources
{

    /// <summary>
    /// 地形资源信息;
    /// </summary>
    [XmlRoot("LandformInfo")]
    public class LandformResourceInfo
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("Terrain")]
        public RectTerrain.LandformResourceInfo Terrain { get; set; }
    }

    /// <summary>
    /// 地形资源;
    /// </summary>
    public class LandformResource
    {
        public string Name { get; private set; }
        public RectTerrain.LandformResource Terrain { get; private set; }
    }
}
