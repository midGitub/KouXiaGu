using System;
using System.IO;
using System.Reflection;
using ProtoBuf;

namespace JiongXiaGu.Unity.Resources
{

    public class ProtoSerializer : ISerializer
    {
        private MethodInfo deserialize;
        private MethodInfo serialize;

        public ProtoSerializer(Type type)
        {
            var protoSerializerType = typeof(Serializer);

            deserialize = protoSerializerType.GetMethod(nameof(Serializer.Deserialize), BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(type);
            serialize = protoSerializerType.GetMethod(nameof(Serializer.Serialize), BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(type);
        }

        public object Deserialize(Stream stream)
        {
            return deserialize.Invoke(null, new object[] { stream });
        }

        public void Serialize(Stream stream, object item)
        {
            serialize.Invoke(null, new object[] { stream, item });
        }
    }

    public sealed class ProtoSerializer<T> : ISerializer<T>
    {
        public static readonly ProtoSerializer<T> Default = new ProtoSerializer<T>();

        public T Deserialize(Stream stream)
        {
            return Serializer.Deserialize<T>(stream);
        }

        public void Serialize(Stream stream, T item)
        {
            Serializer.Serialize(stream, item);
        }
    }
}
