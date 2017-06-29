using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Resources
{

    /// <summary>
    /// 从文件读取资源方式;
    /// </summary>
    public interface IFileSerializer<T>
    {
        T Read(string filePath);
        void Write(T item, string filePath, FileMode fileMode);
    }

    public sealed class ProtoFileSerializer<T> : IFileSerializer<T>
    {
        public static readonly ProtoFileSerializer<T> Default = new ProtoFileSerializer<T>();

        public T Read(string filePath)
        {
            using (Stream fStream = new FileStream(filePath, FileMode.Open))
            {
                return ProtoBuf.Serializer.Deserialize<T>(fStream);
            }
        }

        public void Write(T item, string filePath, FileMode fileMode)
        {
            using (Stream fStream = new FileStream(filePath, fileMode))
            {
                ProtoBuf.Serializer.Serialize(fStream, item);
            }
        }
    }

    public sealed class XmlFileSerializer<T> : IFileSerializer<T>
    {
        public XmlFileSerializer()
        {
            Serializer = new XmlSerializer(typeof(T));
        }

        public XmlSerializer Serializer { get; private set; }

        public T Read(string filePath)
        {
            T item = (T)Serializer.DeserializeXiaGu(filePath);
            return item;
        }

        public void Write(T item, string filePath, FileMode fileMode)
        {
            Serializer.SerializeXiaGu(item, filePath, fileMode);
        }
    }

}
