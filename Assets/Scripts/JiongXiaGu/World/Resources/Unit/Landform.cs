using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JiongXiaGu.World.Resources
{

    /// <summary>
    /// 地形资源;
    /// </summary>
    [XmlRoot("LandformInfo")]
    public class LandformResource
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
    }
}
