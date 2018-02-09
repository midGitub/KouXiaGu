using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;

namespace JiongXiaGu.Unity.Resources.Archives
{

    public abstract class Archive<TEntry>
        where TEntry : IArchiveEntry
    {
        /// <summary>
        /// 枚举所有文件;
        /// </summary>
        public abstract IEnumerable<TEntry> EnumerateEntries();

        /// <summary>
        /// 添加文件;
        /// </summary>
        public abstract TEntry Add(string name, Stream source, bool closeStream = true);

        /// <summary>
        /// 移除文件;
        /// </summary>
        public abstract bool Remove(string name);

        /// <summary>
        /// 保存内容;
        /// </summary>
        public abstract void Save();
    }

    public interface IArchiveEntry
    {
        string Name { get; }
        Stream OpenEntryStream();
    }

    //public class MemoryArchive : Archive<MemoryArchiveEntry>
    //{
    //    public IEnumerable<MemoryArchiveEntry> EnumerateFiles()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class MemoryArchiveEntry : ArchiveEntry
    //{

    //}

    //public class DirectoryArchive : Archive
    //{

    //}

    //public class ZipArchive : Archive
    //{

    //}
}
