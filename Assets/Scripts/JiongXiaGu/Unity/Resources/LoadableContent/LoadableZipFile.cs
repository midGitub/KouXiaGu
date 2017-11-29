using System;
using System.Linq;
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
            if (CacheDirectoryInfo == null)
            {
                CacheDirectoryInfo = CreateCacheDirectory();
            }

            string filePath = Path.Combine(CacheDirectoryInfo.FullName, entry.RelativePath);

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

        private const string CacheDirectoryDescriptionFileName = "ZipFileDescr";
        private XmlSerializer<ZipTempDirectoryDescription> descrXmlSerializer;
        private MD5CryptoServiceProvider mD5CryptoServiceProvider;

        /// <summary>
        /// 创建临时目录;
        /// </summary>
        internal DirectoryInfo CreateCacheDirectory()
        {
            if (descrXmlSerializer == null)
            {
                descrXmlSerializer = new XmlSerializer<ZipTempDirectoryDescription>();
                mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            }

            string cacheDirectory = GetCacheDirectory();
            DirectoryInfo cacheDirectoryInfo = new DirectoryInfo(cacheDirectory);
            MD5 = ComputeMD5(Stream);

            if (cacheDirectoryInfo.Exists)
            {
                try
                {
                    ZipTempDirectoryDescription zipTempDirectoryDescription = ReadCacheDescr(cacheDirectory);
                    if (zipTempDirectoryDescription.MD5 == MD5)
                    {
                        return cacheDirectoryInfo;
                    }
                    else
                    {
                        cacheDirectoryInfo.Delete(true);
                    }
                }
                catch
                {
                }
            }

            cacheDirectoryInfo.Create();
            WriteCacheDescr(cacheDirectory, new ZipTempDirectoryDescription()
            {
                MD5 = MD5,
            });

            return cacheDirectoryInfo;
        }

        private string ComputeMD5(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var md5 = mD5CryptoServiceProvider.ComputeHash(stream);
            var md5Str = string.Join(string.Empty, md5);
            return md5Str;
        }

        /// <summary>
        /// 读取目录的描述;
        /// </summary>
        private ZipTempDirectoryDescription ReadCacheDescr(string tempDirectory)
        {
            string filePath = GetCacheDescrPath(tempDirectory);
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var descr = descrXmlSerializer.Deserialize(stream);
                return descr;
            }
        }

        /// <summary>
        /// 输出描述文件到目录;
        /// </summary>
        private void WriteCacheDescr(string tempDirectory, ZipTempDirectoryDescription descr)
        {
            string filePath = GetCacheDescrPath(tempDirectory);
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                descrXmlSerializer.Serialize(stream, descr);
            }
        }

        private string GetCacheDescrPath(string tempDirectory)
        {
            string descriptionFileName = CacheDirectoryDescriptionFileName + descrXmlSerializer.FileExtension;
            string filePath = Path.Combine(tempDirectory, descriptionFileName);
            return filePath;
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

        /// <summary>
        /// 临时文件夹描述;
        /// </summary>
        public struct ZipTempDirectoryDescription
        {
            /// <summary>
            /// 资源文件的DM5值;
            /// </summary>
            public string MD5 { get; set; }
        }
    }
}
