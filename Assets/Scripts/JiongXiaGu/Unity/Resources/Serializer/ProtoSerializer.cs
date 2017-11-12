﻿using System.IO;

namespace JiongXiaGu.Unity.Resources
{
    public sealed class ProtoSerializer<T> : ISerializer<T>
    {
        internal const string fileExtension = ".data";
        public static readonly ProtoSerializer<T> Default = new ProtoSerializer<T>();

        private ProtoSerializer()
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