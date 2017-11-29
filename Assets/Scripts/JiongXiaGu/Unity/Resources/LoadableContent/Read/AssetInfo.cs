using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{
    [XmlRoot("AssetInfo")]
    public struct AssetInfo : IXmlSerializable
    {
        internal const string LoadModeAttributeName = "loadMode";
        internal const string AssetBundleAttributeName = "assetBundle";

        /// <summary>
        /// 读取方式,默认从文件读取;
        /// </summary>
        public LoadMode Mode { get; set; }

        /// <summary>
        /// 若从资源包读取,则置为所在的资源包名;
        /// </summary>
        public string AssetBundleName { get; set; }

        /// <summary>
        /// 若从 AssetBundle 读取,则为文件名;
        /// 若从 File 读取,则为相对路径;
        /// </summary>
        public string Name { get; set; }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Name = reader.Value;
            string loadMode = reader.GetAttribute(LoadModeAttributeName);
            Mode = (LoadMode)Enum.Parse(typeof(LoadMode), loadMode, true);

            switch (Mode)
            {
                case LoadMode.AssetBundle:
                    AssetBundleName = reader.GetAttribute(AssetBundleAttributeName);
                    break;
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteValue(Name);
            writer.WriteAttributeString(LoadModeAttributeName, Mode.ToString());

            switch (Mode)
            {
                case LoadMode.AssetBundle:
                    writer.WriteAttributeString(AssetBundleAttributeName, AssetBundleName);
                    break;
            }
        }
    }

    public enum LoadMode
    {
        Unknown,
        AssetBundle,
        File,
    }
}
