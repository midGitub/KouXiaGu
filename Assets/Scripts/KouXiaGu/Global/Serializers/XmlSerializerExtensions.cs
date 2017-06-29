using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace KouXiaGu
{


    public static class XmlSerializerExtensions
    {

        static readonly XmlQualifiedName[] namespaces = new XmlQualifiedName[]
          {
                //new XmlQualifiedName("XiaGu", "KouXiaGu"),
                new XmlQualifiedName(string.Empty, string.Empty),
          };

        static readonly XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces(namespaces);

        static readonly XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
        {
            Indent = true,
            NewLineChars = Environment.NewLine,
            NewLineOnAttributes = false,
            Encoding = Encoding.UTF8,
        };

        /// <summary>
        /// XML命名空间;
        /// </summary>
        public static XmlSerializerNamespaces XmlNamespaces
        {
            get { return xmlSerializerNamespaces; }
        }

        /// <summary>
        /// 自定义的 utf-8 格式;
        /// </summary>
        public static XmlWriterSettings XmlWriterSettings
        {
            get { return xmlWriterSettings; }
        }

        /// <summary>
        /// 序列化为 自定义的 utf-8 格式;
        /// </summary>
        public static void SerializeXiaGu(this XmlSerializer serializer, object item, string filePath, FileMode fileMode = FileMode.Create)
        {
            using (FileStream fStream = new FileStream(filePath, fileMode, FileAccess.Write))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(fStream, xmlWriterSettings))
                {
                    serializer.Serialize(xmlWriter, item, xmlSerializerNamespaces);
                }
            }
        }

        /// <summary>
        /// 序列化为 自定义的 utf-8 格式;
        /// </summary>
        public static void SerializeXiaGu(this XmlSerializer serializer, object item, Stream stream)
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
            {
                serializer.Serialize(xmlWriter, item, xmlSerializerNamespaces);
            }
        }

        /// <summary>
        /// 反序列化;
        /// </summary>
        public static object DeserializeXiaGu(this XmlSerializer serializer, string filePath, FileMode fileMode = FileMode.Open)
        {
            using (FileStream fStream = new FileStream(filePath, fileMode, FileAccess.Read))
            {
                return serializer.Deserialize(fStream);
            }
        }
    }
}
