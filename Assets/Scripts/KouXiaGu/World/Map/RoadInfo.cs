using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 道路信息;
    /// </summary>
    public class RoadType
    {
        public int ID { get; private set; }
        public TerrainRoad Terrain { get; private set; }
    }

    /// <summary>
    /// 道路信息;
    /// </summary>
    [XmlType("RoadInfo")]
    public struct RoadInfo
    {
        [XmlAttribute("id")]
        public int ID;

        [XmlAttribute("name")]
        public string Name;

        [XmlElement("Terrain")]
        public TerrainRoadInfo Terrain;
    }

    /// <summary>
    /// 读取道路资源信息;
    /// </summary>
    public class RoadTypeReader
    {
        public RoadTypeReader()
        {
        }

        public RoadType Read(RoadInfo info)
        {
            throw new NotImplementedException();
        }
    }

}
