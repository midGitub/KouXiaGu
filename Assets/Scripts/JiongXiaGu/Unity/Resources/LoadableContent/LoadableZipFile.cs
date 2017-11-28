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
        private DirectoryInfo tempDirectoryInfo;

        public override bool Compressed
        {
            get { return true; }
        }

        public LoadableZipFile(ZipFile zipFile, LoadableContentDescription description, LoadableContentType type) : base(description, type)
        {
            if (zipFile == null)
                throw new ArgumentNullException(nameof(zipFile));

            ZipFile = zipFile;
            string directoryName = Path.GetDirectoryName(ZipFile.Name);
            string fileName = "temp_" + Path.GetFileNameWithoutExtension(ZipFile.Name) + "_" + description.Version;
            string tempDirectoryName = Path.Combine(directoryName, fileName);
            tempDirectoryInfo = new DirectoryInfo(tempDirectoryName);

            if (tempDirectoryInfo.Exists)
            {
                tempDirectoryInfo.Delete(true);
            }
        }

        public override void Unload()
        {
            ZipFile.Close();
            tempDirectoryInfo?.Delete(true);
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

        /// <summary>
        /// 获取到临时的文件路径;
        /// </summary>
        public override string GetFile(ILoadableEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            if (!tempDirectoryInfo.Exists)
            {
                tempDirectoryInfo.Create();
            }

            string filePath = Path.Combine(tempDirectoryInfo.FullName, entry.RelativePath);

            if (!File.Exists(filePath))
            {
                string directoryName = Path.GetDirectoryName(filePath);
                Directory.CreateDirectory(directoryName);

                using (Stream fileStream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write), inputStream = GetInputStream(entry))
                {
                    inputStream.CopyTo(fileStream);
                }
            }

            return filePath;
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
