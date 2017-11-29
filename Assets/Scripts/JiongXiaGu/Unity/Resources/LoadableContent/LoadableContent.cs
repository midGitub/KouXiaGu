using System;
using System.Collections.Generic;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 抽象类 表示可读资源; 对于"获取到流"的方法为线程安全的;
    /// </summary>
    public abstract class LoadableContent
    {
        /// <summary>
        /// 模组描述;
        /// </summary>
        public LoadableContentDescription Description { get; protected set; }

        /// <summary>
        /// 资源类型;DLC 或 MOD;
        /// </summary>
        public LoadableContentType Type { get; protected set; }

        /// <summary>
        /// 该资源是否为压缩的?
        /// </summary>
        public abstract bool Compressed { get; }

        protected LoadableContent()
        {
        }

        protected LoadableContent(LoadableContentDescription description, LoadableContentType type)
        {
            Description = description;
            Type = type;
        }

        /// <summary>
        /// 卸载此资源;
        /// </summary>
        public abstract void Unload();

        /// <summary>
        /// 枚举所有文件信息;
        /// </summary>
        public abstract IEnumerable<ILoadableEntry> EnumerateFiles();

        /// <summary>
        /// 枚举所有文件;(参考 DirectoryInfo.EnumerateFiles 方法 (String, SearchOption))
        /// </summary>
        /// <param name="searchPattern">可以是文本和通配符的组合字符，但不支持正则表达式</param>
        /// <param name="searchOption">指定搜索操作是应仅包含当前目录还是应包含所有子目录的枚举值之一</param>
        public virtual IEnumerable<ILoadableEntry> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            switch (searchOption)
            {
                case SearchOption.AllDirectories:
                    foreach (ILoadableEntry entry in EnumerateFiles())
                    {
                        string fileName = PathHelper.GetFileName(entry.RelativePath);
                        if (PathHelper.IsMatch(fileName, searchPattern))
                        {
                            yield return entry;
                        }
                    }
                    break;

                case SearchOption.TopDirectoryOnly:
                    foreach (ILoadableEntry entry in EnumerateFiles())
                    {
                        if (PathHelper.IsFileName(entry.RelativePath))
                        {
                            string fileName = PathHelper.GetFileName(entry.RelativePath);
                            if (PathHelper.IsMatch(fileName, searchPattern))
                            {
                                yield return entry;
                            }
                        }
                    }
                    break;

                default:
                    throw new ArgumentException(string.Format("未知的{0}[{1}]", nameof(SearchOption), nameof(searchOption)));
            }
        }

        /// <summary>
        /// 枚举所有文件;(参考 DirectoryInfo.EnumerateFiles 方法 (String, SearchOption))
        /// </summary>
        /// <param name="directoryName">指定目录名称</param>
        /// <param name="searchPattern">可以是文本和通配符的组合字符，但不支持正则表达式</param>
        /// <param name="searchOption">指定搜索操作是应仅包含当前目录还是应包含所有子目录的枚举值之一</param>
        public virtual IEnumerable<ILoadableEntry> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            string fileName;
            directoryName = PathHelper.Normalize(directoryName);

            switch (searchOption)
            {
                case SearchOption.AllDirectories:
                    foreach (ILoadableEntry entry1 in EnumerateFiles())
                    {
                        if (PathHelper.IsCommonRoot(directoryName, entry1.RelativePath))
                        {
                            fileName = PathHelper.GetFileName(entry1.RelativePath);
                            if (PathHelper.IsMatch(fileName, searchPattern))
                            {
                                yield return entry1;
                            }
                        }
                    }
                    break;

                case SearchOption.TopDirectoryOnly:
                    foreach (ILoadableEntry entry2 in EnumerateFiles())
                    {
                        if (PathHelper.IsCommonRoot(directoryName, entry2.RelativePath))
                        {
                            fileName = PathHelper.GetFileName(entry2.RelativePath);
                            if (PathHelper.IsMatch(fileName, searchPattern))
                            {
                                yield return entry2;
                            }
                        }
                    }
                    break;

                default:
                    throw new ArgumentException(string.Format("未知的{0}[{1}]", nameof(SearchOption), nameof(searchOption)));
            }
        }

        /// <summary>
        /// 获取到对应入口,若不存在则返回null;
        /// </summary>
        public virtual ILoadableEntry GetEntry(string relativePath)
        {
            foreach (var entry in EnumerateFiles())
            {
                if (entry.RelativePath == relativePath)
                {
                    return entry;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取到只读的流;(线程安全)
        /// </summary>
        public abstract Stream GetInputStream(ILoadableEntry entry);

        /// <summary>
        /// 获取到完整的文件路径;(线程安全)
        /// </summary>
        public abstract string GetFile(ILoadableEntry entry);

        /// <summary>
        /// 获取到唯一的缓存目录;
        /// </summary>
        public string GetCacheDirectory()
        {
            string name = "MOD_" + Description.ID;
            string cacheDirectory = Path.Combine(ResourcePath.CacheDirectory, name);
            return cacheDirectory;
        }

        ///// <summary>
        ///// 移除临时目录;
        ///// </summary>
        //public void DeleteCacheDirectory()
        //{
        //    string cacheDirectory = GetCacheDirectory();
        //    Directory.Delete(cacheDirectory, true);
        //}
    }
}
