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
        private List<MemoryEntry> entries;
        public override bool IsUpdating => isUpdating;
        public override bool CanRead => !isDisposed;
        public override bool CanWrite => !isDisposed;
        public override bool IsDisposed => isDisposed;
        public override bool IsCompress => false;

        public MemoryContent()
        {
            entries = new List<MemoryEntry>();
        }

        public override void Dispose()
        {
            if (!isDisposed)
            {
                foreach (var entry in entries)
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

            return entries.Cast<IContentEntry>();
        }

        public override IContentEntry GetEntry(string name)
        {
            ThrowIfObjectDisposed();
            name = Normalize(name);

            var index = FindIndex(name);
            if (index >= 0)
            {
                var entry = entries[index];
                return entry;
            }
            else
            {
                return null;
            }
        }

        public override Stream GetInputStream(string name)
        {
            ThrowIfObjectDisposed();
            name = Normalize(name);

            var index = FindIndex(name);
            if (index >= 0)
            {
                var entry = entries[index];
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
            name = Normalize(name);

            var index = FindIndex(name);
            if (index >= 0)
            {
                var oldEntry = entries[index];
                oldEntry.Dispose();
                var entry = entries[index] = new MemoryEntry(name, source, lastWriteTime, closeStream);
                return entry;
            }
            else
            {
                var entry = new MemoryEntry(name, source, lastWriteTime, closeStream);
                entries.Add(entry);
                return entry;
            }
        }

        public override bool Remove(string name)
        {
            ThrowIfObjectDisposed();
            ThrowIfObjectNotUpdating();
            name = Normalize(name);

            var index = FindIndex(name);
            if (index >= 0)
            {
                var oldEntry = entries[index];
                oldEntry.Dispose();
                entries.RemoveAt(index);
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
            name = Normalize(name);

            var index = FindIndex(name);
            if (index >= 0)
            {
                var oldEntry = entries[index];
                oldEntry.Dispose();

                var source = new MemoryStream();
                var memoryEntry = new MemoryEntry(name, source, DateTime.Now, true);
                entry = entries[index] = memoryEntry;
                return memoryEntry.Source.GetOutputStream();
            }
            else
            {
                var source = new MemoryStream();
                var memoryEntry = new MemoryEntry(name, source, DateTime.Now, true);
                entry = memoryEntry;
                entries.Add(memoryEntry);
                return memoryEntry.Source.GetOutputStream();
            }
        }

        private int FindIndex(string name)
        {
            var index = entries.FindIndex(entry => entry.Name == name);
            return index;
        }


        private class MemoryEntry : IContentEntry, IDisposable
        {
            public ExclusiveStream Source { get; private set; }
            public string Name { get; private set; }
            public DateTime LastWriteTime { get; private set; }

            public MemoryEntry(string name, Stream source, DateTime lastWriteTime, bool isCloseStream)
            {
                Source = new ExclusiveStream(source, isCloseStream);
                Name = name;
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
