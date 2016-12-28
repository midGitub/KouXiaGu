using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using ProtoBuf;

namespace KouXiaGu
{

    /// <summary>
    /// 序列化工具类;
    /// </summary>
    public static class SerializeExtensions
    {

        #region XML

        static readonly XmlQualifiedName[] namespaces = new XmlQualifiedName[]
            {
                new XmlQualifiedName("XiaGu", "KouXiaGu"),
            };

        static readonly XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces(namespaces);

        static readonly XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
        {
            Indent = true,
            NewLineChars = Environment.NewLine,
            NewLineOnAttributes = false,
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
        public static void SerializeXiaGu(this XmlSerializer serializer, string filePath, object item, FileMode fileMode = FileMode.Create)
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
        /// 反序列化;
        /// </summary>
        public static object DeserializeXiaGu(this XmlSerializer serializer, string filePath, FileMode fileMode = FileMode.Open)
        {
            using (FileStream fStream = new FileStream(filePath, fileMode, FileAccess.Read))
            {
                return serializer.Deserialize(fStream);
            }
        }

        #endregion

        #region ProtoBuf


        public static void SerializeProtoBuf<T>(Stream stream, T t)
        {
            Serializer.Serialize(stream, t);
        }

        public static void SerializeProtoBuf<T>(string filePath, T t, FileMode fileMode = FileMode.Create)
        {
            using (Stream fStream = new FileStream(filePath, fileMode))
            {
                SerializeProtoBuf(fStream, t);
            }
        }


        public static T DeserializeProtoBuf<T>(Stream stream)
        {
            return Serializer.Deserialize<T>(stream);
        }

        public static T DeserializeProtoBuf<T>(string filePath, FileMode fileMode = FileMode.Open)
        {
            T item;
            using (Stream fStream = new FileStream(filePath, fileMode))
            {
                item = DeserializeProtoBuf<T>(fStream);
            }
            return item;
        }

        #endregion

    }

}
