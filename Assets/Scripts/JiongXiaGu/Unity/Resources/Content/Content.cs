using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 抽象的资源合集;
    /// </summary>
    public abstract class Content : IContent, IReadOnlyContent, IDisposable
    {
        /// <summary>
        /// 是否正在更新改内容?
        /// </summary>
        public abstract bool IsUpdating { get; }
        public abstract bool CanRead { get; }
        public abstract bool CanWrite { get; }
        public abstract bool IsDisposed { get; }

        /// <summary>
        /// 资源合集是否为压缩的形式?
        /// </summary>
        public abstract bool IsCompress { get; }

        /// <summary>
        /// 释放所有资源;
        /// </summary>
        public abstract void Dispose();

        #region Read

        /// <summary>
        /// 枚举所有资源入口;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        public abstract IEnumerable<IContentEntry> EnumerateEntries();

        /// <summary>
        /// 筛选符合要求的资源入口;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException">未知的 SearchOption</exception>
        public virtual IEnumerable<IContentEntry> EnumerateEntries(string searchPattern, SearchOption searchOption)
        {
            return EnumerateEntries(EnumerateEntries(), searchPattern, searchOption);
        }

        /// <summary>
        /// 筛选符合要求的资源入口;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException">未知的 SearchOption</exception>
        protected IEnumerable<IContentEntry> EnumerateEntries(IEnumerable<IContentEntry> entries, string searchPattern, SearchOption searchOption)
        {
            if (searchPattern == null)
                throw new ArgumentNullException(nameof(searchPattern));

            switch (searchOption)
            {
                case SearchOption.AllDirectories:
                    foreach (var entry in entries)
                    {
                        string fileName = Path.GetFileName(entry.Name);
                        if (PathHelper.IsMatch(fileName, searchPattern))
                        {
                            yield return entry;
                        }
                    }
                    break;

                case SearchOption.TopDirectoryOnly:
                    foreach (var entry in entries)
                    {
                        if (PathHelper.NonPath(entry.Name))
                        {
                            string fileName = Path.GetFileName(entry.Name);
                            if (PathHelper.IsMatch(fileName, searchPattern))
                            {
                                yield return entry;
                            }
                        }
                    }
                    break;

                default:
                    throw new IndexOutOfRangeException(string.Format("未知的{0}[{1}]", nameof(SearchOption), nameof(searchOption)));
            }
        }

        /// <summary>
        /// 筛选符合要求的资源入口;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException">未知的 SearchOption</exception>
        public virtual IEnumerable<IContentEntry> EnumerateEntries(string directoryName, string searchPattern, SearchOption searchOption)
        {
            return EnumerateEntries(EnumerateEntries(), directoryName, searchPattern, searchOption);
        }

        /// <summary>
        /// 筛选符合要求的资源入口;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException">未知的 SearchOption</exception>
        protected IEnumerable<IContentEntry> EnumerateEntries(IEnumerable<IContentEntry> entries, string directoryName, string searchPattern, SearchOption searchOption)
        {
            if (directoryName == null)
                throw new ArgumentNullException(nameof(directoryName));
            if (searchPattern == null)
                throw new ArgumentNullException(nameof(searchPattern));

            switch (searchOption)
            {
                case SearchOption.AllDirectories:
                    foreach (IContentEntry entry in entries)
                    {
                        if (PathHelper.IsCommonRoot(directoryName, entry.Name))
                        {
                            string fileName = Path.GetFileName(entry.Name);
                            if (PathHelper.IsMatch(fileName, searchPattern))
                            {
                                yield return entry;
                            }
                        }
                    }
                    break;

                case SearchOption.TopDirectoryOnly:
                    foreach (IContentEntry entry in entries)
                    {
                        if (PathHelper.IsCommonRoot(directoryName, entry.Name))
                        {
                            string relativePath = PathHelper.GetRelativePath(directoryName, entry.Name);
                            if (PathHelper.NonPath(relativePath))
                            {
                                string fileName = Path.GetFileName(entry.Name);
                                if (PathHelper.IsMatch(fileName, searchPattern))
                                {
                                    yield return entry;
                                }
                            }
                        }
                    }
                    break;

                default:
                    throw new IndexOutOfRangeException(string.Format("未知的{0}[{1}]", nameof(SearchOption), nameof(searchOption)));
            }
        }

        /// <summary>
        /// 获取到所有文件相对路径;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        public virtual IEnumerable<string> EnumerateFiles()
        {
            return EnumerateEntries().Select(entry => entry.Name);
        }

        /// <summary>
        /// 获取到所有文件相对路径;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException">未知的 SearchOption</exception>
        public virtual IEnumerable<string> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return EnumerateEntries(searchPattern, searchOption).Select(entry => entry.Name);
        }

        /// <summary>
        /// 获取到所有文件相对路径;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException">未知的 SearchOption</exception>
        public virtual IEnumerable<string> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            return EnumerateEntries(directoryName, searchPattern, searchOption).Select(entry => entry.Name);
        }

        /// <summary>
        /// 获取到资源入口,若不存在则返回null;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        public virtual IContentEntry GetEntry(string name)
        {
            ThrowIfObjectDisposed();
            name = Normalize(name);

            foreach (var entry in EnumerateEntries())
            {
                if (entry.Name == name)
                {
                    return entry;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取到对应数据的流,在使用完毕之后需要手动释放;(推荐在using语法内使用)
        /// </summary>
        /// <exception cref="FileNotFoundException">未找到指定路径的流</exception>
        /// <exception cref="IOException">文件被占用</exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidCastException">入口类型错误</exception>
        public virtual Stream GetInputStream(IContentEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            return GetInputStream(entry.Name);
        }

        /// <summary>
        /// 获取到对应数据的流,在使用完毕之后需要手动释放;(推荐在using语法内使用)
        /// </summary>
        /// <exception cref="FileNotFoundException">未找到指定路径的流</exception>
        /// <exception cref="IOException">文件被占用</exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public abstract Stream GetInputStream(string name);

        /// <summary>
        /// 确定是否存在该路径;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        public virtual bool Contains(string name)
        {
            ThrowIfObjectDisposed();
            name = Normalize(name);

            return GetEntry(name) != null;
        }

        /// <summary>
        /// 搜索指定路径,直到返回true停止,若未找到则返回 null;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        public virtual IContentEntry Find(Func<IContentEntry, bool> func)
        {
            ThrowIfObjectDisposed();
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            foreach (var entry in EnumerateEntries())
            {
                if (func.Invoke(entry))
                {
                    return entry;
                }
            }
            return null;
        }

        #endregion

        #region Write

        /// <summary>
        /// 在更改内容之前需要先调用,直到调用 CommitUpdate() 进行修改操作;(推荐在Using语句内使用该方法,在using语句内使用,则无需手动调用 CommitUpdate() 结束更改,否则在调用 CommitUpdate() 时可能返回异常)
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        public abstract void BeginUpdate();

        /// <summary>
        /// 在更改内容之后,需要调用此方法来完成内容更新;在使用该方法之前需要调用 BeginUpdate();
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException">未调用 BeginUpdate()</exception>
        public abstract void CommitUpdate();

        /// <summary>
        /// 添加或替换资源;在使用该方法之前需要调用 BeginUpdate();
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="IOException">文件被占用</exception>
        public IContentEntry AddOrUpdate(string name, Stream source)
        {
            return AddOrUpdate(name, source, DateTime.Now, true);
        }

        /// <summary>
        /// 添加或替换资源;在使用该方法之前需要调用 BeginUpdate();
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="IOException">文件被占用</exception>
        public IContentEntry AddOrUpdate(string name, Stream source, bool isCloseStream)
        {
            return AddOrUpdate(name, source, DateTime.Now, isCloseStream);
        }

        /// <summary>
        /// 添加或替换资源;在使用该方法之前需要调用 BeginUpdate();
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="IOException">文件被占用</exception>
        public abstract IContentEntry AddOrUpdate(string name, Stream source, DateTime lastWriteTime, bool isCloseStream = true);

        /// <summary>
        /// 移除指定资源;在使用该方法之前需要调用 BeginUpdate();
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="IOException">文件被占用</exception>
        public virtual bool Remove(IContentEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            return Remove(entry.Name);
        }

        /// <summary>
        /// 移除指定资源;在使用该方法之前需要调用 BeginUpdate();
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="IOException">文件被占用</exception>
        public abstract bool Remove(string name);

        /// <summary>
        /// 获取到输出流,不管是否已经存在,都返回一个空的用于写的流,该方法可能使用 MemoryStream 作为缓存,在 CommitUpdate() 之后才进行变换;在使用该方法之前需要调用 BeginUpdate();(推荐在using语法内使用)
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="IOException">文件被占用</exception>
        public virtual Stream GetOutputStream(IContentEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            return GetOutputStream(entry.Name);
        }

        /// <summary>
        /// 获取到输出流,不管是否已经存在,都返回一个空的用于写的流,该方法可能使用 MemoryStream 作为缓存,在 CommitUpdate() 之后才进行变换;在使用该方法之前需要调用 BeginUpdate();(推荐在using语法内使用)
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="IOException">文件被占用</exception>
        public Stream GetOutputStream(string name)
        {
            IContentEntry entry;
            return GetOutputStream(name, out entry);
        }

        /// <summary>
        /// 获取到输出流,不管是否已经存在,都返回一个空的用于写的流,该方法可能使用 MemoryStream 作为缓存,在 CommitUpdate() 之后才进行变换;在使用该方法之前需要调用 BeginUpdate();(推荐在using语法内使用)
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="IOException">文件被占用</exception>
        public abstract Stream GetOutputStream(string name, out IContentEntry entry);

        /// <summary>
        /// 提供调用 CommitUpdate() 方法的处置接口;
        /// </summary>
        protected struct ContentCommitUpdateDisposer : IDisposable
        {
            public Content Content { get; private set; }

            public ContentCommitUpdateDisposer(Content content)
            {
                Content = content;
            }

            public void Dispose()
            {
                if (Content != null)
                {
                    Content.CommitUpdate();
                    Content = null;
                }
            }
        }

        #endregion

        /// <summary>
        /// 若该实例已经被销毁,则返回异常;
        /// </summary>
        protected void ThrowIfObjectDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(ToString());
            }
        }

        /// <summary>
        /// 若该实例 IsUpdating 为 false 则返回异常;
        /// </summary>
        protected void ThrowIfObjectNotUpdating()
        {
            if (!IsUpdating)
            {
                throw new InvalidOperationException(string.Format("在使用该方法之前需要调用{0}();", nameof(BeginUpdate)));
            }
        }

        /// <summary>
        /// 统一目录分隔符 为 "/";
        /// </summary>
        public static string Normalize(string relativePath)
        {
            relativePath = relativePath.Replace('\\', '/');
            return relativePath;
        }
    }
}
