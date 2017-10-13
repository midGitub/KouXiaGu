using System.IO;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Resources
{
    /// <summary>
    /// XML 序列化;
    /// </summary>
    public sealed class XmlSerializer<T> : ISerializer<T>
    {
        internal const string fileExtension = ".xml";
        public XmlSerializer Serializer { get; private set; }

        public XmlSerializer()
        {
            Serializer = new XmlSerializer(typeof(T));
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
