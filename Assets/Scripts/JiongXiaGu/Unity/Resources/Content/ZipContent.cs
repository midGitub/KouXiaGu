using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源Zip文件;
    /// </summary>
    public class ZipContent : Content
    {
        /// <summary>
        /// 压缩包文件路径;
        /// </summary>
        private readonly string zipFilePath;

        /// <summary>
        /// 压缩文件;
        /// </summary>
        private ZipFile zipFile;

        /// <summary>
        /// 压缩文件流;
        /// </summary>
        private Stream stream;

        /// <summary>
        /// 正在更新的路径;
        /// </summary>
        private Lazy<List<string>> updatingPath = new Lazy<List<string>>();

        private bool isDisposed;
        public override bool IsUpdating => zipFile.IsUpdating;
        public override bool CanRead => !isDisposed;
        public override bool CanWrite => !isDisposed;
        public override bool IsDisposed => isDisposed;

        /// <summary>
        /// 指定压缩文件路径构建,若路径不存在则返回异常;
        /// </summary>
        public ZipContent(string zipFilePath)
        {
            this.zipFilePath = zipFilePath;
            stream = new FileStream(zipFilePath, FileMode.Open, FileAccess.ReadWrite);
            zipFile = new ZipFile(stream);
        }

        /// <summary>
        /// 指定参数构建;
        /// </summary>
        public ZipContent(string zipFilePath, Stream stream, ZipFile zipFile)
        {
            if (zipFile == null)
                throw new ArgumentNullException(nameof(zipFile));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            this.zipFilePath = zipFilePath;
            this.stream = stream;
            this.zipFile = zipFile;
        }

        /// <summary>
        /// 创建新的压缩文件,若文件已经存在则返回异常;
        /// </summary>
        public static ZipContent CreateNew(string zipFilePath)
        {
            Stream stream = new FileStream(zipFilePath, FileMode.CreateNew, FileAccess.ReadWrite);
            ZipFile zip = ZipFile.Create(stream);
            ZipContent contentZip = new ZipContent(zipFilePath, stream, zip);
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

                updatingPath = null;

                isDisposed = true;
            }
        }

        public override IEnumerable<string> EnumerateFiles()
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

        public override Stream GetInputStream(string relativePath)
        {
            ThrowIfObjectDisposed();

            relativePath = Normalize(relativePath);
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

        public override IDisposable BeginUpdate()
        {
            ThrowIfObjectDisposed();

            zipFile.BeginUpdate();
            return new ContentCommitUpdateDisposer(this);
        }

        public override void CommitUpdate()
        {
            ThrowIfObjectDisposed();

            zipFile.CommitUpdate();
        }

        public override Stream GetOutputStream(string relativePath)
        {
            ThrowIfObjectDisposed();

            relativePath = Normalize(relativePath);

            if (updatingPath.Value.Contains(relativePath))
            {
                throw new IOException(string.Format("该相对路径[{0}]已经被占用", relativePath));
            }

            updatingPath.Value.Add(relativePath);
            ZipUpdateStream update = new ZipUpdateStream(this, relativePath);
            return update;
        }

        public override bool Remove(string relativePath)
        {
            ThrowIfObjectDisposed();

            relativePath = Normalize(relativePath);
            return zipFile.Delete(relativePath);
        }

        /// <summary>
        /// 用于缓存更新数据;
        /// </summary>
        private class ZipUpdateStream : Stream, IStaticDataSource
        {
            private bool isDisposed = false;
            private readonly Stream stream;
            private readonly ZipContent zipContent;
            private readonly string relativePath;
            private ZipFile zipFile => zipContent.zipFile;
            public override bool CanRead => !isDisposed && stream.CanRead;
            public override bool CanSeek => !isDisposed && stream.CanSeek;
            public override bool CanWrite => !isDisposed && stream.CanWrite;

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

            public ZipUpdateStream(ZipContent zipContent, string relativePath)
            {
                stream = new MemoryStream();
                this.zipContent = zipContent;
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
                    zipContent.updatingPath.Value.Remove(relativePath);
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
