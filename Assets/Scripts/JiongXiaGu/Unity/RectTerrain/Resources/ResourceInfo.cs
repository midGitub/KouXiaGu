using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.RectTerrain.Resources
{


    [XmlRoot("ResourceInfo")]
    public struct ResourceInfo : IXmlSerializable
    {
        /// <summary>
        /// 读取方式,默认从文件读取;
        /// </summary>
        [XmlAttribute("loadMode")]
        public LoadMode LoadMode { get; set; }

        public string AssetBundleName { get; set; }

        /// <summary>
        /// 若从 AssetBundle 读取,则为文件名;
        /// 若从 File 读取,则为相对路径;
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }

    public enum LoadMode
    {
        AssetBundle,
        File,
    }
}
