//using SharpCompress.Archives.Zip;
//using SharpCompress.Common;
//using SharpCompress.Readers;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.IO.Compression;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using SharpCompress.Archives;

//namespace JiongXiaGu.Unity.Resources
//{

//    public class ZipContent : DelayedUpdateContent
//    {
//        private bool isDisposed = false;
//        private Stream stream;
//        private IArchive archive;
//        private ZipArchive zipArchive;

//        public override bool IsUpdating { get; }
//        public override bool CanRead { get; }
//        public override bool CanWrite { get; }
//        public override bool IsDisposed => isDisposed;

//        public ZipContent(Stream stream)
//        {
//            this.stream = stream;
//            archive = ArchiveFactory.Open(stream);
//        }

//        public override void Dispose()
//        {
//            if (isDisposed)
//            {
//                archive.Dispose();
//                isDisposed = false;
//            }
//        }

//        public override IEnumerable<string> EnumerateFiles()
//        {
//            ThrowIfObjectDisposed();

//            return archive.Entries.Select(entry => entry.Key);
//        }

//        public override Stream GetInputStream(string relativePath)
//        {
//            ThrowIfObjectDisposed();

//            IArchiveEntry entry = GetEntry(relativePath);
//            return entry.OpenEntryStream();
//        }

//        public override IDisposable BeginUpdate()
//        {
//            ThrowIfObjectDisposed();

//            return new ContentCommitUpdateDisposer(this);
//        }

//        public override void CommitUpdate()
//        {
//            ThrowIfObjectDisposed();
//        }

//        public override Stream GetOutputStream(string relativePath)
//        {
//            ThrowIfObjectDisposed();

//            IArchiveEntry entry = GetEntry(relativePath);
//            return entry.OpenEntryStream();
//        }

//        public override bool Remove(string relativePath)
//        {
//            throw new NotImplementedException();
//        }

//        private IArchiveEntry GetEntry(string relativePath)
//        {
//            return archive.Entries.First(entry => entry.Key == relativePath);
//        }
//    }
//}
