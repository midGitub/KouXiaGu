﻿using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 表示资源;
    /// </summary>
    public struct AssetInfo : IXmlSerializable
    {
        internal const AssetLoadModes DefaultLoadMode = AssetLoadModes.File;
        internal const string LoadModeAttribute = "from";
        internal const string AssetBundleNameAttribute = "assetBundle";

        /// <summary>
        /// 读取方式,默认从文件读取;
        /// </summary>
        public AssetLoadModes From { get; private set; }

        /// <summary>
        /// 若为 AssetBundle 的资源,则为 AssetBundleName,否则为null;
        /// </summary>
        public AssetPath AssetBundleName { get; private set; }

        /// <summary>
        /// 若从 AssetBundle 读取,则为文件名,忽略拓展名;
        /// 若从 File 读取,则为相对路径;
        /// </summary>
        public AssetPath Name { get; private set; }

        public AssetInfo(AssetPath name) : this()
        {
            From = AssetLoadModes.File;
            Name = name;
        }

        public AssetInfo(AssetPath assteBundleName, string name) : this()
        {
            From = AssetLoadModes.AssetBundle;
            AssetBundleName = assteBundleName;
            Name = name;

            if (Name.IsReferencePath())
            {
                throw new ArgumentException();
            }
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Name = reader.Value;
            string fromStr = reader.GetAttribute(LoadModeAttribute);
            try
            {
                From = (AssetLoadModes)Enum.Parse(typeof(AssetLoadModes), fromStr, true);

                switch (From)
                {
                    case AssetLoadModes.AssetBundle:
                        AssetBundleName = reader.GetAttribute(AssetBundleNameAttribute);
                        break;

                    default:
                        break;
                }
            }
            catch (ArgumentException)
            {
                From = AssetLoadModes.Unknown;
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            switch (From)
            {
                case AssetLoadModes.File:
                    writer.WriteAttributeString(LoadModeAttribute, From.ToString());
                    writer.WriteValue(Name);
                    break;

                case AssetLoadModes.AssetBundle:
                    writer.WriteAttributeString(LoadModeAttribute, From.ToString());
                    writer.WriteAttributeString(AssetBundleNameAttribute, AssetBundleName.Name);
                    writer.WriteValue(Name);
                    break;

                default:
                    throw new NotSupportedException(From.ToString());
            }
        }
    }
}