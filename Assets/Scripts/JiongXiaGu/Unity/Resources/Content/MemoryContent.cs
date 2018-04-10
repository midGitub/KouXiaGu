using System;
using System.Collections.Generic;
using System.Linq;
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
        private IDictionary<string, MemoryEntry> entries;
        public override bool IsUpdating => isUpdating;
        public override bool CanRead => !isDisposed;
        public override bool CanWrite => !isDisposed;
        public override bool IsDisposed => isDisposed;
        public override bool IsCompress => false;

        public MemoryContent()
        {
            entries = new Dictionary<string, MemoryEntry>(new PathComparer());
        }

        public override void Dispose()
        {
            if (!isDisposed)
            {
                foreach (var entry in entries.Values)
                {
                    entry.Dispose();
                }
                entries = null;

                isDisposed = true;
            }
        }

        public override IEnumerable<IContentEntry> EnumerateEntries()
        {
            ThrowIfObjectDisposed();

            return entries.Values.Cast<IContentEntry>();
        }

        public override IContentEntry GetEntry(string name)
        {
            ThrowIfObjectDisposed();

            MemoryEntry entry;
            entries.TryGetValue(name, out entry);
            return entry;
        }

        public override Stream GetInputStream(string name)
        {
            ThrowIfObjectDisposed();

            MemoryEntry entry;
            if (entries.TryGetValue(name, out entry))
            {
                return entry.OpenRead();
            }
            else
            {
                throw new FileNotFoundException(name);
            }
        }

        public override void BeginUpdate()
        {
            ThrowIfObjectDisposed();

            isUpdating = true;
        }

        public override void CommitUpdate()
        {
            ThrowIfObjectDisposed();
            ThrowIfObjectNotUpdating();

            isUpdating = false;
        }

        public override IContentEntry AddOrUpdate(string name, Stream source, DateTime lastWriteTime, bool closeStream)
        {
            ThrowIfObjectDisposed();
            ThrowIfObjectNotUpdating();

            MemoryEntry oldEntry;
            if (entries.TryGetValue(name, out oldEntry))
            {
                oldEntry.Dispose();
                var entry = entries[name] = new MemoryEntry(name, source, lastWriteTime, closeStream);
                return entry;
            }
            else
            {
                var entry = new MemoryEntry(name, source, lastWriteTime, closeStream);
                entries.Add(name, entry);
                return entry;
            }
        }

        public override bool Remove(string name)
        {
            ThrowIfObjectDisposed();
            ThrowIfObjectNotUpdating();

            MemoryEntry oldEntry;
            if (entries.TryGetValue(name, out oldEntry))
            {
                oldEntry.Dispose();
                entries.Remove(name);
                return true;
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

            MemoryEntry oldEntry;
            if (entries.TryGetValue(name, out oldEntry))
            {
                oldEntry.Dispose();

                var source = new MemoryStream();
                var memoryEntry = new MemoryEntry(name, source, DateTime.Now, true);
                entries[name] = memoryEntry;
                entry = memoryEntry;
                return memoryEntry.Source.GetOutputStream();
            }
            else
            {
                var source = new MemoryStream();
                var memoryEntry = new MemoryEntry(name, source, DateTime.Now, true);
                entry = memoryEntry;
                entries.Add(name, memoryEntry);
                return memoryEntry.Source.GetOutputStream();
            }
        }

        private class MemoryEntry : IContentEntry, IDisposable
        {
            public ExclusiveStream Source { get; private set; }
            public string Name { get; private set; }
            public DateTime LastWriteTime { get; private set; }

            public MemoryEntry(string name, Stream source, DateTime lastWriteTime, bool isCloseStream)
            {
                Name = name;
                Source = new ExclusiveStream(source, isCloseStream);
                LastWriteTime = lastWriteTime;
            }

            public void Dispose()
            {
                if (Source != null)
                {
                    Source.Dispose();
                    Source = null;
                }
            }

            public Stream OpenRead()
            {
                return Source.GetInputStream();
            }
        }
    }
}
