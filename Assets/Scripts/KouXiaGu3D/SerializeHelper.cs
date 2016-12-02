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

        public static void SerializeXml<T>(Stream stream, T t)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, t);
        }

        public static void SerializeXml<T>(string filePath, T t, FileMode fileMode = FileMode.Create)
        {
            using (Stream fStream = new FileStream(filePath, fileMode))
            {
                SerializeXml(fStream, t);
            }
        }


        public static T DeserializeXml<T>(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }

        public static T DeserializeXml<T>(string filePath , FileMode fileMode = FileMode.Open)
        {
            T t;
            using (Stream fStream = new FileStream(filePath, fileMode))
            {
                t = DeserializeXml<T>(fStream);
            }
            return t;
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
