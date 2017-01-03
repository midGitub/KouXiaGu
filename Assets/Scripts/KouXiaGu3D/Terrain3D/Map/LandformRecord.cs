using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 记录地图所有引用的地形;
    /// </summary>
    [Serializable, XmlType("Record")]
    public struct LandformRecord
    {
        [XmlAttribute("id")]
        public int ID;

        [XmlAttribute("count")]
        public int Count;

        public LandformRecord(int id, int count)
        {
            this.ID = id;
            this.Count = count;
        }
    }

}
