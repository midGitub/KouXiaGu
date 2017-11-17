using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 可读资源目录;
    /// </summary>
    public class LoadableContentDirectory : ILoadableContentConstruct
    {
        public DirectoryInfo DirectoryInfo { get; private set; }

        public bool IsLoadable
        {
            get { return true; }
        }

        public bool Exists
        {
            get { return DirectoryInfo.Exists; }
        }

        public LoadableContentDirectory(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
                throw new ArgumentNullException(nameof(directoryInfo));

            DirectoryInfo = directoryInfo;
        }

        public void Load()
        {
            return;
        }

        public void Unload()
        {
            return;
        }

        public IEnumerable<ILoadableEntry> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return DirectoryInfo.EnumerateFiles(searchPattern, searchOption).Select(delegate (FileInfo fileInfo)
            {
                string name = PathHelper.GetRelativePath(DirectoryInfo.FullName, fileInfo.FullName);
                return new FileEntry(fileInfo, name);
            });
        }

        public IEnumerable<ILoadableEntry> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            string directory = Path.Combine(DirectoryInfo.FullName, directoryName);
            var directoryInfo = new DirectoryInfo(directory);

            if (directoryInfo.Exists)
            {
                return directoryInfo.EnumerateFiles(searchPattern, searchOption).Select(delegate (FileInfo fileInfo)
                {
                    string name = PathHelper.GetRelativePath(directoryInfo.FullName, fileInfo.FullName);
                    return new FileEntry(fileInfo, name);
                });
            }
            else
            {
                return new ILoadableEntry[0];
            }
        }

        private class FileEntry : ILoadableEntry
        {
            public FileInfo FileInfo { get; private set; }
            public string Name { get; private set; }

            public FileEntry(FileInfo fileInfo, string name)
            {
                FileInfo = fileInfo;
                Name = name;
            }

            public Stream GetStream()
            {
                return FileInfo.OpenRead();
            }
        }
    }

    /// <summary>
    /// 可读资源Zip文件;
    /// </summary>
    public class LoadableContentZip
    {
        public FileInfo ZipFileInfo { get; private set; }
        private ZipFile zipFile;

        public LoadableContentZip()
        {
        }
    }

    /// <summary>
    /// 可读资源结构;
    /// </summary>
    public interface ILoadableContentConstruct
    {
        /// <summary>
        /// 是否可读?
        /// </summary>
        bool IsLoadable { get; }

        /// <summary>
        /// 是否存在?
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// 加载资源;
        /// </summary>
        void Load();

        /// <summary>
        /// 卸载资源;
        /// </summary>
        void Unload();

        /// <summary>
        /// 枚举所有目录信息;
        /// </summary>
        IEnumerable<ILoadableEntry> EnumerateFiles(string searchPattern, SearchOption searchOption);

        /// <summary>
        /// 枚举所有目录信息;
        /// </summary>
        IEnumerable<ILoadableEntry> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption);
    }

    public interface ILoadableEntry
    {
        /// <summary>
        /// 名称;
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 获取到只读的流;
        /// </summary>
        Stream GetStream();
    }
}
