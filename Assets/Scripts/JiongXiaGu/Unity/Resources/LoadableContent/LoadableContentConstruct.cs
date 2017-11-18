using System;
using System.Collections.Generic;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{
    /// <summary>
    /// 抽象类 表示可读资源;
    /// </summary>
    public abstract class LoadableContentConstruct
    {
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
            directoryName = PathHelper.Normalize(directoryName);

            switch (searchOption)
            {
                case SearchOption.AllDirectories:
                    foreach (ILoadableEntry entry in EnumerateFiles())
                    {
                        if (PathHelper.IsCommonRoot(directoryName, entry.RelativePath))
                        {
                            string fileName = PathHelper.GetFileName(entry.RelativePath);
                            if (PathHelper.IsMatch(fileName, searchPattern))
                            {
                                yield return entry;
                            }
                        }
                    }
                    break;

                case SearchOption.TopDirectoryOnly:
                    foreach (ILoadableEntry entry in EnumerateFiles())
                    {
                        if (PathHelper.IsCommonRoot(directoryName, entry.RelativePath))
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
                    }
                    break;

                default:
                    throw new ArgumentException(string.Format("未知的{0}[{1}]", nameof(SearchOption), nameof(searchOption)));
            }
        }

        /// <summary>
        /// 获取到只读的流;
        /// </summary>
        public abstract Stream GetStream(ILoadableEntry entry);
    }
}
