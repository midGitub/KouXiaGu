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
        public ZipFile ZipFile { get; private set; }

        public LoadableZipFile(ZipFile zipFile)
        {
            if (zipFile == null)
                throw new ArgumentNullException(nameof(zipFile));

            ZipFile = zipFile;
        }

        public override IEnumerable<ILoadableEntry> EnumerateFiles()
        {
            foreach (ZipEntry entry in ZipFile)
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
                ZipEntry zipEntry = ZipFile.GetEntry(zipLoadableEntry.RelativePath);

                if (zipEntry != null)
                {
                    return ZipFile.GetInputStream(zipEntry);
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
