using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 可读的Zip文件;
    /// </summary>
    public class LoadableZipFile : LoadableContentConstruct
    {
        private ZipFile zipFile;
        public FileInfo ZipFileInfo { get; private set; }

        public override bool IsLoadable
        {
            get { return zipFile != null; }
        }

        public override bool Exists
        {
            get { return ZipFileInfo.Exists; }
        }

        public LoadableZipFile(FileInfo zipFileInfo)
        {
            ZipFileInfo = zipFileInfo;
        }

        public override void Load()
        {
            if (zipFile == null)
            {
                ZipFileInfo.ThrowIfFileNotExisted();
                zipFile = new ZipFile(ZipFileInfo.OpenRead());
            }
        }

        public override void Unload()
        {
            if (zipFile != null)
            {
                (zipFile as IDisposable).Dispose();
                zipFile = null;
            }
        }

        public override IEnumerable<ILoadableEntry> EnumerateFiles()
        {
            foreach (ZipEntry entry in zipFile)
            {
                ZipLoadableEntry zipLoadableEntry = new ZipLoadableEntry(this, entry);
                yield return zipLoadableEntry;
            }
        }

        public override Stream GetStream(ILoadableEntry entry)
        {
            if (entry is ZipLoadableEntry)
            {
                var zipLoadableEntry = (ZipLoadableEntry)entry;
                ZipEntry zipEntry = zipFile.GetEntry(zipLoadableEntry.RelativePath);

                if (zipEntry != null)
                {
                    return zipFile.GetInputStream(zipEntry);
                }
                else
                {
                    throw new FileNotFoundException(string.Format("未找到相对路径为[{0}]的文件", zipLoadableEntry.RelativePath));
                }
            }
            else
            {
                throw new ArgumentException(string.Format("参数[{0}]不为类[{1}]", nameof(entry), nameof(ZipLoadableEntry)));
            }
        }

        private struct ZipLoadableEntry : ILoadableEntry
        {
            public LoadableZipFile Parent { get; private set; }
            public string RelativePath { get; private set; }

            public ZipLoadableEntry(LoadableZipFile parent, ZipEntry zipEntry)
            {
                Parent = parent;
                RelativePath = zipEntry.Name;
            }
        }
    }
}
