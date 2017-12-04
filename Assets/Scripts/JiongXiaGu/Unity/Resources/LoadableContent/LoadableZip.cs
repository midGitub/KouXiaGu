﻿using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 可读的Zip文件;
    /// </summary>
    public class LoadableZip : LoadableContent
    {
        internal readonly string filePath;

        /// <summary>
        /// 压缩文件;
        /// </summary>
        internal readonly ZipFile zipFile;

        /// <summary>
        /// 压缩文件流;
        /// </summary>
        internal readonly Stream stream;

        internal LoadableZip(string filePath, Stream stream, ZipFile zipFile, LoadableContentDescription description) : base(description, InternalGetAssetBundle(filePath))
        {
            if (zipFile == null)
                throw new ArgumentNullException(nameof(zipFile));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            this.filePath = filePath;
            this.stream = stream;
            this.zipFile = zipFile;
        }

        [Obsolete]
        internal LoadableZip(ZipFile zipFile, Stream stream, LoadableContentDescription description, LoadableContentType type) : base(description, InternalGetAssetBundle(zipFile.Name))
        {
            if (zipFile == null)
                throw new ArgumentNullException(nameof(zipFile));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            this.zipFile = zipFile;
            this.stream = stream;
        }

        public override void Unload()
        {
            zipFile.Close();
            stream.Dispose();
        }

        public override IEnumerable<ILoadableEntry> EnumerateFiles()
        {
            foreach (ZipEntry entry in zipFile)
            {
                if (entry.IsFile)
                {
                    var zipLoadableEntry = new ZipLoadableEntry(this, entry);
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
                return zipFile.GetInputStream(zipLoadableEntry.ZipEntry);
            }
            else
            {
                throw new ArgumentException(string.Format("参数[{0}]不为类[{1}]", nameof(entry), nameof(ZipLoadableEntry)));
            }
        }

        public override void BeginUpdate()
        {
            base.BeginUpdate();
            zipFile.BeginUpdate();
        }

        public override void CommitUpdate()
        {
            base.CommitUpdate();
            zipFile.CommitUpdate();
        }

        /// <summary>
        /// 在 CommitUpdate() 之后才会应用变化;
        /// </summary>
        public override void AddOrUpdate(string relativePath, Stream stream)
        {
            if (!stream.CanRead)
                throw new NotSupportedException(string.Format("[{0}]不可读;", nameof(stream)));

            ZipUpdate zipUpdate = new ZipUpdate(stream);
            zipFile.Add(zipUpdate, relativePath);
        }

        /// <summary>
        /// 写完毕后需要关闭流,在 CommitUpdate() 之后才会应用变化;
        /// </summary>
        public override Stream GetOutStream(string relativePath)
        {
            ZipEntry entry = zipFile.GetEntry(relativePath);
            if (entry != null)
            {
                var inputSream = zipFile.GetInputStream(entry);
                MemoryStream stream = new MemoryStream();
                inputSream.CopyTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                ZipUpdateStream update = new ZipUpdateStream(stream, zipFile, relativePath);
                return update;
            }
            else
            {
                ZipUpdateStream update = new ZipUpdateStream(zipFile, relativePath);
                return update;
            }
        }

        public override Stream CreateOutStream(string relativePath)
        {
            ZipUpdateStream update = new ZipUpdateStream(zipFile, relativePath);
            return update;
        }

        private static string InternalGetAssetBundle(string fileName)
        {
            string assetBundlePath = Path.ChangeExtension(fileName, Resource.AssetBundleExtension);
            return assetBundlePath;
        }

        /// <summary>
        /// 文件入口;
        /// </summary>
        public class ZipLoadableEntry : ILoadableEntry
        {
            public LoadableZip Parent { get; private set; }
            public ZipEntry ZipEntry { get; private set; }

            public string RelativePath
            {
                get { return ZipEntry.Name; }
            }

            public ZipLoadableEntry(LoadableZip parent, ZipEntry zipEntry)
            {
                Parent = parent;
                ZipEntry = zipEntry;
            }
        }

        private class ZipUpdate : IStaticDataSource
        {
            private Stream stream;

            public ZipUpdate(Stream stream)
            {
                if (!stream.CanRead)
                    throw new NotSupportedException(string.Format("[{0}]不可读;", nameof(stream)));

                this.stream = stream;
            }

            public Stream GetSource()
            {
                return stream;
            }
        }

        /// <summary>
        /// 用于缓存更新数据;
        /// </summary>
        private class ZipUpdateStream : Stream, IStaticDataSource
        {
            private bool isDisposed = false;
            private readonly Stream stream;
            private readonly ZipFile zipFile;
            private readonly string relativePath;

            public override bool CanRead
            {
                get { return !isDisposed && stream.CanRead; }
            }

            public override bool CanSeek
            {
                get { return !isDisposed && stream.CanSeek; }
            }

            public override bool CanWrite
            {
                get { return !isDisposed && stream.CanWrite; }
            }

            public override long Length
            {
                get
                {
                    ThrowIfObjectDisposed();
                    return stream.Length;
                }
            }

            public override long Position
            {
                get
                {
                    ThrowIfObjectDisposed();
                    return stream.Position;
                }
                set
                {
                    ThrowIfObjectDisposed();
                    stream.Position = value;
                }
            }

            public ZipUpdateStream(ZipFile zipFile, string relativePath) : this(new MemoryStream(), zipFile, relativePath)
            {
            }

            public ZipUpdateStream(Stream stream, ZipFile zipFile, string relativePath)
            {
                this.stream = stream;
                this.zipFile = zipFile;
                this.relativePath = relativePath;
            }

            Stream IStaticDataSource.GetSource()
            {
                return stream;
            }

            private void ThrowIfObjectDisposed()
            {
                if (isDisposed)
                {
                    throw new ObjectDisposedException(nameof(ZipUpdateStream));
                }
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                if (disposing && !isDisposed)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    zipFile.Add(this, relativePath);
                    isDisposed = true;
                }
            }

            public override void Flush()
            {
                ThrowIfObjectDisposed();
                stream.Flush();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                ThrowIfObjectDisposed();
                return stream.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                ThrowIfObjectDisposed();
                stream.SetLength(value);
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                ThrowIfObjectDisposed();
                return stream.Read(buffer, offset, count);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                ThrowIfObjectDisposed();
                stream.Write(buffer, offset, count);
            }
        }
    }
}
