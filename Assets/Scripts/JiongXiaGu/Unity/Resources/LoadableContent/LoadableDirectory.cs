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


        //public override IEnumerable<string> EnumerateFiles()
        //{
        //    return InternalEnumerateFiles();
        //}

        //public override IEnumerable<string> EnumerateFiles(string searchPattern, SearchOption searchOption)
        //{
        //    return InternalEnumerateFiles(searchPattern, searchOption);
        //}

        //public override IEnumerable<string> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        //{
        //    return InternalEnumerateFiles(directoryName, searchPattern, searchOption);
        //}

        //public override string Find(Func<string, bool> func)
        //{
        //    return InternalFind(func);
        //}

        //public override Stream GetInputStream(string relativePath)
        //{
        //    return InternaltInputStream(relativePath);
        //}


        /// <summary>
        /// 枚举所有文件路径;
        /// </summary>
        internal override IEnumerable<string> InternalEnumerateFiles()
        {
            return Directory.EnumerateFiles(DirectoryInfo.FullName, "*", SearchOption.AllDirectories).Select(delegate (string filePath)
            {
                string relativePath = PathHelper.GetRelativePath(DirectoryInfo.FullName, filePath);
                return relativePath;
            });
        }

        internal override IEnumerable<string> InternalEnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFiles(DirectoryInfo.FullName, searchPattern, searchOption).Select(delegate (string filePath)
            {
                string relativePath = PathHelper.GetRelativePath(DirectoryInfo.FullName, filePath);
                return relativePath;
            });
        }

        internal override IEnumerable<string> InternalEnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
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

        internal override Stream InternaltInputStream(string relativePath)
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



        internal override void InternaltBeginUpdate()
        {
        }

        internal override void InternaltCommitUpdate()
        {
        }

        internal override void InternaltAddOrUpdate(string relativePath, Stream stream)
        {
            string filePath = GetFullPath(relativePath);
            var fStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            stream.CopyTo(fStream);
        }

        internal override bool InternaltRemove(string relativePath)
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

        internal override Stream InternaltGetOutStream(string relativePath)
        {
            string filePath = GetFullPath(relativePath);
            return new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        internal override Stream InternaltCreateOutStream(string relativePath)
        {
            string filePath = GetFullPath(relativePath);
            return new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
        }

        internal string GetFullPath(string relativePath)
        {
            string filePath = Path.Combine(DirectoryInfo.FullName, relativePath);
            return filePath;
        }
    }
}
