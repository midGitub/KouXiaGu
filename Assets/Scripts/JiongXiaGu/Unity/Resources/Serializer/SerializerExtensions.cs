using System;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{

    public static class SerializerExtensions
    {
        public static T Read<T>(this ISerializer<T> serializer, string file)
        {
            using (Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                return serializer.Deserialize(stream);
            }
        }

        public static void Write<T>(this ISerializer<T> serializer, string file, T item, FileMode fileMode = FileMode.Create)
        {
            using (Stream stream = new FileStream(file, fileMode, FileAccess.Write))
            {
                serializer.Serialize(stream, item);
            }
        }
    }
}
