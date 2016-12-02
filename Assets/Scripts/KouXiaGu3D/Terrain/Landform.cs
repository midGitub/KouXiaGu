using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Xml.Serialization;

namespace KouXiaGu.Terrain
{

    /// <summary>
    /// 地貌定义;
    /// </summary>
    public struct Landform
    {

        /// <summary>
        /// 地形名;
        /// </summary>
        [XmlAttribute]
        public string name;

        /// <summary>
        /// 地形唯一标示;
        /// </summary>
        [XmlAttribute]
        public int terrainID;

        /// <summary>
        /// 漫反射贴图;
        /// </summary>
        [NonSerialized]
        public Texture diffuse;

        /// <summary>
        /// 高度贴图;
        /// </summary>
        [NonSerialized]
        public Texture height;

        /// <summary>
        /// 混合贴图;
        /// </summary>
        [NonSerialized]
        public Texture mixer;



    }

}
