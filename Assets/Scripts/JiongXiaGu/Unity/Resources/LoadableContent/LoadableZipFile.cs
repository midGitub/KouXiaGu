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

        internal LoadableZipFile(ZipFile zipFile, DirectoryInfo tempDirectoryInfo, LoadableContentDescription description, LoadableContentType type) : base(description, type)
        {
            if (zipFile == null)
                throw new ArgumentNullException(nameof(zipFile));

            ZipFile = zipFile;
            this.tempDirectoryInfo = tempDirectoryInfo;
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
        /// 将需要获取的文件解压到一个临时的文件夹内,返回其路径;
        /// 在资源文件发生变化时,都会重新解压,保证文件最新;
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

        /// <summary>
        /// 文件入口;
        /// </summary>
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
