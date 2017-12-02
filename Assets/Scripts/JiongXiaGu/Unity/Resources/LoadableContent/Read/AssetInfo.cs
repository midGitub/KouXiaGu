using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Resources
{
    /// <summary>
    /// 表示资源;
    /// </summary>
    [XmlRoot("AssetInfo")]
    public struct AssetInfo : IXmlSerializable
    {
        internal const LoadMode DefaultLoadMode = LoadMode.AssetBundle;
        internal const string LoadModeAttributeName = "from";

        /// <summary>
        /// 读取方式,默认从文件读取;
        /// </summary>
        public LoadMode From { get; set; }

        /// <summary>
        /// 若从 AssetBundle 读取,则为文件名;
        /// 若从 File 读取,则为相对路径;
        /// </summary>
        public string Name { get; set; }

        public AssetInfo(string name)
        {
            From = DefaultLoadMode;
            Name = name;
        }

        public AssetInfo(LoadMode from, string name)
        {
            From = from;
            Name = name;
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Name = reader.Value;
            string loadMode = reader.GetAttribute(LoadModeAttributeName);
            if (string.IsNullOrEmpty(loadMode))
            {
                From = DefaultLoadMode;
            }
            else
            {
                From = (LoadMode)Enum.Parse(typeof(LoadMode), loadMode, true);
            }
            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteValue(Name);
            writer.WriteAttributeString(LoadModeAttributeName, From.ToString());
        }
    }

    public enum LoadMode
    {
        Unknown,
        AssetBundle,
        File,
    }
}
