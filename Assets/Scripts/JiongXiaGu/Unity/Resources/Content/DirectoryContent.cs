using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源目录;
    /// </summary>
    public class DirectoryContent : Content
    {
        private bool isUpdating;
        private bool isDisposed;
        public DirectoryInfo DirectoryInfo { get; private set; }
        public override bool IsUpdating => isUpdating;
        public override bool IsDisposed => isDisposed;
        public override bool CanRead => !isDisposed && DirectoryInfo.Exists;
        public override bool CanWrite => !isDisposed && DirectoryInfo.Exists;
        public override bool IsCompress => false;

        /// <summary>
        /// 创建目录或者指定目录;
        /// </summary>
        /// <param name="directory">若目录不存在则创建</param>
        public DirectoryContent(string directory)
        {
            DirectoryInfo = new DirectoryInfo(directory);
            DirectoryInfo.Create();
        }

        private IContentEntry CreateEntryFromFilePath(string filePath)
        {
            string relativePath = PathHelper.GetRelativePath(DirectoryInfo.FullName, filePath);
            DirectoryContentEntry entry = new DirectoryContentEntry(this, relativePath);
            return entry;
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

        #region Read

        public override IEnumerable<IContentEntry> EnumerateEntries()
        {
            ThrowIfObjectDisposed();

            return Directory.EnumerateFiles(DirectoryInfo.FullName, "*", SearchOption.AllDirectories).Select(CreateEntryFromFilePath);
        }

        public override IEnumerable<IContentEntry> EnumerateEntries(string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();

            return Directory.EnumerateFiles(DirectoryInfo.FullName, searchPattern, searchOption).Select(CreateEntryFromFilePath);
        }

        public override IEnumerable<IContentEntry> EnumerateEntries(string directoryName, string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();

            string targetDirectory = Path.Combine(DirectoryInfo.FullName, directoryName);
            return Directory.EnumerateFiles(targetDirectory, searchPattern, searchOption).Select(CreateEntryFromFilePath);
        }

        public override IContentEntry GetEntry(string name)
        {
            ThrowIfObjectDisposed();

            string filePath = Path.Combine(DirectoryInfo.FullName, name);
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

            string filePath = Path.Combine(DirectoryInfo.FullName, name);
            return File.Exists(filePath);
        }

        public override Stream GetInputStream(IContentEntry entry)
        {
            ThrowIfObjectDisposed();

            var directoryContentEntry = (DirectoryContentEntry)entry;
            return new FileStream(directoryContentEntry.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public override Stream GetInputStream(string relativePath)
        {
            ThrowIfObjectDisposed();

            string filePath = Path.Combine(DirectoryInfo.FullName, relativePath);
            return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        #endregion

        #region Write

        public override IDisposable BeginUpdate()
        {
            ThrowIfObjectDisposed();

            isUpdating = true;
            return new ContentCommitUpdateDisposer(this);
        }

        public override void CommitUpdate()
        {
            ThrowIfObjectDisposed();
            ThrowIfObjectNotUpdating();

            isUpdating = false;
        }

        public override IContentEntry AddOrUpdate(string name, Stream source, bool isCloseStream)
        {
            ThrowIfObjectDisposed();
            ThrowIfObjectNotUpdating();

            string filePath = Path.Combine(DirectoryInfo.FullName, name);
            string directory = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(directory);

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                source.CopyTo(stream);
            }

            var entry = CreateEntry(filePath, name);
            return entry;
        }

        public override void Remove(IContentEntry entry)
        {
            ThrowIfObjectDisposed();
            ThrowIfObjectNotUpdating();

            var directoryContentEntry = (DirectoryContentEntry)entry;
            File.Delete(directoryContentEntry.FilePath);
        }

        public override bool Remove(string name)
        {
            ThrowIfObjectDisposed();

            string filePath = Path.Combine(DirectoryInfo.FullName, name);
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

        public override Stream GetOutputStream(IContentEntry entry)
        {
            ThrowIfObjectDisposed();
            ThrowIfObjectNotUpdating();

            var directoryContentEntry = (DirectoryContentEntry)entry;
            return new FileStream(directoryContentEntry.FilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
        }

        public override Stream GetOutputStream(string name, out IContentEntry entry)
        {
            ThrowIfObjectDisposed();
            ThrowIfObjectNotUpdating();

            string filePath = Path.Combine(DirectoryInfo.FullName, name);
            string directory = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(directory);
            entry = CreateEntry(filePath, name);
            return new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
        }

        #endregion
    }

    internal class DirectoryContentEntry : IContentEntry
    {
        public DirectoryContent Parent { get; private set; }
        public string Name { get; private set; }
        public string FilePath => Path.Combine(Parent.DirectoryInfo.FullName, Name);
        public DateTime LastWriteTime => File.GetLastWriteTime(FilePath);

        public DirectoryContentEntry(DirectoryContent parent, string name)
        {
            Parent = parent;
            Name = name;
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
