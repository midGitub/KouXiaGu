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
        public Stream Main { get; private set; }

        public ExclusiveStream(Stream soure)
        {
            if (soure == null)
                throw new ArgumentNullException(nameof(soure));

            Main = soure;
        }

        public Stream GetInputStream()
        {
            if (isWrite)
                throw new IOException("Stream已经被占用");

            readerCount++;
            var synchronizedStream = Stream.Synchronized(Main);
            var inputeStream = new ReadOnlyStream(synchronizedStream, delegate ()
            {
                readerCount--;
                if (isDisposed && readerCount == 0)
                {
                    Main.Dispose();
                }
            });
            inputeStream.Seek(0, SeekOrigin.Begin);
            return inputeStream;
        }

        public Stream GetOutputStream()
        {
            if (readerCount > 0 || isWrite)
                throw new IOException("Stream已经被占用");

            isWrite = true;
            var synchronizedStream = Stream.Synchronized(Main);
            var outputStream = new EditOnlyStream(synchronizedStream, delegate ()
            {
                isWrite = false;
                if (isDisposed)
                {
                    Main.Dispose();
                }
                else
                {
                    Main.Position = 0;
                }
            });
            outputStream.Seek(0, SeekOrigin.Begin);
            return outputStream;
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                if (!isWrite || readerCount == 0)
                {
                    Main.Dispose();
                    Main = null;
                }

                isDisposed = true;
            }
        }


        /// <summary>
        /// 用于提供只读的流,在 Dispose() 之后重置内部流的位置到起点;
        /// </summary>
        internal class ReadOnlyStream : Stream
        {
            private bool isDisposed = false;
            private Stream stream;
            private Action onDispose;
            public override bool CanRead => stream.CanRead;
            public override bool CanSeek => stream.CanSeek;
            public override bool CanWrite => false;
            public override long Length => stream.Length;

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

            public ReadOnlyStream(Stream stream, Action onDispose)
            {
                this.stream = stream;
                this.onDispose = onDispose;
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                if (!isDisposed)
                {
                    onDispose.Invoke();
                    isDisposed = true;
                }
            }

            public override void Flush()
            {
                ThrowIfObjectDisposed();

                stream.Flush();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                ThrowIfObjectDisposed();

                return stream.Read(buffer, offset, count);
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

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
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

        /// <summary>
        /// 提供写入使用的流;
        /// </summary>
        private class EditOnlyStream : Stream
        {
            private bool isDisposed = false;
            private Stream stream;
            private Action onDispose;
            public override bool CanRead => stream.CanRead;
            public override bool CanSeek => stream.CanSeek;
            public override bool CanWrite => stream.CanWrite;
            public override long Length => stream.Length;

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

            public EditOnlyStream(Stream stream, Action onDispose)
            {
                this.stream = stream;
                this.onDispose = onDispose;
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                if (!isDisposed)
                {
                    onDispose.Invoke();
                    isDisposed = true;
                }
            }

            public override void Flush()
            {
                ThrowIfObjectDisposed();

                stream.Flush();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                ThrowIfObjectDisposed();

                return stream.Read(buffer, offset, count);
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

            public override void Write(byte[] buffer, int offset, int count)
            {
                ThrowIfObjectDisposed();

                stream.Write(buffer, offset, count);
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
