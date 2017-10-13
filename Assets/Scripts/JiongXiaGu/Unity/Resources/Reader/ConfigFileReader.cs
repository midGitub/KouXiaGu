using System.IO;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 用于固定的单个文件读写;
    /// </summary>
    /// <typeparam name="T">返回的内容</typeparam>
    public abstract class ConfigFileReader<T>
    {
        /// <summary>
        /// 序列化接口;
        /// </summary>
        public ISerializer<T> Serializer { get; private set; }

        public ConfigFileReader(ISerializer<T> serializer)
        {
            Serializer = serializer;
        }

        /// <summary>
        /// 文件拓展名;
        /// </summary>
        string FileExtension
        {
            get { return Serializer.FileExtension; }
        }

        /// <summary>
        /// 获取到完整的文件路径(不包括后缀名);
        /// </summary>
        public abstract string GetFilePathWithoutExtension();

        /// <summary>
        /// 获取到完整的文件路径;
        /// </summary>
        public string GetFilePath()
        {
            string filePath = GetFilePathWithoutExtension();
            filePath += FileExtension;
            return filePath;
        }

        /// <summary>
        /// 从文件读取资源;
        /// </summary>
        public T Read()
        {
            string path = GetFilePath();
            var item = Serializer.Read(path);
            return item;
        }

        /// <summary>
        /// 输出资源到文件;
        /// </summary>
        public void Write(T item, bool isAutoCreateDirectory = true)
        {
            string filePath = GetFilePath();
            if (isAutoCreateDirectory)
            {
                string dirName = Path.GetDirectoryName(filePath);
                Directory.CreateDirectory(dirName);
            }
            Serializer.Write(filePath, item);
        }
    }
}
