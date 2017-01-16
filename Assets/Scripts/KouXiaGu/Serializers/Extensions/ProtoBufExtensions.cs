using System.IO;
using ProtoBuf;

namespace KouXiaGu
{

    /// <summary>
    /// 序列化工具类;
    /// </summary>
    public static class ProtoBufExtensions
    {

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

    }

}
