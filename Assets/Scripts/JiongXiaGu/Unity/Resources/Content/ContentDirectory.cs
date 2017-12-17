﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections.Concurrent;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源目录;
    /// </summary>
    public class ContentDirectory : Content
    {
        /// <summary>
        /// 存放实例;
        /// </summary>
        private static readonly BlockingCollection<string> instancedDirectory = new BlockingCollection<string>();

        private bool isUpdating;
        private bool isDisposed;
        public DirectoryInfo DirectoryInfo { get; private set; }

        public override bool IsUpdating => isUpdating;
        public override bool IsDisposed => isDisposed;
        public override bool CanRead => !isDisposed;
        public override bool CanWrite => !isDisposed;

        internal ContentDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException(directory);
            if (IsInstanced(directory))
                throw new ArgumentException(string.Format("路径[{0}]已经创建为可读取资源;", directory));

            DirectoryInfo = new DirectoryInfo(directory);
        }

        private static bool IsInstanced(string directory)
        {
            return instancedDirectory.Contains(directory);
        }

        /// <summary>
        /// 枚举所有文件路径;
        /// </summary>
        public override IEnumerable<string> EnumerateFiles()
        {
            ThrowIfObjectDisposed();
            return Directory.EnumerateFiles(DirectoryInfo.FullName, "*", SearchOption.AllDirectories).Select(delegate (string filePath)
            {
                string relativePath = PathHelper.GetRelativePath(DirectoryInfo.FullName, filePath);
                return relativePath;
            });
        }

        public override IEnumerable<string> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();
            return Directory.EnumerateFiles(DirectoryInfo.FullName, searchPattern, searchOption).Select(delegate (string filePath)
            {
                string relativePath = PathHelper.GetRelativePath(DirectoryInfo.FullName, filePath);
                return relativePath;
            });
        }

        public override IEnumerable<string> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();
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
            ThrowIfObjectDisposed();
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


        public override IDisposable BeginUpdate()
        {
            ThrowIfObjectDisposed();
            isUpdating = true;
            return new CommitUpdateDisposer(this);
        }

        public override void CommitUpdate()
        {
            ThrowIfObjectDisposed();
            isUpdating = false;
        }

        public override void AddOrUpdate(string relativePath, Stream stream)
        {
            ThrowIfObjectDisposed();
            string filePath = GetFullPath(relativePath);
            var fStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            stream.CopyTo(fStream);
        }

        public override bool Remove(string relativePath)
        {
            ThrowIfObjectDisposed();
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
            ThrowIfObjectDisposed();
            string filePath = GetFullPath(relativePath);
            return new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        public override Stream CreateOutStream(string relativePath)
        {
            ThrowIfObjectDisposed();
            string filePath = GetFullPath(relativePath);
            return new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
        }

        private string GetFullPath(string relativePath)
        {
            string filePath = Path.Combine(DirectoryInfo.FullName, relativePath);
            return filePath;
        }

        public override void Dispose()
        {
            isDisposed = true;
        }
    }
}