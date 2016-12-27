﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 道路资源定义;
    /// </summary>
    [XmlType("Road")]
    public struct RoadDescr
    {

        static readonly XmlSerializer arraySerializer = new XmlSerializer(typeof(RoadDescr[]));

        public static XmlSerializer ArraySerializer
        {
            get { return arraySerializer; }
        }

        /// <summary>
        /// 唯一标示(0,-1作为保留);
        /// </summary>
        [XmlAttribute("id")]
        public int ID { get; set; }

        /// <summary>
        /// 定义名,允许为空;
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 高度调整贴图名;
        /// </summary>
        [XmlElement("HeightTex")]
        public string HeightAdjustTex { get; set; }

        /// <summary>
        /// 高度调整的权重贴图;
        /// </summary>
        [XmlElement("HeightBlendTex")]
        public string HeightAdjustBlendTex { get; set; }

        /// <summary>
        /// 漫反射贴图名;
        /// </summary>
        [XmlElement("DiffuseTex")]
        public string DiffuseTex { get; set; }

        /// <summary>
        /// 漫反射混合贴图名;
        /// </summary>
        [XmlElement("DiffuseBlendTex")]
        public string DiffuseBlendTex { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is RoadDescr))
                return false;
            return ID == ((RoadDescr)obj).ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override string ToString()
        {
            return "[ID:" + ID.ToString() + ",Name:" + Name.ToString() + "]";
        }

    }


}
