using System.IO;
using ProtoBuf;

namespace KouXiaGu
{

    /// <summary>
    /// 序列化工具类;
    /// </summary>
    public static class ProtoBufExtensions
    {

        public static void Serialize<T>(string filePath, T t, FileMode fileMode = FileMode.Create)
        {
            using (Stream fStream = new FileStream(filePath, fileMode))
            {
                Serializer.Serialize(fStream, t);
            }
        }

        public static T Deserialize<T>(string filePath, FileMode fileMode = FileMode.Open)
        {
            T item;
            using (Stream fStream = new FileStream(filePath, fileMode))
            {
                item = Serializer.Deserialize<T>(fStream);
            }
            return item;
        }

        /// <summary>
        /// 将路径文件更改为指定的后缀名;
        /// </summary>
        public static void SerializeXiaGu<T>(string filePath, T t, FileMode fileMode = FileMode.Create)
        {
            using (Stream fStream = new FileStream(filePath, fileMode))
            {
                Serializer.Serialize(fStream, t);
            }
        }

        /// <summary>
        /// 将路径文件更改为指定的后缀名;
        /// </summary>
        public static T DeserializeXiaGu<T>(string filePath, FileMode fileMode = FileMode.Open)
        {
            T item;
            using (Stream fStream = new FileStream(filePath, fileMode))
            {
                item = Serializer.Deserialize<T>(fStream);
            }
            return item;
        }


    }

}
