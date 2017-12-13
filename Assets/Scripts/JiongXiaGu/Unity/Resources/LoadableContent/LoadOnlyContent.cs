using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 支持多线程安全访问的只读内容合集;
    /// </summary>
    public sealed class LoadOnlyContent : IDisposable
    {
        private readonly object asyncLock = new object();
        public LoadableContent LoadableContent { get; private set; }

        public LoadOnlyContent(LoadableContent loadableContent)
        {
            LoadableContent = loadableContent;
        }

        public void Dispose()
        {
            LoadableContent.Dispose();
        }

        /// <summary>
        /// 枚举所有文件信息;
        /// </summary>
        public List<string> EnumerateFiles()
        {
            lock (asyncLock)
            {
                return LoadableContent.ConcurrentEnumerateFiles().ToList();
            }
        }

        /// <summary>
        /// 枚举所有文件;(参考 DirectoryInfo.EnumerateFiles 方法 (String, SearchOption))
        /// </summary>
        /// <param name="searchPattern">可以是文本和通配符的组合字符，但不支持正则表达式</param>
        /// <param name="searchOption">指定搜索操作是应仅包含当前目录还是应包含所有子目录的枚举值之一</param>
        public List<string> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            if (string.IsNullOrWhiteSpace(searchPattern))
                throw new ArgumentNullException(nameof(searchPattern));
            if (searchOption != SearchOption.AllDirectories || searchOption != SearchOption.TopDirectoryOnly)
                throw new IndexOutOfRangeException(nameof(searchOption));

            lock (asyncLock)
            {
                return LoadableContent.ConcurrentEnumerateFiles(searchPattern, searchOption).ToList();
            }
        }

        /// <summary>
        /// 枚举所有文件;(参考 DirectoryInfo.EnumerateFiles 方法 (String, SearchOption))
        /// </summary>
        /// <param name="directoryName">指定目录名称</param>
        /// <param name="searchPattern">可以是文本和通配符的组合字符，但不支持正则表达式</param>
        /// <param name="searchOption">指定搜索操作是应仅包含当前目录还是应包含所有子目录的枚举值之一</param>
        public List<string> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            if (directoryName == null)
                throw new ArgumentNullException(nameof(directoryName));
            if (string.IsNullOrWhiteSpace(searchPattern))
                throw new ArgumentNullException(nameof(searchPattern));
            if (searchOption != SearchOption.AllDirectories || searchOption != SearchOption.TopDirectoryOnly)
                throw new IndexOutOfRangeException(nameof(searchOption));

            lock (asyncLock)
            {
                return LoadableContent.ConcurrentEnumerateFiles(directoryName, searchPattern, searchOption).ToList();
            }
        }

        public string Find(Func<string, bool> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            lock (asyncLock)
            {
                return LoadableContent.ConcurrentFind(func);
            }
        }

        /// <summary>
        /// 获取到只读的流,若未能获取到则返回 FileNotFound 异常;
        /// </summary>
        public Stream GetInputStream(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                throw new ArgumentNullException(nameof(relativePath));

            lock (asyncLock)
            {
                return LoadableContent.ConcurrentGetInputStream(relativePath);
            }
        }

        /// <summary>
        /// 获取到对应的 AssetBundle,若还未读取或不存在则返回null;
        /// </summary>
        public AssetBundle GetAssetBundle(AssetPath path)
        {
            lock (asyncLock)
            {
                return LoadableContent.GetAssetBundle(path);
            }
        }

        /// <summary>
        /// 获取到对应的 AssetBundle,若还未读取,则读取并返回,若不存在 AssetBundle 或读取失败则返回异常;
        /// </summary>
        public AssetBundle GetOrLoadAssetBundle(AssetPath path)
        {
            lock (asyncLock)
            {
                return LoadableContent.GetOrLoadAssetBundle(path);
            }
        }
    }
}
