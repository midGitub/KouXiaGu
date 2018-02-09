using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源Zip文件;
    /// </summary>
    public class SharpZipLibContent : CompressContent<SharpZipLibContent.ZipContentEntry>
    {
        /// <summary>
        /// 压缩文件;
        /// </summary>
        private ZipFile zipFile;

        /// <summary>
        /// 压缩文件流;
        /// </summary>
        private Stream stream;

        private bool isDisposed;
        public override bool IsUpdating => zipFile.IsUpdating;
        public override bool CanRead => !isDisposed;
        public override bool CanWrite => !isDisposed;
        public override bool IsDisposed => isDisposed;
        public override bool IsCompress => true;

        /// <summary>
        /// 指定压缩文件路径构建,若路径不存在则返回异常;
        /// </summary>
        public SharpZipLibContent(string zipFilePath)
        {
            stream = new FileStream(zipFilePath, FileMode.Open, FileAccess.ReadWrite);
            zipFile = new ZipFile(stream);
            RebuildEntriesCollection();
        }

        public SharpZipLibContent(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            this.stream = stream;
            zipFile = new ZipFile(stream);
            RebuildEntriesCollection();
        }

        /// <summary>
        /// 指定参数构建;
        /// </summary>
        public SharpZipLibContent(Stream stream, ZipFile zipFile)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (zipFile == null)
                throw new ArgumentNullException(nameof(zipFile));

            this.stream = stream;
            this.zipFile = zipFile;
            RebuildEntriesCollection();
        }

        /// <summary>
        /// 创建新的压缩文件,若文件已经存在则返回异常;
        /// </summary>
        public static SharpZipLibContent CreateNew(string zipFilePath)
        {
            Stream stream = new FileStream(zipFilePath, FileMode.CreateNew, FileAccess.ReadWrite);
            ZipFile zip = ZipFile.Create(stream);
            SharpZipLibContent contentZip = new SharpZipLibContent(stream, zip);
            return contentZip;
        }

        public override void Dispose()
        {
            if (!isDisposed)
            {
                zipFile.Close();
                zipFile = null;

                stream.Dispose();
                stream = null;

                isDisposed = true;
            }
        }

        public override IEnumerable<IContentEntry> EnumerateCompressedEntries()
        {
            return zipFile.Cast<ZipEntry>().Where(entry => entry.IsFile).Select(CreateEntry);
        }

        protected override Stream GetInputStreamInCompression(ZipContentEntry entry)
        {
            ThrowIfObjectDisposed();

            return zipFile.GetInputStream(entry.ZipEntry);
        }

        private IContentEntry CreateEntry(ZipEntry zipEntry)
        {
            ZipContentEntry entry = new ZipContentEntry(this, zipEntry);
            return entry;
        }

        public override IDisposable BeginUpdate()
        {
            IDisposable disposable = base.BeginUpdate();
            zipFile.BeginUpdate();
            return disposable;
        }

        public override void CommitUpdate()
        {
            base.CommitUpdate();
            zipFile.CommitUpdate();
            RebuildEntriesCollection();
        }

        protected override void AddEntry(string name, Stream source)
        {
            IStaticDataSource staticDataSource = new ZipUpdate(source);
            zipFile.Add(staticDataSource, name);
        }

        protected override void RemoveEntry(string name)
        {
            zipFile.Delete(name);
        }

        public struct ZipContentEntry : IContentEntry
        {
            public SharpZipLibContent Parent { get; private set; }
            public ZipEntry ZipEntry { get; private set; }
            public string Name => ZipEntry.Name;
            public DateTime LastWriteTime => ZipEntry.DateTime;

            public ZipContentEntry(SharpZipLibContent parent, ZipEntry zipEntry)
            {
                Parent = parent;
                ZipEntry = zipEntry;
            }

            public Stream OpenRead()
            {
                return Parent.zipFile.GetInputStream(ZipEntry);
            }
        }

        private struct ZipUpdate : IStaticDataSource
        {
            public Stream Stream { get; private set; }

            public ZipUpdate(Stream stream)
            {
                Stream = stream;
            }

            public Stream GetSource()
            {
                return Stream;
            }
        }
    }
}
