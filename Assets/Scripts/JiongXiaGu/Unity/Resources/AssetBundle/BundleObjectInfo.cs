//using System;
//using System.Xml;
//using System.Xml.Schema;
//using System.Xml.Serialization;

//namespace JiongXiaGu.Unity.Resources
//{

//    /// <summary>
//    /// AssetBundle 资源信息;
//    /// Xml节点内容例子: Core:terrain:HeightMap 
//    /// 表示[来自 Core 资源目录 从名为 terrain 的资源包 读取名为 HeightMap 的资源]
//    /// 若不指定资源目录,则默认从本身资源目录获取;
//    /// </summary>
//    [XmlRoot("BundleObject")]
//    public struct BundleObjectInfo : IXmlSerializable
//    {
//        /// <summary>
//        /// 模组名;
//        /// </summary>
//        [XmlIgnore]
//        public string ModName { get; set; }

//        /// <summary>
//        /// 资源包名;
//        /// </summary>
//        [XmlIgnore]
//        public string BundleName { get; set; }

//        /// <summary>
//        /// 资源名;
//        /// </summary>
//        [XmlIgnore]
//        public string ResName { get; set; }

//        public BundleObjectInfo(string bundleName, string resName) : this()
//        {
//            if(string.IsNullOrEmpty(bundleName))
//                throw new ArgumentNullException(nameof(bundleName));
//            if (string.IsNullOrEmpty(resName))
//                throw new ArgumentNullException(nameof(resName));

//            BundleName = bundleName;
//            ResName = resName;
//        }

//        public BundleObjectInfo(string modName, string bundleName, string resName) : this()
//        {
//            if (string.IsNullOrEmpty(modName))
//                throw new ArgumentNullException(nameof(modName));
//            if (string.IsNullOrEmpty(bundleName))
//                throw new ArgumentNullException(nameof(bundleName));
//            if (string.IsNullOrEmpty(resName))
//                throw new ArgumentNullException(nameof(resName));

//            ModName = modName;
//            BundleName = bundleName;
//            ResName = resName;
//        }

//        XmlSchema IXmlSerializable.GetSchema()
//        {
//            return null;
//        }

//        private const char Separator = ':';

//        void IXmlSerializable.ReadXml(XmlReader reader)
//        {
//            string path = reader.ReadElementContentAsString();
//            string[] values = path.Split(Separator);

//            if (values.Length == 2)
//            {
//                BundleName = values[0];
//                ResName = values[1];
//            }
//            else if (values.Length >= 3)
//            {
//                ModName = values[0];
//                BundleName = values[1];
//                ResName = values[2];
//            }
//            else
//            {
//                ModName = string.Empty;
//                BundleName = string.Empty;
//                ResName = string.Empty;
//            }
//        }

//        void IXmlSerializable.WriteXml(XmlWriter writer)
//        {
//            string value = string.Empty;
//            if (!string.IsNullOrWhiteSpace(ModName))
//            {
//                value = ModName + Separator;
//            }
//            if (!string.IsNullOrWhiteSpace(BundleName) && !string.IsNullOrWhiteSpace(ResName))
//            {
//                value += BundleName + Separator + ResName;
//                writer.WriteValue(value);
//            }
//        }
//    }
//}
