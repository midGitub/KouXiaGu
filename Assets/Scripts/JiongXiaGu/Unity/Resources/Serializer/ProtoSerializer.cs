using System;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{

    [Obsolete("似乎非线程安全")]
    public sealed class ProtoSerializer<T> : ISerializer<T>
    {
        internal const string fileExtension = ".data";
        public static readonly ProtoSerializer<T> Default = new ProtoSerializer<T>();

        public ProtoSerializer()
        {
        }

        public string FileExtension
        {
            get { return fileExtension; }
        }

        public T Deserialize(Stream stream)
        {
            return ProtoBuf.Serializer.Deserialize<T>(stream);
        }

        public void Serialize(Stream stream, T item)
        {
            ProtoBuf.Serializer.Serialize(stream, item);
        }
    }
}
