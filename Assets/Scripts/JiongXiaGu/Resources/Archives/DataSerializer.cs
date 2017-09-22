using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Resources.Archives
{

    /// <summary>
    /// 从存档和本地文件读取对应资源;
    /// </summary>
    /// <typeparam name="TArchive">存档序列化得到的内容;</typeparam>
    /// <typeparam name="TResult">最终转换到的内容;</typeparam>
    public abstract class DataSerializer<TSource, TArchive, TResult> : IArchiveSerializer<TResult>
    {
        protected DataSerializer()
        {
        }

        public DataSerializer(IResourceSerializer<TSource> resourceSerializer, ISerializer<TArchive> archiveSerializer, string archiveName)
        {
            ResourceSerializer = resourceSerializer;
            ArchiveDataSerializer = archiveSerializer;
            ArchiveName = archiveName;
        }

        public IResourceSerializer<TSource> ResourceSerializer { get; set; }
        public ISerializer<TArchive> ArchiveDataSerializer { get; set; }
        public string ArchiveName { get; set; }

        /// <summary>
        /// 转换为存档类型;
        /// </summary>
        protected abstract TArchive ConvertArchive(TResult result);

        /// <summary>
        /// 若不存在存档资源则传入默认TArchive;
        /// </summary>
        protected abstract TResult Convert(TSource source, TArchive archive);

        /// <summary>
        /// 序列化存档资源到...;
        /// </summary>
        public void Serialize(Archive archive, TResult result)
        {
            TArchive archiveData = ConvertArchive(result);
            Serialize(archive, archiveData);
        }

        /// <summary>
        /// 序列化存档;
        /// </summary>
        public void Serialize(Archive archive, TArchive archiveData)
        {
            string archivePath = GetFullPath(archive);
            Directory.CreateDirectory(Path.GetDirectoryName(archivePath));
            using (Stream stream = new FileStream(archivePath, FileMode.Create, FileAccess.Write))
            {
                ArchiveDataSerializer.Serialize(archiveData, stream);
            }
        }

        /// <summary>
        /// 从存档反序列化资源;
        /// </summary>
        public TResult Deserialize(Archive archive)
        {
            TSource source = ResourceSerializer.Deserialize();
            TArchive archiveData = DeserializeArchive(archive);
            return Convert(source, archiveData);
        }

        public TArchive DeserializeArchive(Archive archive)
        {
            string archivePath = GetFullPath(archive);
            if (File.Exists(archivePath))
            {
                using (Stream stream = new FileStream(archivePath, FileMode.Open, FileAccess.Read))
                {
                    return ArchiveDataSerializer.Deserialize(stream);
                }
            }
            return default(TArchive);
        }

        /// <summary>
        /// 获取到存档路径;
        /// </summary>
        string GetFullPath(Archive archive)
        {
            return Path.Combine(archive.ArchiveDirectory, ArchiveName + ArchiveDataSerializer.Extension);
        }
    }
}
