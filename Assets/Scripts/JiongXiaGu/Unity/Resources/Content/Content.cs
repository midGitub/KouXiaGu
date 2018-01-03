using System;
using System.Collections.Generic;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 抽象资源合集;
    /// </summary>
    public abstract class Content : IDisposable
    {
        /// <summary>
        /// 是否正在更新改内容?
        /// </summary>
        public abstract bool IsUpdating { get; }
        public abstract bool CanRead { get; }
        public abstract bool CanWrite { get; }
        public abstract bool IsDisposed { get; }

        /// <summary>
        /// 释放所有资源;
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// 获取到所有文件相对路径;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        public abstract IEnumerable<string> EnumerateFiles();

        /// <summary>
        /// 获取到所有文件相对路径;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException">未知的 SearchOption</exception>
        public virtual IEnumerable<string> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();

            return EnumerateFiles(EnumerateFiles(), searchPattern, searchOption);
        }

        /// <summary>
        /// 获取到所有文件相对路径;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException">未知的 SearchOption</exception>
        public virtual IEnumerable<string> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            ThrowIfObjectDisposed();

            return EnumerateFiles(EnumerateFiles(), directoryName, searchPattern, searchOption);
        }

        /// <summary>
        /// 获取到所有文件相对路径;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException">未知的 SearchOption</exception>
        public static IEnumerable<string> EnumerateFiles(IEnumerable<string> fileNames, string searchPattern, SearchOption searchOption)
        {
            if (fileNames == null)
                throw new ArgumentNullException(nameof(fileNames));
            if (searchPattern == null)
                throw new ArgumentNullException(nameof(searchPattern));

            switch (searchOption)
            {
                case SearchOption.AllDirectories:
                    foreach (string entry in fileNames)
                    {
                        string fileName = PathHelper.GetFileName(entry);
                        if (PathHelper.IsMatch(fileName, searchPattern))
                        {
                            yield return entry;
                        }
                    }
                    break;

                case SearchOption.TopDirectoryOnly:
                    foreach (string entry in fileNames)
                    {
                        if (PathHelper.IsFileName(entry))
                        {
                            string fileName = PathHelper.GetFileName(entry);
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
        /// 获取到所有文件相对路径;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException">未知的 SearchOption</exception>
        public static IEnumerable<string> EnumerateFiles(IEnumerable<string> fileNames, string directoryName, string searchPattern, SearchOption searchOption)
        {
            if (fileNames == null)
                throw new ArgumentNullException(nameof(fileNames));
            if (directoryName == null)
                throw new ArgumentNullException(nameof(directoryName));
            if (searchPattern == null)
                throw new ArgumentNullException(nameof(searchPattern));

            string fileName;
            directoryName = PathHelper.Normalize(directoryName);

            switch (searchOption)
            {
                case SearchOption.AllDirectories:
                    foreach (var entry in fileNames)
                    {
                        if (PathHelper.IsCommonRoot(directoryName, entry))
                        {
                            fileName = PathHelper.GetFileName(entry);
                            if (PathHelper.IsMatch(fileName, searchPattern))
                            {
                                yield return entry;
                            }
                        }
                    }
                    break;

                case SearchOption.TopDirectoryOnly:
                    foreach (var entry in fileNames)
                    {
                        if (PathHelper.IsCommonRoot(directoryName, entry))
                        {
                            fileName = PathHelper.GetFileName(entry);
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
        /// 确定是否存在该路径;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        public virtual bool Contains(string relativePath)
        {
            foreach (var path in EnumerateFiles())
            {
                if (path == relativePath)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 搜索指定路径,直到返回true停止,若未找到则返回 null;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        public virtual string Find(Func<string, bool> func)
        {
            ThrowIfObjectDisposed();
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            foreach (var file in EnumerateFiles())
            {
                if (func.Invoke(file))
                {
                    return file;
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
        public abstract Stream GetInputStream(string relativePath);


        /// <summary>
        /// 在更改内容之前需要先调用,直到调用 CommitUpdate() 进行修改操作;(推荐在Using语句内使用该方法,在using语句内使用,则无需手动调用 CommitUpdate() 结束更改,否则在调用 CommitUpdate() 时可能返回异常)
        /// </summary>
        public abstract IDisposable BeginUpdate();

        /// <summary>
        /// 在更改内容之后,需要调用此方法来完成内容更新;在使用该方法之前需要调用 BeginUpdate();
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        public abstract void CommitUpdate();

        /// <summary>
        /// 获取到输出流,不管是否已经存在,都返回一个空的用于写的流;在使用该方法之前需要调用 BeginUpdate();(推荐在using语法内使用)
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="IOException">文件被占用</exception>
        public abstract Stream GetOutputStream(string relativePath);

        /// <summary>
        /// 移除指定资源;在使用该方法之前需要调用 BeginUpdate();
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="IOException">文件被占用</exception>
        public abstract bool Remove(string relativePath);

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
        /// 统一目录分隔符 为 "/";
        /// </summary>
        public static string Normalize(string relativePath)
        {
            relativePath = relativePath.Replace('\\', '/');
            return relativePath;
        }

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
    }
}
