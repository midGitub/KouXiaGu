using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 提供多个读取,单个写入的流包装;
    /// </summary>
    public class ExclusiveStream : IDisposable
    {
        private bool isDisposed = false;
        private volatile int readerCount = 0;
        private volatile bool isWrite = false;

        public Stream SynchronizedSource { get; private set; }
        public bool IsCloseStream { get; private set; }

        public ExclusiveStream(Stream soure, bool isCloseStream)
        {
            if (soure == null)
                throw new ArgumentNullException(nameof(soure));
            if (!soure.CanRead || !soure.CanWrite || !soure.CanSeek)
                throw new ArgumentException(nameof(soure));

            SynchronizedSource = Stream.Synchronized(soure);
            IsCloseStream = isCloseStream;
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                if (IsCloseStream)
                {
                    SynchronizedSource.Dispose();
                    SynchronizedSource = null;
                }

                isDisposed = true;
            }
        }

        public Stream GetInputStream()
        {
            if (isWrite)
                throw new IOException("Stream已经被占用");

            readerCount++;
            var inputeStream = new InputStream(SynchronizedSource, delegate()
            {
                readerCount--;
            });
            return inputeStream;
        }

        public Stream GetOutputStream()
        {
            if (readerCount > 0 || isWrite)
                throw new IOException("Stream已经被占用");

            isWrite = true;
            SynchronizedSource.Position = 0;
            var outputStream = new OuputStream(SynchronizedSource, delegate()
            {
                isWrite = false;
            });
            return outputStream;
        }


        private class InputStream : Stream, IDisposable
        {
            private bool isDisposed;
            private Stream baseStream;
            private long readPos;
            private Action onDispose;

            public override bool CanRead => baseStream.CanRead && !isDisposed;
            public override bool CanWrite => baseStream.CanWrite && !isDisposed;
            public override bool CanSeek => baseStream.CanSeek && !isDisposed;

            public override long Length
            {
                get
                {
                    ThrowIfObjectDisposed();

                    return baseStream.Length;
                }
            }

            public override long Position
            {
                get
                {
                    ThrowIfObjectDisposed();
                    return readPos;
                }
                set
                {
                    ThrowIfObjectDisposed();
                    readPos = value;
                }
            }

            public InputStream(Stream baseStream, Action onDispose)
            {
                if (baseStream == null)
                    throw new ArgumentNullException(nameof(baseStream));

                this.baseStream = baseStream;
                this.onDispose = onDispose;
                readPos = 0;
            }

            protected override void Dispose(bool disposing)
            {
                if (!isDisposed)
                {
                    onDispose?.Invoke();
                    onDispose = null;

                    isDisposed = true;
                }
            }

            public override int ReadByte()
            {
                ThrowIfObjectDisposed();

                baseStream.Seek(readPos++, SeekOrigin.Begin);
                return baseStream.ReadByte();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                ThrowIfObjectDisposed();

                baseStream.Seek(readPos, SeekOrigin.Begin);
                int readCount = baseStream.Read(buffer, offset, count);
                if (readCount > 0)
                {
                    readPos += readCount;
                }
                return readCount;
            }
            
            public override long Seek(long offset, SeekOrigin origin)
            {
                ThrowIfObjectDisposed();

                switch (origin)
                {
                    case SeekOrigin.Begin:
                        readPos = offset;
                        return readPos;

                    case SeekOrigin.Current:
                        readPos += offset;
                        return readPos;

                    case SeekOrigin.End:
                        readPos = baseStream.Length + offset;
                        return readPos;

                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            public override void Flush()
            {
                ThrowIfObjectDisposed();

                baseStream.Flush();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            /// <summary>
            /// 若该实例已经被销毁,则返回异常;
            /// </summary>
            private void ThrowIfObjectDisposed()
            {
                if (isDisposed)
                {
                    throw new ObjectDisposedException(ToString());
                }
            }
        }

        private class OuputStream : Stream
        {
            private bool isDisposed = false;
            private Stream baseStream;
            private Action onDispose;

            public override bool CanRead => baseStream.CanRead;
            public override bool CanSeek => baseStream.CanSeek;
            public override bool CanWrite => baseStream.CanWrite;

            public override long Length
            {
                get
                {
                    ThrowIfObjectDisposed();

                    return baseStream.Length;
                }
            }

            public override long Position
            {
                get
                {
                    ThrowIfObjectDisposed();

                    return baseStream.Position;
                }
                set
                {
                    ThrowIfObjectDisposed();

                    baseStream.Position = value;
                }
            }

            public OuputStream(Stream source, Action onDispose)
            {
                if (source == null)
                    throw new ArgumentNullException(nameof(source));

                baseStream = source;
                this.onDispose = onDispose;
            }

            protected override void Dispose(bool disposing)
            {
                if (!isDisposed)
                {
                    onDispose?.Invoke();
                    onDispose = null;

                    isDisposed = true;
                }
            }

            public override void Flush()
            {
                ThrowIfObjectDisposed();

                baseStream.Flush();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                ThrowIfObjectDisposed();

                return baseStream.Read(buffer, offset, count);
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                ThrowIfObjectDisposed();

                return baseStream.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                ThrowIfObjectDisposed();

                baseStream.SetLength(value);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                ThrowIfObjectDisposed();

                baseStream.Write(buffer, offset, count);
            }

            /// <summary>
            /// 若该实例已经被销毁,则返回异常;
            /// </summary>
            protected void ThrowIfObjectDisposed()
            {
                if (isDisposed)
                {
                    throw new ObjectDisposedException(ToString());
                }
            }
        }
    }
}
