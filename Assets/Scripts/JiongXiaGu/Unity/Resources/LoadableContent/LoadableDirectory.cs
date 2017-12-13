using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections.Concurrent;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 可读取资源的目录;
    /// </summary>
    public class LoadableDirectory : LoadableContent
    {
        /// <summary>
        /// 存放实例;
        /// </summary>
        private static readonly BlockingCollection<string> instancedDirectory = new BlockingCollection<string>();

        public DirectoryInfo DirectoryInfo { get; private set; }

        internal LoadableDirectory(string directory, LoadableContentDescription description) : base(description)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException(directory);
            if (IsInstanced(directory))
                throw new ArgumentException(string.Format("路径[{0}]已经创建为可读取资源;", directory));

            DirectoryInfo = new DirectoryInfo(directory);
        }

        private bool IsInstanced(string directory)
        {
            return instancedDirectory.Contains(directory);
        }


        public override IEnumerable<string> ConcurrentEnumerateFiles()
        {
            return EnumerateFiles();
        }

        public override IEnumerable<string> ConcurrentEnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return EnumerateFiles(searchPattern, searchOption);
        }

        public override IEnumerable<string> ConcurrentEnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            return EnumerateFiles(directoryName, searchPattern, searchOption);
        }

        public override string ConcurrentFind(Func<string, bool> func)
        {
            return Find(func);
        }

        public override Stream ConcurrentGetInputStream(string relativePath)
        {
            return GetInputStream(relativePath);
        }


        /// <summary>
        /// 枚举所有文件路径;
        /// </summary>
        public override IEnumerable<string> EnumerateFiles()
        {
            return Directory.EnumerateFiles(DirectoryInfo.FullName, "*", SearchOption.AllDirectories).Select(delegate (string filePath)
            {
                string relativePath = PathHelper.GetRelativePath(DirectoryInfo.FullName, filePath);
                return relativePath;
            });
        }

        public override IEnumerable<string> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFiles(DirectoryInfo.FullName, searchPattern, searchOption).Select(delegate (string filePath)
            {
                string relativePath = PathHelper.GetRelativePath(DirectoryInfo.FullName, filePath);
                return relativePath;
            });
        }

        public override IEnumerable<string> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            string directory = Path.Combine(DirectoryInfo.FullName, directoryName);

            if (DirectoryInfo.Exists)
            {
                return Directory.EnumerateFiles(directory, searchPattern, searchOption).Select(delegate (string filePath)
                {
                    string relativePath = PathHelper.GetRelativePath(DirectoryInfo.FullName, filePath);
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
            string filePath = Path.Combine(DirectoryInfo.FullName, relativePath);
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
            string filePath = Path.Combine(DirectoryInfo.FullName, relativePath);
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
            string filePath = Path.Combine(DirectoryInfo.FullName, relativePath);
            return filePath;
        }
    }
}
