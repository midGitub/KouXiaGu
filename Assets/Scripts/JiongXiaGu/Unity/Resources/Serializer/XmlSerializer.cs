using System;
using System.IO;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 经过包装的 System.Xml.Serialization.XmlSerializer;
    /// </summary>
    public class XmlSerializer : ISerializer
    {
        public System.Xml.Serialization.XmlSerializer Serializer { get; private set; }
        public string FileExtension => ".xml";

        public XmlSerializer(Type type)
        {
            Serializer = new System.Xml.Serialization.XmlSerializer(type);
        }

        public void Serialize(Stream stream, object item)
        {
            Serializer.SerializeXiaGu(stream, item);
        }

        public object Deserialize(Stream stream)
        {
            return Serializer.Deserialize(stream);
        }
    }

    /// <summary>
    /// XML 序列化;
    /// </summary>
    public sealed class XmlSerializer<T> : ISerializer<T>
    {
        internal const string fileExtension = ".xml";
        public System.Xml.Serialization.XmlSerializer Serializer { get; private set; }

        public XmlSerializer()
        {
            Serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        }

        public string FileExtension
        {
            get { return fileExtension; }
        }

        public T Deserialize(Stream stream)
        {
            T item = (T)Serializer.Deserialize(stream);
            return item;
        }

        public void Serialize(Stream stream, T item)
        {
            Serializer.SerializeXiaGu(stream, item);
        }
    }
}
