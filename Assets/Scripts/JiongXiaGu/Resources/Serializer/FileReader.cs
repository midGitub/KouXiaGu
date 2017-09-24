using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Resources
{

    /// <summary>
    /// 用于固定的单个文件序列化;
    /// </summary>
    /// <typeparam name="T">返回的内容</typeparam>
    public abstract class FileReader<T> : IResourceReader<T>
    {
        /// <summary>
        /// 序列化接口;
        /// </summary>
        public ISerializer<T> Serializer { get; set; }

        /// <summary>
        /// 文件拓展名;
        /// </summary>
        public string FileExtension
        {
            get { return Serializer.Extension; }
        }

        public FileReader(ISerializer<T> serializer)
        {
            Serializer = serializer;
        }

        /// <summary>
        /// 输出资源到文件;
        /// </summary>
        public void Serialize(T item)
        {
            string path = GetFilePath();
            using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                Serializer.Serialize(item, stream);
            }
        }

        /// <summary>
        /// 从文件读取资源;
        /// </summary>
        public T Deserialize()
        {
            string path = GetFilePath();
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return Serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// 获取到完整的文件路径(包括后缀名);
        /// </summary>
        public abstract string GetFilePath();
    }
}
