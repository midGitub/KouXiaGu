using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.Navigation
{

    [XmlType("Landform")]
    public struct NavLandformInfo
    {
        /// <summary>
        /// 地形标签,允许多个,以","分隔;
        /// </summary>
        [XmlAttribute("tag")]
        public string Tags;
    }
}
