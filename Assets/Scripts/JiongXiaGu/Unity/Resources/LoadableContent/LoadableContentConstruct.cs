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
        /// 是否可读?
        /// </summary>
        public abstract bool IsLoadable { get; }

        /// <summary>
        /// 是否存在?
        /// </summary>
        public abstract bool Exists { get; }

        /// <summary>
        /// 加载资源;
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// 卸载资源;
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// 枚举所有文件;(参考 DirectoryInfo.EnumerateFiles 方法 (String, SearchOption))
        /// </summary>
        /// <param name="directoryName">指定目录名称</param>
        /// <param name="searchPattern">可以是文本和通配符的组合字符，但不支持正则表达式</param>
        /// <param name="searchOption">指定搜索操作是应仅包含当前目录还是应包含所有子目录的枚举值之一</param>
        public virtual IEnumerable<ILoadableEntry> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取到只读的流;
        /// </summary>
        public abstract Stream GetStream(ILoadableEntry entry);
    }
}
