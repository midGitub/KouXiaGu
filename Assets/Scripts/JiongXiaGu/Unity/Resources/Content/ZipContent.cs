using SharpCompress.Archives.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JiongXiaGu.Unity.Resources
{

    [Obsolete("使用" + nameof(SharpZipLibContent), true)]
    public class ZipContent : Content
    {
        private string filePath;
        private ZipArchive zipArchive;
        private bool isUpdating;
        private bool isDisposed;

        public override bool IsUpdating => isUpdating;
        public override bool CanRead => true;
        public override bool CanWrite => true;
        public override bool IsDisposed => isDisposed;
        public override bool IsCompress => true;

        public ZipContent(ZipArchive zipArchive)
        {
            this.zipArchive = zipArchive;
        }

        public override void Dispose()
        {
            if (!isDisposed)
            {
                zipArchive.Dispose();

                isDisposed = true;
            }
        }

        public override IEnumerable<IContentEntry> EnumerateEntries()
        {
            ThrowIfObjectDisposed();

            return zipArchive.Entries.Select(zipArchiveEntry => new Entry(zipArchiveEntry) as IContentEntry);
        }

        public override IContentEntry GetEntry(string name)
        {
            ThrowIfObjectDisposed();

            var entry = InternalGetEntry(name);
            return entry != null ? new Entry(entry) as IContentEntry : null;
        }

        internal ZipArchiveEntry InternalGetEntry(string name)
        {
            var entry = zipArchive.Entries.FirstOrDefault(zipArchive => string.Equals(zipArchive.Key, name, StringComparison.OrdinalIgnoreCase));
            return entry;
        }

        internal ZipArchiveEntry InternalGetEntry(IContentEntry contentEntry)
        {
            if (contentEntry is Entry)
            {
                var zipEntry = (Entry)contentEntry;
                return zipEntry.ZipArchiveEntry;
            }
            else
            {
                return InternalGetEntry(contentEntry.Name);
            }
        }

        public override Stream GetInputStream(string name)
        {
            ThrowIfObjectDisposed();

            var entry = InternalGetEntry(name);
            if (entry != null)
            {
                return entry.OpenEntryStream();
            }
            else
            {
                throw new FileNotFoundException(name);
            }
        }

        public override Stream GetInputStream(IContentEntry entry)
        {
            if (entry is Entry)
            {
                var zipEntry = (Entry)entry;
                return zipEntry.ZipArchiveEntry.OpenEntryStream();
            }
            else
            {
                return GetInputStream(entry.Name);
            }
        }

        public override void BeginUpdate()
        {
            throw new NotImplementedException();
        }

        public override void CommitUpdate()
        {
            throw new NotImplementedException();
        }

        public override IContentEntry AddOrUpdate(string name, Stream source, DateTime lastWriteTime, bool isCloseStream = true)
        {
            throw new NotImplementedException();
        }

        public override Stream GetOutputStream(IContentEntry entry)
        {
            return base.GetOutputStream(entry);
        }

        public override Stream GetOutputStream(string name, out IContentEntry entry)
        {
            throw new NotImplementedException();
        }

        public override bool Remove(string name)
        {
            var entry = InternalGetEntry(name);
            if (entry != null)
            {
                zipArchive.RemoveEntry(entry);
                return true;
            }
            else
            {
                return false;
            }
        }

        private struct Entry : IContentEntry
        {
            public ZipArchiveEntry ZipArchiveEntry { get; private set; }
            public string Name => ZipArchiveEntry.Key;
            public DateTime LastWriteTime => ZipArchiveEntry.LastModifiedTime.Value;

            public Entry(ZipArchiveEntry zipArchiveEntry)
            {
                ZipArchiveEntry = zipArchiveEntry;
            }
        }
    }
}
