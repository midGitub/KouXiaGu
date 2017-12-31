using System.IO;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 序列化接口;
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// 拓展名;
        /// </summary>
        string FileExtension { get; }

        /// <summary>
        /// 序列化;
        /// </summary>
        void Serialize(Stream stream, object item);

        /// <summary>
        /// 反序列化;
        /// </summary>
        object Deserialize(Stream stream);
    }

    /// <summary>
    /// 序列化接口;
    /// </summary>
    public interface ISerializer<T>
    {
        /// <summary>
        /// 拓展名;
        /// </summary>
        string FileExtension { get; }

        void Serialize(Stream stream, T item);
        T Deserialize(Stream stream);
    }
}
