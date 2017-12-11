using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 可读取资源的目录;
    /// </summary>
    public class LoadableDirectory : LoadableContent
    {
        internal readonly DirectoryInfo directoryInfo;

        public LoadableDirectory(string directory, LoadableContentDescription description) : base(description)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException(directory);

            directoryInfo = new DirectoryInfo(directory);
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override IEnumerable<string> EnumerateFiles()
        {
            return Directory.EnumerateFiles(directoryInfo.FullName, "*", SearchOption.AllDirectories).Select(delegate (string filePath)
            {
                string relativePath = PathHelper.GetRelativePath(directoryInfo.FullName, filePath);
                return relativePath;
            });
        }

        public override IEnumerable<string> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFiles(directoryInfo.FullName, searchPattern, searchOption).Select(delegate (string filePath)
            {
                string relativePath = PathHelper.GetRelativePath(directoryInfo.FullName, filePath);
                return relativePath;
            });
        }

        public override IEnumerable<string> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            string directory = Path.Combine(this.directoryInfo.FullName, directoryName);

            if (directoryInfo.Exists)
            {
                return Directory.EnumerateFiles(directory, searchPattern, searchOption).Select(delegate (string filePath)
                {
                    string relativePath = PathHelper.GetRelativePath(directoryInfo.FullName, filePath);
                    return relativePath;
                });
            }
            else
            {
                return new string[0];
            }
        }

        public override Stream GetInputStream(string relativePath)
        {
            string filePath = Path.Combine(directoryInfo.FullName, relativePath);
            if (File.Exists(filePath))
            {
                return new FileStream(filePath, FileMode.Open, FileAccess.Read);
            }
            else
            {
                throw new FileNotFoundException(filePath);
            }
        }


        public override void BeginUpdate()
        {
        }

        public override void CommitUpdate()
        {
        }

        public override void AddOrUpdate(string relativePath, Stream stream)
        {
            string filePath = GetFullPath(relativePath);
            var fStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            stream.CopyTo(fStream);
        }

        public override bool Remove(string relativePath)
        {
            string filePath = Path.Combine(directoryInfo.FullName, relativePath);
            if (File.Exists(relativePath))
            {
                File.Delete(filePath);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override Stream GetOutStream(string relativePath)
        {
            string filePath = GetFullPath(relativePath);
            return new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        public override Stream CreateOutStream(string relativePath)
        {
            string filePath = GetFullPath(relativePath);
            return new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
        }

        private string GetFullPath(string relativePath)
        {
            string filePath = Path.Combine(directoryInfo.FullName, relativePath);
            return filePath;
        }
    }
}
