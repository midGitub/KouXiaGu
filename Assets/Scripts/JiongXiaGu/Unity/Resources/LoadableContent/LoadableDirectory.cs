using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 可读取资源的目录;
    /// </summary>
    public class LoadableDirectory : LoadableContent
    {
        internal readonly DirectoryInfo directoryInfo;

        public LoadableDirectory(string directory, LoadableContentDescription description) : base(description)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException(directory);

            directoryInfo = new DirectoryInfo(directory);
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override IEnumerable<ILoadableEntry> EnumerateFiles()
        {
            return directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories).Select(delegate (FileInfo fileInfo)
            {
                return (ILoadableEntry)new FileEntry(this, directoryInfo, fileInfo);
            });
        }

        public override IEnumerable<ILoadableEntry> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return directoryInfo.EnumerateFiles(searchPattern, searchOption).Select(delegate (FileInfo fileInfo)
            {
                return (ILoadableEntry)new FileEntry(this, directoryInfo, fileInfo);
            });
        }

        public override IEnumerable<ILoadableEntry> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            string directory = Path.Combine(this.directoryInfo.FullName, directoryName);
            var directoryInfo = new DirectoryInfo(directory);

            if (directoryInfo.Exists)
            {
                return directoryInfo.EnumerateFiles(searchPattern, searchOption).Select(delegate (FileInfo fileInfo)
                {
                    return (ILoadableEntry)new FileEntry(this, this.directoryInfo, fileInfo);
                });
            }
            else
            {
                return new ILoadableEntry[0];
            }
        }

        public override Stream GetInputStream(ILoadableEntry entry)
        {
            var fileEntry = TypeOfFileEntry(entry);
            string filePath = Path.Combine(directoryInfo.FullName, entry.RelativePath);
            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }

        public override void BeginUpdate()
        {
        }

        public override void CommitUpdate()
        {
        }

        public override void AddOrUpdate(string relativePath, Stream stream)
        {
            string filePath = GetFullPath(relativePath);
            var fStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            stream.CopyTo(fStream);
        }

        public override bool Remove(string relativePath)
        {
            string filePath = Path.Combine(directoryInfo.FullName, relativePath);
            if (File.Exists(relativePath))
            {
                File.Delete(filePath);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Remove(ILoadableEntry entry)
        {
            var fileEntry = TypeOfFileEntry(entry);
            string filePath = Path.Combine(directoryInfo.FullName, entry.RelativePath);
            File.Delete(filePath);
        }

        public override Stream GetOutStream(string relativePath)
        {
            string filePath = GetFullPath(relativePath);
            return new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        public override Stream CreateOutStream(string relativePath)
        {
            string filePath = GetFullPath(relativePath);
            return new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
        }

        private string GetFullPath(string relativePath)
        {
            string filePath = Path.Combine(directoryInfo.FullName, relativePath);
            return filePath;
        }

        private FileEntry TypeOfFileEntry(ILoadableEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }
            else if (entry is FileEntry)
            {
                return (FileEntry)entry;
            }
            else
            {
                throw new ArgumentException(string.Format("参数[{0}]不为类[{1}]", nameof(entry), nameof(FileEntry)));
            }
        }

        protected override IEnumerable<AssetBundleDescription> GetAssetBundlesDescription()
        {
            AssetBundleDescription[] assetBundleDescriptions = Description.AssetBundles;
            foreach (var assetBundleDescription in assetBundleDescriptions)
            {
                var newDescr = assetBundleDescription;
                newDescr.Path = Path.Combine(directoryInfo.FullName, assetBundleDescription.Path);
                yield return newDescr;
            }
        }

        private struct FileEntry : ILoadableEntry
        {
            public LoadableDirectory Parent { get; private set; }

            /// <summary>
            /// 相对路径;
            /// </summary>
            public string RelativePath { get; private set; }

            public FileEntry(LoadableDirectory parent, DirectoryInfo directoryInfo, FileInfo fileInfo)
            {
                Parent = parent;
                RelativePath = PathHelper.GetRelativePath(directoryInfo.FullName, fileInfo.FullName);
            }
        }
    }
}
