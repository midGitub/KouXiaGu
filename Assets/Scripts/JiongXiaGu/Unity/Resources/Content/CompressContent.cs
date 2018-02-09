using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{
    public enum EntryOperation
    {
        None,
        AddOrUpdate,
        Remove,
    }

    /// <summary>
    /// 延迟更新的资源合集;如压缩包资源;
    /// </summary>
    public abstract class CompressContent<T> : Content
        where T : IContentEntry
    {
        private bool isUpdating;
        public override bool IsUpdating => isUpdating;
        public override bool IsCompress => true;

        /// <summary>
        /// 资源入口合集;
        /// </summary>
        protected List<IContentEntry> Entries { get; private set; } = new List<IContentEntry>();

        /// <summary>
        /// 重建资源合集,同时清除所有变更;
        /// </summary>
        internal void RebuildEntriesCollection()
        {
            Entries.Clear();
            Entries.AddRange(EnumerateCompressedEntries());
        }

        /// <summary>
        /// 枚举压缩完毕的资源入口;
        /// </summary>
        public abstract IEnumerable<IContentEntry> EnumerateCompressedEntries();

        /// <summary>
        /// 从压缩包内获取到读取流;
        /// </summary>
        protected abstract Stream GetInputStreamInCompression(T entry);


        #region Read

        public override IEnumerable<IContentEntry> EnumerateEntries()
        {
            return Entries.Where(delegate(IContentEntry entry)
            {
                UpdateEntry updateEntry = entry as UpdateEntry;
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
            var value = EnumerateEntries().FirstOrDefault(entry => entry.Name == name);
            return value;
        }

        public override Stream GetInputStream(IContentEntry entry)
        {
            UpdateEntry updateEntry = entry as UpdateEntry;
            if (updateEntry != null)
            {
                return updateEntry.AsInputStream();
            }
            else
            {
                T _entry = (T)entry;
                return GetInputStreamInCompression(_entry);
            }
        }

        #endregion

        #region Write

        protected abstract void AddEntry(string name, Stream source);
        protected abstract void RemoveEntry(string name);

        public override IDisposable BeginUpdate()
        {
            ThrowIfObjectDisposed();

            isUpdating = true;
            return new ContentCommitUpdateDisposer(this);
        }

        public override void CommitUpdate()
        {
            ThrowIfObjectDisposed();
            ThrowIfObjectNotUpdating();

            foreach (var entry in Entries)
            {
                UpdateEntry updateEntry = entry as UpdateEntry;
                if (updateEntry != null)
                {
                    switch (updateEntry.Operation)
                    {
                        case EntryOperation.AddOrUpdate:
                            AddEntry(updateEntry.Name, updateEntry.GetSource());
                            break;

                        case EntryOperation.Remove:
                            RemoveEntry(updateEntry.Name);
                            break;
                    }
                }
            }

            isUpdating = false;
        }

        protected UpdateEntry CreateAddEntryEvent(string name, Stream source, bool closeStream)
        {
            return new UpdateEntry(name, source, DateTime.Now, closeStream);
        }

        protected UpdateEntry CreateRemoveEntryEvent(string name)
        {
            return new UpdateEntry(name, DateTime.Now);
        }

        public override IContentEntry AddOrUpdate(string name, Stream source, bool closeStream)
        {
            ThrowIfObjectDisposed();
            ThrowIfObjectNotUpdating();

            int index = Entries.FindIndex(entry => entry.Name == name);
            if (index >= 0)
            {
                IContentEntry contentEntry = Entries[index];

                var disposer = contentEntry as IDisposable;
                if (disposer != null)
                {
                    disposer.Dispose();
                }

                var newEntry = CreateAddEntryEvent(name, source, closeStream);
                Entries[index] = newEntry;
                return newEntry;
            }
            else
            {
                var updateEntry = CreateAddEntryEvent(name, source, closeStream);
                Entries.Add(updateEntry);
                return updateEntry;
            }
        }

        public override void Remove(IContentEntry entry)
        {
            ThrowIfObjectDisposed();
            ThrowIfObjectNotUpdating();

            int index = Entries.FindIndex(oldEntry => oldEntry == entry);
            if (index >= 0)
            {
                var updateEntry = CreateRemoveEntryEvent(entry.Name);
                Entries[index] = updateEntry;
            }
        }

        public override Stream GetOutputStream(IContentEntry entry)
        {
            var updateEntry = entry as UpdateEntry;
            if (updateEntry != null)
            {
                return updateEntry.AsOutputStream();
            }
            else
            {
                var memoryStream = new MemoryStream();
                updateEntry = CreateAddEntryEvent(entry.Name, memoryStream, true);

                int index = Entries.FindIndex(oldEntry => oldEntry == entry);
                if (index >= 0)
                {
                    Entries[index] = updateEntry;
                }
                else
                {
                    Entries.Add(updateEntry);
                }

                return updateEntry.AsOutputStream();
            }
        }

        public override Stream GetOutputStream(string name, out IContentEntry entry)
        {
            int index = Entries.FindIndex(oldEntry => oldEntry.Name == name);
            if (index >= 0)
            {
                entry = Entries[index];
                var updateEntry = entry as UpdateEntry;
                if (updateEntry != null)
                {
                    return updateEntry.AsOutputStream();
                }
                else
                {
                    var memoryStream = new MemoryStream();
                    updateEntry = CreateAddEntryEvent(entry.Name, memoryStream, true);
                    Entries[index] = updateEntry;
                    return updateEntry.AsOutputStream();
                }
            }
            else
            {
                var memoryStream = new MemoryStream();
                var updateEntry = CreateAddEntryEvent(name, memoryStream, true);
                entry = updateEntry;
                Entries.Add(entry);
                return updateEntry.AsOutputStream();
            }
        }

        /// <summary>
        /// 压缩的资源入口;
        /// </summary>
        protected class UpdateEntry : IContentEntry, IDisposable
        {
            public string Name { get; private set; }
            public ExclusiveStream Source { get; private set; }
            public EntryOperation Operation { get; private set; }
            public DateTime LastWriteTime { get; private set; }
            public bool IsCloseStream { get; private set; }
            public bool IsDisposed { get; private set; }

            public UpdateEntry(string name, Stream source, DateTime lastWriteTime, bool closeStream)
            {
                Name = name;
                Source = new ExclusiveStream(source);
                Operation = EntryOperation.AddOrUpdate;
                LastWriteTime = lastWriteTime;
                IsCloseStream = closeStream;
            }

            public UpdateEntry(string name, DateTime lastWriteTime)
            {
                Name = name;
                Operation = EntryOperation.Remove;
                LastWriteTime = lastWriteTime;
            }

            public void Dispose()
            {
                if (IsCloseStream && !IsDisposed)
                {
                    Source.Dispose();
                    IsDisposed = true;
                }
            }

            public Stream GetSource()
            {
                return Source.Main;
            }

            public Stream AsInputStream()
            {
                return Source.GetInputStream();
            }

            public Stream AsOutputStream()
            {
                return Source.GetOutputStream();
            }

            public Stream OpenRead()
            {
                return Source.GetInputStream();
            }
        }

        #endregion
    }
}
