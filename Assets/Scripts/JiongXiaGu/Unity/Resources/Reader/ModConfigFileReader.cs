using System;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{
    /// <summary>
    /// 模组配置文件读写;
    /// </summary>
    public abstract class ModConfigFileReader<T>
    {
        /// <summary>
        /// 序列化接口;
        /// </summary>
        public ISerializer<T> Serializer { get; private set; }

        /// <summary>
        /// 输出时自动创建目录;
        /// </summary>
        public bool IsAutoCreateDirectory { get; set; } = false;

        public ModConfigFileReader(ISerializer<T> serializer)
        {
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            Serializer = serializer;
        }

        /// <summary>
        /// 文件拓展名;
        /// </summary>
        string FileExtension
        {
            get { return ".xml"; }
        }

        /// <summary>
        /// 获取到完整的文件路径(不包括后缀名);
        /// </summary>
        protected abstract string GetFilePathWithoutExtension(string modDirectory);

        /// <summary>
        /// 获取到完整的文件路径;
        /// </summary>
        public string GetFilePath(string modDirectory)
        {
            string filePath = GetFilePathWithoutExtension(modDirectory);
            filePath += FileExtension;
            return filePath;
        }

        /// <summary>
        /// 从文件读取资源;
        /// </summary>
        public T Read(string modDirectory)
        {
            string filePath = GetFilePath(modDirectory);
            return Serializer.Read(filePath);
        }

        /// <summary>
        /// 输出资源到文件;
        /// </summary>
        /// <param name="isAutoCreateDirectory">输出时自动创建目录</param>
        public void Write(string modDirectory, T item, bool isAutoCreateDirectory = true)
        {
            string filePath = GetFilePath(modDirectory);
            if (isAutoCreateDirectory)
            {
                string dirName = Path.GetDirectoryName(filePath);
                Directory.CreateDirectory(dirName);
            }
            Serializer.Write(filePath, item);
        }
    }
}
