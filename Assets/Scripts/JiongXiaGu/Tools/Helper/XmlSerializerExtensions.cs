using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace JiongXiaGu
{


    public static class XmlSerializerExtensions
    {

        static readonly XmlQualifiedName[] namespaces = new XmlQualifiedName[]
          {
                new XmlQualifiedName(string.Empty, string.Empty),
          };

        /// <summary>
        /// XML命名空间;
        /// </summary>
        public static XmlSerializerNamespaces XmlNamespaces { get; private set; } = new XmlSerializerNamespaces(namespaces);

        /// <summary>
        /// 自定义的 utf-8 格式;
        /// </summary>
        public static XmlWriterSettings XmlWriterSettings { get; private set; } = new XmlWriterSettings()
        {
            Indent = true,
            NewLineChars = Environment.NewLine,
            NewLineOnAttributes = false,
            Encoding = Encoding.UTF8,
        };

        /// <summary>
        /// 序列化为 自定义的 utf-8 格式;
        /// </summary>
        public static void SerializeXiaGu(this XmlSerializer serializer, Stream stream, object item)
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(stream, XmlWriterSettings))
            {
                serializer.Serialize(xmlWriter, item, XmlNamespaces);
            }
        }



        /// <summary>
        /// 序列化为 自定义的 utf-8 格式;
        /// </summary>
        [Obsolete]
        public static void SerializeXiaGu(this XmlSerializer serializer, object item, Stream stream)
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(stream, XmlWriterSettings))
            {
                serializer.Serialize(xmlWriter, item, XmlNamespaces);
            }
        }

        /// <summary>
        /// 序列化为 自定义的 utf-8 格式;
        /// </summary>
        [Obsolete]
        public static void SerializeXiaGu(this XmlSerializer serializer, object item, string filePath, FileMode fileMode = FileMode.Create)
        {
            using (FileStream fStream = new FileStream(filePath, fileMode, FileAccess.Write))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(fStream, XmlWriterSettings))
                {
                    serializer.Serialize(xmlWriter, item, XmlNamespaces);
                }
            }
        }

        /// <summary>
        /// 反序列化;
        /// </summary>
        [Obsolete]
        public static object DeserializeXiaGu(this XmlSerializer serializer, string filePath, FileMode fileMode = FileMode.Open)
        {
            using (FileStream fStream = new FileStream(filePath, fileMode, FileAccess.Read))
            {
                return serializer.Deserialize(fStream);
            }
        }
    }
}
