using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace JiongXiaGu
{


    public static class ZipHelper
    {

        /// <summary>
        /// 提供实现 using 语法的 BeginUpdate() 方法;
        /// </summary>
        public static IDisposable BeginUpdateAuto(this ZipFile zipFile)
        {
            zipFile.BeginUpdate();
            return new CallBack(() => zipFile.CommitUpdate());
        }


        /// <summary>
        /// 添加内容到zip文件,在 ZipFile.CommitUpdate() 之后自动释放流;
        /// </summary>
        public static void Add(this ZipFile zipFile, string entryName, Stream stream)
        {
            if (zipFile == null)
                throw new ArgumentNullException(nameof(zipFile));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            ZipData data = new ZipData(stream);
            zipFile.Add(data, entryName);
        }

        private struct ZipData : IStaticDataSource
        {
            private Stream stream;

            public ZipData(Stream stream)
            {
                this.stream = stream;
            }

            public Stream GetSource()
            {
                return stream;
            }
        }

        /// <summary>
        /// 创建 MemoryStream 用于缓存数据;
        /// </summary>
        public static Stream GetOutputStream(this ZipFile zipFile, string entryName)
        {
            if (zipFile == null)
                throw new ArgumentNullException(nameof(zipFile));

            ZipUpdateData data = new ZipUpdateData(zipFile, entryName);
            return data;
        }

        /// <summary>
        /// 用于缓存更新数据;
        /// </summary>
        private class ZipUpdateData : Stream, IStaticDataSource
        {
            private bool isDisposed = false;
            private readonly Stream stream;
            private readonly ZipFile zipFile;
            private readonly string entryName;
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

            public ZipUpdateData(ZipFile zipFile, string entryName)
            {
                stream = new MemoryStream();
                this.zipFile = zipFile;
                this.entryName = entryName;
            }

            Stream IStaticDataSource.GetSource()
            {
                return stream;
            }

            private void ThrowIfObjectDisposed()
            {
                if (isDisposed)
                {
                    throw new ObjectDisposedException(nameof(ZipUpdateData));
                }
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                if (disposing && !isDisposed)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    zipFile.Add(this, entryName);
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
