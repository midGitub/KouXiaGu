using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KouXiaGu.Resources.Archive
{

    /// <summary>
    /// 可存档资源读取;
    /// </summary>
    public interface IArchiveSerializer<T>
    {
        void Serialize(ArchiveInfo archive, T result);
        T Deserialize(ArchiveInfo archive);
    }

    /// <summary>
    /// 读取资源;
    /// </summary>
    /// <typeparam name="TArchive">存档序列化得到的内容;</typeparam>
    /// <typeparam name="TResult">最终转换到的内容;</typeparam>
    public abstract class ArchiveSerializer<TSource, TArchive, TResult> : IArchiveSerializer<TResult>
    {
        protected ArchiveSerializer()
        {
        }

        public ArchiveSerializer(IResourceSerializer<TSource> resourceSerializer, ISerializer<TArchive> archiveSerializer, string archiveName)
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
        public void Serialize(ArchiveInfo archive, TResult result)
        {
            TArchive archiveData = ConvertArchive(result);
            Serialize(archive, archiveData);
        }

        /// <summary>
        /// 序列化存档;
        /// </summary>
        public void Serialize(ArchiveInfo archive, TArchive archiveData)
        {
            string archivePath = GetFullPath(archive);
            using (Stream stream = new FileStream(archivePath, FileMode.Create, FileAccess.Write))
            {
                ArchiveDataSerializer.Serialize(archiveData, stream);
            }
        }

        /// <summary>
        /// 从存档反序列化资源;
        /// </summary>
        public TResult Deserialize(ArchiveInfo archive)
        {
            TSource source = ResourceSerializer.Deserialize();
            TArchive archiveData = DeserializeArchive(archive);
            return Convert(source, archiveData);
        }

        public TArchive DeserializeArchive(ArchiveInfo archive)
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

        string GetFullPath(ArchiveInfo archive)
        {
            return Path.Combine(archive.Directory, ArchiveName + ArchiveDataSerializer.Extension);
        }
    }
}
