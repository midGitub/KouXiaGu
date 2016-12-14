using System;
using System.IO;
using System.Xml.Serialization;
using ProtoBuf;

namespace KouXiaGu
{

    /// <summary>
    /// 序列化工具类;
    /// </summary>
    public static class SerializeHelper
    {

        #region XML

        public static void Serialize(this XmlSerializer serializer, string filePath, object item, FileMode fileMode = FileMode.Create)
        {
            using (Stream fStream = new FileStream(filePath, fileMode, FileAccess.Write))
            {
                serializer.Serialize(fStream, item);
            }
        }

        public static object Deserialize(this XmlSerializer serializer, string filePath, FileMode fileMode = FileMode.Open)
        {
            using (Stream fStream = new FileStream(filePath, fileMode, FileAccess.Read))
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
