using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源目录,线程安全;
    /// </summary>
    public class DirectoryContent : Content
    {
        private bool isUpdating;
        private bool isDisposed;
        public string DirectoryPath { get; private set; }
        public override bool IsUpdating => isUpdating;
        public override bool IsDisposed => isDisposed;
        public override bool CanRead => !isDisposed && Directory.Exists(DirectoryPath);
        public override bool CanWrite => !isDisposed && Directory.Exists(DirectoryPath);
        public override bool IsCompress => false;

        /// <summary>
        /// 指定目录;若目录不存在则返回异常;
        /// </summary>
        /// <exception cref="DirectoryNotFoundException">目录不存在,或则路径无效</exception>
        public DirectoryContent(string directory)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException(directory);

            DirectoryPath = directory;
        }

        private IContentEntry CreateEntry(string filePath, string name)
        {
            DirectoryContentEntry entry = new DirectoryContentEntry(this, name);
            return entry;
        }

        public override void Dispose()
        {
            isDisposed = true;
        }

        #region Static

        /// <summary>
        /// 创建目录或者指定目录;
        /// </summary>
        /// <param name="directory">若目录不存在则创建</param>
        public static DirectoryContent Create(string directory)
        {
            Directory.CreateDirectory(directory);
            var content = new DirectoryContent(directory);
            return content;
        }

        /// <summary>
        /// 获取到对应数据的流,在使用完毕之后需要手动释放;(推荐在using语法内使用)
        /// </summary>
        /// <exception cref="FileNotFoundException">未找到指定路径的流</exception>
        /// <exception cref="IOException">文件被占用</exception>
        public static Stream GetInputStream(string directory, string name)
        {
            string filePath = Path.Combine(directory, name);
            return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// 获取到输出流,不管是否已经存在,都返回一个空的用于写的流;(推荐在using语法内使用)
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException">文件被占用</exception>
        public static Stream GetOutputStream(string directory, string name)
        {
            string filePath = Path.Combine(directory, name);
            return new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
        }

        #endregion


        #region Read

        public override IEnumerable<IContentEntry> EnumerateEntries()
        {
            ThrowIfObjectDisposed();

            return Directory.EnumerateFiles(DirectoryPath, "*", SearchOption.AllDirectories).Select(CreateEntryFromFilePath);
        }

        public override IEnumerable<IContentEntry> EnumerateEntries(string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();

            return Directory.EnumerateFiles(DirectoryPath, searchPattern, searchOption).Select(CreateEntryFromFilePath);
        }

        public override IEnumerable<IContentEntry> EnumerateEntries(string directoryName, string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();

            string targetDirectory = Path.Combine(DirectoryPath, directoryName);
            if (Directory.Exists(targetDirectory))
            {
                return Directory.EnumerateFiles(targetDirectory, searchPattern, searchOption).Select(CreateEntryFromFilePath);
            }
            else
            {
                return new IContentEntry[0];
            }
        }

        private IContentEntry CreateEntryFromFilePath(string filePath)
        {
            string relativePath = PathHelper.GetRelativePath(DirectoryPath, filePath);
            DirectoryContentEntry entry = new DirectoryContentEntry(this, relativePath);
            return entry;
        }

        public override IEnumerable<string> EnumerateFiles()
        {
            ThrowIfObjectDisposed();

            return Directory.EnumerateFiles(DirectoryPath, "*", SearchOption.AllDirectories);
        }

        public override IEnumerable<string> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();

            return Directory.EnumerateFiles(DirectoryPath, searchPattern, SearchOption.AllDirectories);
        }

        public override IEnumerable<string> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();

            string targetDirectory = Path.Combine(DirectoryPath, directoryName);
            if (Directory.Exists(targetDirectory))
            {
                return Directory.EnumerateFiles(targetDirectory, searchPattern, searchOption);
            }
            else
            {
                return new string[0];
            }
        }

        public override IContentEntry GetEntry(string name)
        {
            ThrowIfObjectDisposed();

            string filePath = Path.Combine(DirectoryPath, name);
            if (File.Exists(filePath))
            {
                var entry = CreateEntry(filePath, name);
                return entry;
            }
            return null;
        }

        public override bool Contains(string name)
        {
            ThrowIfObjectDisposed();

            string filePath = Path.Combine(DirectoryPath, name);
            return File.Exists(filePath);
        }

        public override Stream GetInputStream(IContentEntry entry)
        {
            ThrowIfObjectDisposed();

            var directoryContentEntry = (DirectoryContentEntry)entry;
            if (File.Exists(directoryContentEntry.FilePath))
            {
                return new FileStream(directoryContentEntry.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public override Stream GetInputStream(string name)
        {
            ThrowIfObjectDisposed();

            string filePath = Path.Combine(DirectoryPath, name);
            if (File.Exists(filePath))
            {
                return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        #endregion

        #region Write

        public override void BeginUpdate()
        {
            ThrowIfObjectDisposed();

            isUpdating = true;
        }

        public override void CommitUpdate()
        {
            ThrowIfObjectDisposed();
            ThrowIfObjectNotUpdating();

            isUpdating = false;
        }

        public override IContentEntry AddOrUpdate(string name, Stream source, DateTime lastWriteTime, bool isCloseStream)
        {
            ThrowIfObjectDisposed();
            ThrowIfObjectNotUpdating();

            string filePath = Path.Combine(DirectoryPath, name);
            string directory = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(directory);

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                source.CopyTo(stream);
            }

            var entry = CreateEntry(filePath, name);
            return entry;
        }

        public override bool Remove(string name)
        {
            ThrowIfObjectDisposed();
            ThrowIfObjectNotUpdating();

            string filePath = Path.Combine(DirectoryPath, name);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override Stream GetOutputStream(string name, out IContentEntry entry)
        {
            ThrowIfObjectDisposed();
            ThrowIfObjectNotUpdating();

            string filePath = Path.Combine(DirectoryPath, name);
            string directory = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(directory);
            entry = CreateEntry(filePath, name);
            return new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
        }

        #endregion

        private class DirectoryContentEntry : IContentEntry
        {
            public DirectoryContent Parent { get; private set; }
            public string Name { get; private set; }
            public string FilePath => Path.Combine(Parent.DirectoryPath, Name);
            public DateTime LastWriteTime => File.GetLastWriteTime(FilePath);

            public DirectoryContentEntry(DirectoryContent parent, string name)
            {
                Parent = parent;
                Name = NormalizePath(name);
            }

            public Stream OpenRead()
            {
                return new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }

            public Stream OpenWrite()
            {
                return new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
        }
    }
}
