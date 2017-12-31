using JiongXiaGu.Collections;
using System;
using System.Collections.Generic;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 在内存中创建的资源合集;
    /// </summary>
    public class MemoryContent : Content
    {
        private bool isDisposed = false;
        private bool isUpdating = false;
        private Dictionary<string, Stream> content;

        public override bool IsUpdating => isUpdating;
        public override bool CanRead => !isDisposed;
        public override bool CanWrite => !isDisposed;
        public override bool IsDisposed => isDisposed;

        public MemoryContent()
        {
            content = new Dictionary<string, Stream>();
        }

        public override void Dispose()
        {
            if (!isDisposed)
            {
                content = null;
                isDisposed = true;
            }
        }

        public override IEnumerable<string> EnumerateFiles()
        {
            ThrowIfObjectDisposed();

            return content.Keys;
        }

        public override bool Contains(string relativePath)
        {
            ThrowIfObjectDisposed();

            return content.ContainsKey(relativePath);
        }

        public override Stream GetInputStream(string relativePath)
        {
            ThrowIfObjectDisposed();

            Stream stream;
            if (content.TryGetValue(relativePath, out stream))
            {
                var inputStream = new InputStream(stream);
                return inputStream;
            }
            else
            {
                throw new FileNotFoundException(relativePath);
            }
        }

        public override IDisposable BeginUpdate()
        {
            ThrowIfObjectDisposed();

            isUpdating = true;
            return new ContentCommitUpdateDisposer(this);
        }

        public override void CommitUpdate()
        {
            ThrowIfObjectDisposed();

            isUpdating = false;
        }

        public override Stream GetOutputStream(string relativePath)
        {
            ThrowIfObjectDisposed();

            Stream stream;
            if (!content.TryGetValue(relativePath, out stream))
            {
                stream = new MemoryStream();
                content.Add(relativePath, stream);
            }
            var outputStream = new OutputStream(stream);
            return outputStream;
        }

        public override Stream CreateOutputStream(string relativePath)
        {
            ThrowIfObjectDisposed();

            Stream stream = new MemoryStream();
            if (content.ContainsKey(relativePath))
            {
                content[relativePath] = stream;
            }
            else
            {
                content.Add(relativePath, stream);
            }
            var outputStream = new OutputStream(stream);
            return outputStream;
        }

        public override void AddOrUpdate(string relativePath, Stream stream)
        {
            ThrowIfObjectDisposed();
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            content.AddOrUpdate(relativePath, stream);
        }

        public override bool Remove(string relativePath)
        {
            ThrowIfObjectDisposed();

            return content.Remove(relativePath);
        }

        /// <summary>
        /// 提供读使用的流;
        /// </summary>
        private class InputStream : Stream
        {
            private Stream stream;

            public override bool CanRead => stream.CanRead;
            public override bool CanSeek => stream.CanSeek;
            public override bool CanWrite => false;
            public override long Length => stream.Length;

            public override long Position
            {
                get { return stream.Position; }
                set { stream.Position = value; }
            }

            public InputStream(Stream stream)
            {
                stream.Seek(0, SeekOrigin.Begin);
                this.stream = stream;
            }

            public override void Flush()
            {
                stream.Flush();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return stream.Read(buffer, offset, count);
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return stream.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                stream.SetLength(value);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// 提供写入使用的流;
        /// </summary>
        private class OutputStream : Stream
        {
            private Stream stream;
            public override bool CanRead => stream.CanRead;
            public override bool CanSeek => stream.CanSeek;
            public override bool CanWrite => stream.CanWrite;
            public override long Length => stream.Length;

            public override long Position
            {
                get { return stream.Position; }
                set { stream.Position = value; }
            }

            public OutputStream(Stream stream)
            {
                stream.Seek(0, SeekOrigin.Begin);
                this.stream = stream;
            }

            public override void Flush()
            {
                stream.Flush();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return stream.Read(buffer, offset, count);
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return stream.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                stream.SetLength(value);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                stream.Write(buffer, offset, count);
            }
        }
    }
}
