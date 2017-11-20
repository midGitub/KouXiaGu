using System;
using System.Linq;
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
        public ZipFile ZipFile { get; private set; }

        public LoadableZipFile(ZipFile zipFile, LoadableContentDescription description, LoadableContentType type) : base(description, type)
        {
            if (zipFile == null)
                throw new ArgumentNullException(nameof(zipFile));

            ZipFile = zipFile;
        }

        public override void Unload()
        {
            ZipFile.Close();
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

        public override Stream GetStream(ILoadableEntry entry)
        {
            if (entry is ZipLoadableEntry)
            {
                var zipLoadableEntry = (ZipLoadableEntry)entry;
                return ZipFile.GetInputStream(zipLoadableEntry.ZipEntry);
            }
            else
            {
                throw new ArgumentException(string.Format("参数[{0}]不为类[{1}]", nameof(entry), nameof(ZipLoadableEntry)));
            }
        }

        private class ZipLoadableEntry : ILoadableEntry
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
