using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 地形信息;
    /// </summary>
    [XmlType("Landform")]
    public struct LandformInfo
    {
        [XmlAttribute("id")]
        public int ID;

        [XmlAttribute("name")]
        public string Name;

    }

    public class LandformInfoXmlReader : IReader<LandformInfo[]>, IReader<Dictionary<int, LandformInfo>>
    {
        public LandformInfo[] Read()
        {
            throw new NotImplementedException();
        }

        Dictionary<int, LandformInfo> IReader<Dictionary<int, LandformInfo>>.Read()
        {
            throw new NotImplementedException();
        }
    }

}
