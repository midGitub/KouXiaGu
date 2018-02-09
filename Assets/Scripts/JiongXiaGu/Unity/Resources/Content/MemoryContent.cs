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
        private Dictionary<string, ExclusiveStream> contents;
        public override bool IsUpdating => isUpdating;
        public override bool CanRead => !isDisposed;
        public override bool CanWrite => !isDisposed;
        public override bool IsDisposed => isDisposed;
        public override bool IsCompress => false;

        public MemoryContent()
        {
            contents = new Dictionary<string, ExclusiveStream>();
        }

        public override void Dispose()
        {
            if (!isDisposed)
            {
                foreach (var content in contents.Values)
                {
                    content.Dispose();
                }
                contents = null;

                isDisposed = true;
            }
        }

        #region Read

        public override IEnumerable<IContentEntry> EnumerateEntries()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> EnumerateFiles()
        {
            ThrowIfObjectDisposed();

            return contents.Keys;
        }

        public override bool Contains(string relativePath)
        {
            ThrowIfObjectDisposed();

            return contents.ContainsKey(relativePath);
        }

        public override IContentEntry GetEntry(string name)
        {
            throw new NotImplementedException();
        }

        public override Stream GetInputStream(string relativePath)
        {
            ThrowIfObjectDisposed();

            ExclusiveStream stream;
            if (contents.TryGetValue(relativePath, out stream))
            {
                var inputStream = stream.GetInputStream();
                return inputStream;
            }
            else
            {
                throw new FileNotFoundException(relativePath);
            }
        }

        public override Stream GetInputStream(IContentEntry entry)
        {
            throw new NotImplementedException();
        }

        #endregion

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

        public override Stream GetOutputStream(string name, out IContentEntry entry)
        {
            throw new NotImplementedException();

            //ThrowIfObjectDisposed();

            //ExclusiveStream stream;
            //if (!contents.TryGetValue(name, out stream))
            //{
            //    stream = new ExclusiveStream();
            //    contents.Add(name, stream);
            //}
            //var outputStream = stream.GetOutputStream();
            //entry = stream;
            //return outputStream;
        }

        public override Stream GetOutputStream(IContentEntry entry)
        {
            throw new NotImplementedException();
        }

        public override IContentEntry AddOrUpdate(string name, Stream source, bool isCloseStream)
        {
            throw new NotImplementedException();
        }

        public override void Remove(IContentEntry entry)
        {
            throw new NotImplementedException();
        }

        public override bool Remove(string relativePath)
        {
            ThrowIfObjectDisposed();

            ExclusiveStream exclusiveStream;
            if (contents.TryGetValue(relativePath, out exclusiveStream))
            {
                exclusiveStream.Dispose();
                return contents.Remove(relativePath);
            }
            return false;
        }


        private class ExclusiveStream : IDisposable
        {
            private bool isDisposed = false;
            private MemoryStream stream;
            private volatile int readerCount = 0;
            private volatile bool isWrite = false;

            public ExclusiveStream()
            {
                stream = new MemoryStream();
            }

            public ExclusiveStream(Stream stream)
            {
                this.stream = new MemoryStream();
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(this.stream);
            }

            public Stream GetInputStream()
            {
                if(isWrite)
                    throw new IOException("Stream已经被占用");

                readerCount++;
                var synchronizedStream = Stream.Synchronized(stream);
                var inputeStream = new InputStream(synchronizedStream, delegate()
                {
                    readerCount--;
                    if (isDisposed && readerCount == 0)
                    {
                        stream.Dispose();
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
                var outputStream = new OutputStream(stream, delegate ()
                {
                    isWrite = false;
                    if (isDisposed)
                    {
                        stream.Dispose();
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
                        stream.Dispose();
                        stream = null;
                    }

                    isDisposed = true;
                }
            }

            /// <summary>
            /// 提供读使用的流;
            /// </summary>
            private class InputStream : Stream
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
                    get { return stream.Position; }
                    set { stream.Position = value; }
                }

                public InputStream(Stream stream, Action onDispose)
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
                private bool isDisposed = false;
                private Stream stream;
                private Action onDispose;
                public override bool CanRead => stream.CanRead;
                public override bool CanSeek => stream.CanSeek;
                public override bool CanWrite => stream.CanWrite;
                public override long Length => stream.Length;

                public override long Position
                {
                    get { return stream.Position; }
                    set { stream.Position = value; }
                }

                public OutputStream(Stream stream, Action onDispose)
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


    public class MemoryContentEntry
    {
        public Stream GetInputStream()
        {
            throw new NotImplementedException();
        }
    }
}
