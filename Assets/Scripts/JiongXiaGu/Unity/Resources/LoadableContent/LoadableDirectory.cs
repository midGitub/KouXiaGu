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
        private FileInfo assetBundleFileInfo;
        internal DirectoryInfo DirectoryInfo { get; private set; }

        protected override FileInfo AssetBundleFileInfo
        {
            get { return assetBundleFileInfo; }
        }

        public override bool Compressed
        {
            get { return false; }
        }

        public LoadableDirectory(string directory, LoadableContentDescription description, LoadableContentType type) : base(description, type)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentNullException(nameof(directory));

            directory = PathHelper.Normalize(directory);
            DirectoryInfo = new DirectoryInfo(directory);
            assetBundleFileInfo = new FileInfo(InternalGetAssetBundle());
        }

        public override void Unload()
        {
            return;
        }

        public override IEnumerable<ILoadableEntry> EnumerateFiles()
        {
            return DirectoryInfo.EnumerateFiles("*", SearchOption.AllDirectories).Select(delegate (FileInfo fileInfo)
            {
                return (ILoadableEntry)new FileEntry(this, DirectoryInfo, fileInfo);
            });
        }

        public override IEnumerable<ILoadableEntry> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return DirectoryInfo.EnumerateFiles(searchPattern, searchOption).Select(delegate (FileInfo fileInfo)
            {
                return (ILoadableEntry)new FileEntry(this, DirectoryInfo, fileInfo);
            });
        }

        public override IEnumerable<ILoadableEntry> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            string directory = Path.Combine(DirectoryInfo.FullName, directoryName);
            var directoryInfo = new DirectoryInfo(directory);

            if (directoryInfo.Exists)
            {
                return directoryInfo.EnumerateFiles(searchPattern, searchOption).Select(delegate (FileInfo fileInfo)
                {
                    return (ILoadableEntry)new FileEntry(this, DirectoryInfo, fileInfo);
                });
            }
            else
            {
                return new ILoadableEntry[0];
            }
        }

        public override Stream GetInputStream(ILoadableEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }
            else if (entry is FileEntry)
            {
                string filePath = Path.Combine(DirectoryInfo.FullName, entry.RelativePath);
                return new FileStream(filePath, FileMode.Open, FileAccess.Read);
            }
            else
            {
                throw new ArgumentException(string.Format("参数[{0}]不为类[{1}]", nameof(entry), nameof(FileEntry)));
            }
        }

        private string InternalGetAssetBundle()
        {
            string assetBundlePath = Path.Combine(DirectoryInfo.Name, "AssetBundles", "assetBundle");
            return assetBundlePath;
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
