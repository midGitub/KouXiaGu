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
    public class SharpZipLibContent : Content
    {
        /// <summary>
        /// 压缩文件;
        /// </summary>
        private ZipFile zipFile;

        /// <summary>
        /// 压缩文件流;
        /// </summary>
        private Stream stream;

        /// <summary>
        /// 资源入口合集;
        /// </summary>
        private IDictionary<string, IContentEntry> entries;

        private bool isDisposed;
        public override bool IsUpdating => zipFile.IsUpdating;
        public override bool CanRead => !isDisposed;
        public override bool CanWrite => !isDisposed;
        public override bool IsDisposed => isDisposed;
        public override bool IsCompress => true;

        /// <summary>
        /// 指定压缩文件路径构建;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ZipException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public SharpZipLibContent(string zipFilePath)
        {
            stream = new FileStream(zipFilePath, FileMode.Open, FileAccess.ReadWrite);
            zipFile = new ZipFile(stream);
            entries = new Dictionary<string, IContentEntry>(new PathComparer());
            RebuildEntriesCollection();
        }

        /// <summary>
        /// 从指定流创建;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ZipException"></exception>
        public SharpZipLibContent(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            this.stream = stream;
            zipFile = new ZipFile(stream);
            entries = new Dictionary<string, IContentEntry>(new PathComparer());
            RebuildEntriesCollection();
        }

        /// <summary>
        /// 指定参数构建;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public SharpZipLibContent(Stream stream, ZipFile zipFile)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (zipFile == null)
                throw new ArgumentNullException(nameof(zipFile));

            this.stream = stream;
            this.zipFile = zipFile;
            entries = new Dictionary<string, IContentEntry>(new PathComparer());
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

                foreach (var entry in entries.Values)
                {
                    var disposer = entry as IDisposable;
                    if (disposer != null)
                    {
                        disposer.Dispose();
                    }
                }

                entries = null;

                isDisposed = true;
            }
        }

        private void ClearEntriesCollection()
        {
            foreach (var entry in entries.Values)
            {
                var disposer = entry as IDisposable;
                if (disposer != null)
                {
                    disposer.Dispose();
                }
            }
            entries.Clear();
        }

        /// <summary>
        /// 重构资源入口合集,将会清除未保存的资源;
        /// </summary>
        internal void RebuildEntriesCollection()
        {
            if (IsUpdating)
                throw new InvalidOperationException("合集正在更新");

            ClearEntriesCollection();

            for (int index = 0; index < zipFile.Count; index++)
            {
                ZipEntry zipEntry = zipFile[index];
                entries.Add(zipEntry.Name, new Entry(zipEntry));
            }
        }

        /// <summary>
        /// 枚举压缩完毕的资源入口;
        /// </summary>
        private IEnumerable<Entry> EnumerateCompressedEntries()
        {
            return zipFile.Cast<ZipEntry>().Where(entry => entry.IsFile).Select(entry => new Entry(entry));
        }
        
        public override IEnumerable<IContentEntry> EnumerateEntries()
        {
            ThrowIfObjectDisposed();

            return entries.Values.Where(delegate (IContentEntry entry)
            {
                var updateEntry = entry as UpdateEntry;
                if (updateEntry != null)
                {
                    return updateEntry.Operation != EntryOperation.Remove;
                }
                else
                {
                    return true;
                }
            });
        }

        public override IContentEntry GetEntry(string name)
        {
            ThrowIfObjectDisposed();

            IContentEntry entry;
            entries.TryGetValue(name, out entry);
            return entry;
        }

        public override Stream GetInputStream(string name)
        {
            ThrowIfObjectDisposed();

            IContentEntry entry;
            if (entries.TryGetValue(name, out entry))
            {
                return GetInputStream(entry);
            }
            else
            {
                throw new FileNotFoundException(name);
            }
        }

        public override Stream GetInputStream(IContentEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));
            ThrowIfObjectDisposed();

            Entry zipContentEntry = entry as Entry;
            if (zipContentEntry != null)
            {
                return zipFile.GetInputStream(zipContentEntry.ZipEntry);
            }
            else
            {
                return GetInputStream(entry.Name);
            }
        }


        public override void BeginUpdate()
        {
            ThrowIfObjectDisposed();

            zipFile.BeginUpdate();
        }

        public override void CommitUpdate()
        {
            ThrowIfObjectDisposed();
            ThrowIfObjectNotUpdating();

            foreach (var entry in entries.Values)
            {
                var updateEntry = entry as UpdateEntry;
                if (updateEntry != null)
                {
                    switch (updateEntry.Operation)
                    {
                        case EntryOperation.AddOrUpdate:
                            zipFile.Add(updateEntry, updateEntry.Name);
                            break;

                        case EntryOperation.Remove:
                            zipFile.Delete(updateEntry.Name);
                            break;

                        default:
                            throw new IndexOutOfRangeException(updateEntry.Operation.ToString());
                    }
                }
            }

            zipFile.CommitUpdate();
            RebuildEntriesCollection();
        }

        public override IContentEntry AddOrUpdate(string name, Stream source, DateTime lastWriteTime, bool isCloseStream = true)
        {
            ThrowIfObjectDisposed();
            ThrowIfObjectNotUpdating();
            name = NormalizePath(name);

            IContentEntry oldEntry;
            if (entries.TryGetValue(name, out oldEntry))
            {
                if (oldEntry is IDisposable)
                {
                    (oldEntry as IDisposable).Dispose();
                }

                var updateEntry = new UpdateEntry(name, source, lastWriteTime, isCloseStream, EntryOperation.AddOrUpdate);
                entries[name] = updateEntry;
                return updateEntry;
            }
            else
            {
                var updateEntry = new UpdateEntry(name, source, lastWriteTime, isCloseStream, EntryOperation.AddOrUpdate);
                entries.Add(name, updateEntry);
                return updateEntry;
            }
        }

        public override bool Remove(string name)
        {
            ThrowIfObjectDisposed();
            ThrowIfObjectNotUpdating();
            name = NormalizePath(name);

            IContentEntry entry;
            if (entries.TryGetValue(name, out entry))
            {
                var updateEntry = entry as UpdateEntry;
                if (updateEntry != null)
                {
                    updateEntry.Dispose();

                    switch (updateEntry.Operation)
                    {
                        case EntryOperation.AddOrUpdate:
                            entries.Remove(name);
                            return true;

                        case EntryOperation.Remove:
                            return true;

                        default:
                            throw new IndexOutOfRangeException(updateEntry.Operation.ToString());
                    }
                }
                else
                {
                    updateEntry = new UpdateEntry(name, EntryOperation.Remove);
                    entries[name] = updateEntry;
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public override Stream GetOutputStream(string name, out IContentEntry entry)
        {
            ThrowIfObjectDisposed();
            ThrowIfObjectNotUpdating();
            name = NormalizePath(name);

            IContentEntry oldEntry;
            if (entries.TryGetValue(name, out oldEntry))
            {
                if (oldEntry is IDisposable)
                {
                    (oldEntry as IDisposable).Dispose();
                }

                var source = new MemoryStream();
                var updateEntry = new UpdateEntry(name, source, DateTime.Now, true, EntryOperation.AddOrUpdate);
                entries[name] = updateEntry;
                entry = updateEntry;
                return updateEntry.Source.GetOutputStream();
            }
            else
            {
                var source = new MemoryStream();
                var updateEntry = new UpdateEntry(name, source, DateTime.Now, true, EntryOperation.AddOrUpdate);
                entry = updateEntry;
                entries.Add(name, updateEntry);
                return updateEntry.Source.GetOutputStream();
            }
        }

        private class Entry : IContentEntry
        {
            public ZipEntry ZipEntry { get; private set; }
            public string Name => ZipEntry.Name;
            public DateTime LastWriteTime => ZipEntry.DateTime;

            public Entry(ZipEntry zipEntry)
            {
                ZipEntry = zipEntry;
            }
        }

        private class UpdateEntry : IContentEntry, IStaticDataSource, IDisposable
        {
            public string Name { get; private set; }
            public DateTime LastWriteTime { get; private set; }
            public ExclusiveStream Source { get; private set; }
            public EntryOperation Operation { get; private set; }
            public bool IsDisposed { get; private set; }

            public UpdateEntry(string name, EntryOperation operation)
            {
                Name = name;
                Operation = operation;
            }

            public UpdateEntry(string name, Stream source, DateTime lastWriteTime, bool isCloseStream, EntryOperation operation)
            {
                Name = name;
                LastWriteTime = lastWriteTime;
                Source = new ExclusiveStream(source, isCloseStream);
                Operation = operation;
            }

            public void Dispose()
            {
                if (!IsDisposed)
                {
                    Source?.Dispose();
                    Source = null;

                    IsDisposed = true;
                }
            }

            public Stream GetSource()
            {
                return Source.GetInputStream();
            }
        }
    }
}
