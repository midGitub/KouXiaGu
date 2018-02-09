using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    public interface IContentInfo
    {
        bool Exists { get; }

        /// <summary>
        /// 获取到合集类;
        /// </summary>
        Content GetContent();
    }

    public class MemoryContentInfo : IContentInfo
    {
        public MemoryContent MemoryContent { get; private set; }
        public bool Exists => true;

        public MemoryContentInfo(MemoryContent memoryContent)
        {
            MemoryContent = memoryContent;
        }

        public Content GetContent()
        {
            return MemoryContent;
        }
    }

    public class DirectoryContentInfo : IContentInfo
    {
        public DirectoryInfo DirectoryInfo { get; private set; }
        public bool Exists => DirectoryInfo.Exists;

        public DirectoryContentInfo(string directoryPath)
        {
            DirectoryInfo = new DirectoryInfo(directoryPath);
        }

        public Content GetContent()
        {
            if (DirectoryInfo.Exists)
            {
                return new DirectoryContent(DirectoryInfo.FullName);
            }
            else
            {
                throw new DirectoryNotFoundException(DirectoryInfo.FullName);
            }
        }
    }

    public class ZipContentInfo : IContentInfo
    {
        public FileInfo ZipFileInfo { get; private set; }
        public bool Exists => ZipFileInfo.Exists;

        public ZipContentInfo(string zipFile)
        {
            ZipFileInfo = new FileInfo(zipFile);
        }

        public Content GetContent()
        {
            if (ZipFileInfo.Exists)
            {
                return new SharpZipLibContent(ZipFileInfo.FullName);
            }
            else
            {
                throw new FileNotFoundException(ZipFileInfo.FullName);
            }
        }
    }
}
