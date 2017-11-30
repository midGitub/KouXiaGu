using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Security.Cryptography;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 可读的Zip文件;
    /// </summary>
    public class LoadableZipFile : LoadableContent
    {
        /// <summary>
        /// 压缩文件;
        /// </summary>
        internal ZipFile ZipFile { get; private set; }

        /// <summary>
        /// 压缩文件流;
        /// </summary>
        internal Stream Stream { get; private set; }

        private FileInfo assetBundleFileInfo;

        /// <summary>
        /// AssetBundle 文件路径;
        /// </summary>
        protected override FileInfo AssetBundleFileInfo
        {
            get { return assetBundleFileInfo; }
        }

        /// <summary>
        /// 用于存放临时文件的目录;
        /// </summary>
        internal DirectoryInfo CacheDirectoryInfo { get; private set; }

        /// <summary>
        /// 压缩文件的MD5值,在没创建临时目录前都为NULL;
        /// </summary>
        internal string MD5 { get; private set; }

        internal string ZipFilePath
        {
            get { return ZipFile.Name; }
        }

        public override bool Compressed
        {
            get { return true; }
        }

        internal LoadableZipFile(ZipFile zipFile, Stream stream, LoadableContentDescription description, LoadableContentType type) : base(description, type)
        {
            if (zipFile == null)
                throw new ArgumentNullException(nameof(zipFile));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            ZipFile = zipFile;
            Stream = stream;
            assetBundleFileInfo = new FileInfo(InternalGetAssetBundle());
        }

        public override void Unload()
        {
            ZipFile.Close();
            Stream.Dispose();
        }

        public override IEnumerable<ILoadableEntry> EnumerateFiles()
        {
            foreach (ZipEntry entry in ZipFile)
            {
                if (entry.IsFile)
                {
                    ILoadableEntry zipLoadableEntry = new ZipLoadableEntry(this, entry);
                    yield return zipLoadableEntry;
                }
            }
        }

        public override Stream GetInputStream(ILoadableEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }
            else if (entry is ZipLoadableEntry)
            {
                var zipLoadableEntry = (ZipLoadableEntry)entry;
                return ZipFile.GetInputStream(zipLoadableEntry.ZipEntry);
            }
            else
            {
                throw new ArgumentException(string.Format("参数[{0}]不为类[{1}]", nameof(entry), nameof(ZipLoadableEntry)));
            }
        }

        private string InternalGetAssetBundle()
        {
            string assetBundlePath = Path.ChangeExtension(ZipFilePath, ResourcePath.AssetBundleExtension);
            return assetBundlePath;
        }

        /// <summary>
        /// 文件入口;
        /// </summary>
        public class ZipLoadableEntry : ILoadableEntry
        {
            public LoadableZipFile Parent { get; private set; }
            public ZipEntry ZipEntry { get; private set; }

            public string RelativePath
            {
                get { return ZipEntry.Name; }
            }

            public ZipLoadableEntry(LoadableZipFile parent, ZipEntry zipEntry)
            {
                Parent = parent;
                ZipEntry = zipEntry;
            }
        }
    }
}
