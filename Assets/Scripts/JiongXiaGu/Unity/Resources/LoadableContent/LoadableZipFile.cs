using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

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
        internal readonly ZipFile zipFile;

        /// <summary>
        /// 所有地址入口;
        /// </summary>
        internal readonly List<ZipLoadableEntry> entrys;

        /// <summary>
        /// 压缩文件流;
        /// </summary>
        internal readonly Stream stream;

        /// <summary>
        /// AssetBundle 文件路径;
        /// </summary>
        internal readonly FileInfo assetBundleFileInfo;

        /// <summary>
        /// AssetBundle 文件路径;
        /// </summary>
        protected override FileInfo AssetBundleFileInfo
        {
            get { return assetBundleFileInfo; }
        }

        internal string ZipFilePath
        {
            get { return zipFile.Name; }
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

            this.zipFile = zipFile;
            this.stream = stream;
            assetBundleFileInfo = new FileInfo(InternalGetAssetBundle());
            entrys = GetEntryAll();
        }

        public override void Unload()
        {
            zipFile.Close();
            stream.Dispose();
        }

        private List<ZipLoadableEntry> GetEntryAll()
        {
            List<ZipLoadableEntry> entrys = new List<ZipLoadableEntry>();

            foreach (ZipEntry entry in zipFile)
            {
                if (entry.IsFile)
                {
                    var zipLoadableEntry = new ZipLoadableEntry(this, entry);
                    entrys.Add(zipLoadableEntry);
                }
            }

            return entrys;
        }

        public override IEnumerable<ILoadableEntry> EnumerateFiles()
        {
            return entrys;
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
                return zipFile.GetInputStream(zipLoadableEntry.ZipEntry);
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
