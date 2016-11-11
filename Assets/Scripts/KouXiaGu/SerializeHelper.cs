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

        /// <summary>
        /// 序列化;
        /// </summary>
        public static void Serialize_Xml<T>(string filePath, T t)
        {
            Stream fStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            Serialize_Xml(fStream, t);
            fStream.Close();
        }

        /// <summary>
        /// 序列化;
        /// </summary>
        public static void Serialize_Xml<T>(Stream stream, T t)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, t);
        }

        /// <summary>
        /// 反序列化;
        /// </summary>
        public static T Deserialize_Xml<T>(string filePath)
        {
            Stream fStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
            T t = Deserialize_Xml<T>(fStream);
            fStream.Close();
            return t;
        }

        /// <summary>
        /// 反序列化;
        /// </summary>
        public static T Deserialize_Xml<T>(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }

        #endregion

        #region ProtoBuf

        public static void Serialize_ProtoBuf<T>(this T t, string filePath)
        {
            using (Stream fStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                Serializer.Serialize(fStream, t);
            }
        }

        public static T Deserialize_ProtoBuf<T>(this string filePath)
        {
            Stream fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            T t = Serializer.Deserialize<T>(fStream);
            fStream.Close();
            return t;
        }

        public static T Deserialize_ProtoBuf<T>(this Stream stream)
        {
            return Serializer.Deserialize<T>(stream);
        }

        /// <summary>
        /// 序列化;
        /// </summary>
        public static void Serialize_ProtoBuf<T>(string filePath, T t)
        {
            Stream fStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            Serializer.Serialize(fStream, t);
            fStream.Close();
        }

        /// <summary>
        /// 序列化;
        /// </summary>
        public static void Serialize_ProtoBuf<T>(Stream stream, T t)
        {
            Serializer.Serialize(stream, t);
        }

        #endregion

    }

}
