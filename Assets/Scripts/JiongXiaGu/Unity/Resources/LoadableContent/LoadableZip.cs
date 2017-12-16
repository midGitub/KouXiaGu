using System;
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
        /// <summary>
        /// 压缩包文件路径;
        /// </summary>
        internal readonly string zipFilePath;

        /// <summary>
        /// 压缩文件;
        /// </summary>
        internal ZipFile zipFile;

        /// <summary>
        /// 压缩文件流;
        /// </summary>
        internal Stream stream;

        internal LoadableZip(string zipFilePath, Stream stream, ZipFile zipFile, LoadableContentDescription description) : base(description)
        {
            if (zipFile == null)
                throw new ArgumentNullException(nameof(zipFile));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            this.zipFilePath = zipFilePath;
            this.stream = stream;
            this.zipFile = zipFile;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                zipFile.Close();
                zipFile = null;

                stream.Dispose();
                stream = null;
            }
        }


        internal override IEnumerable<string> InternalEnumerateFiles()
        {
            ThrowIfObjectDisposed();

            foreach (ZipEntry entry in zipFile)
            {
                if (entry.IsFile)
                {
                    yield return entry.Name;
                }
            }
        }

        internal override Stream InternaltInputStream(string relativePath)
        {
            ThrowIfObjectDisposed();

            ZipEntry entry = zipFile.GetEntry(relativePath);
            if (entry != null && entry.IsFile)
            {
                return zipFile.GetInputStream(entry);
            }
            else
            {
                throw new FileNotFoundException(relativePath);
            }
        }



        internal override void InternaltBeginUpdate()
        {
            ThrowIfObjectDisposed();

            zipFile.BeginUpdate();
        }

        internal override void InternaltCommitUpdate()
        {
            ThrowIfObjectDisposed();

            zipFile.CommitUpdate();
        }

        /// <summary>
        /// 在 CommitUpdate() 之后才会应用变化;
        /// </summary>
        internal override void InternaltAddOrUpdate(string relativePath, Stream stream)
        {
            ThrowIfObjectDisposed();

            if (!stream.CanRead)
                throw new NotSupportedException(string.Format("[{0}]不可读;", nameof(stream)));

            ZipUpdate zipUpdate = new ZipUpdate(stream);
            zipFile.Add(zipUpdate, relativePath);
        }

        internal override bool InternaltRemove(string relativePath)
        {
            return zipFile.Delete(relativePath);
        }

        /// <summary>
        /// 写完毕后需要关闭流,在 CommitUpdate() 之后才会应用变化;
        /// </summary>
        internal override Stream InternaltGetOutStream(string relativePath)
        {
            ThrowIfObjectDisposed();

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

        internal override Stream InternaltCreateOutStream(string relativePath)
        {
            ThrowIfObjectDisposed();

            ZipUpdateStream update = new ZipUpdateStream(zipFile, relativePath);
            return update;
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
