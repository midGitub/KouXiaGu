using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形之上的建筑物;
    /// </summary>
    public class Building
    {

        /// <summary>
        /// 建筑信息描述;
        /// </summary>
        public struct BuildingDescr
        {

            [XmlAttribute("name")]
            public string Name;

            /// <summary>
            /// 唯一标示(0,-1作为保留);
            /// </summary>
            [XmlAttribute("id")]
            public int ID;

            /// <summary>
            /// 高度调整贴图名;
            /// </summary>
            [XmlElement("HeightAdjustTex")]
            public string HeightAdjustTex;

        }

    }

}
