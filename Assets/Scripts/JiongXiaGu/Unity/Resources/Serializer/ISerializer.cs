using System.IO;

namespace JiongXiaGu.Unity.Resources
{
    /// <summary>
    /// 从文件读取资源方式;
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
