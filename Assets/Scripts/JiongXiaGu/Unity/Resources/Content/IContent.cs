using System;
using System.Collections.Generic;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{

    public static class ContentExtensions
    {
        public static IDisposable BeginUpdateAuto(this IContent content)
        {
            content.BeginUpdate();
            return new ContentCommitUpdateDisposer(content);
        }

        /// <summary>
        /// 提供调用 CommitUpdate() 方法的处置接口;
        /// </summary>
        private struct ContentCommitUpdateDisposer : IDisposable
        {
            public IContent Content { get; private set; }

            public ContentCommitUpdateDisposer(IContent content)
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

    /// <summary>
    /// 资源合集;
    /// </summary>
    public interface IContent
    {
        IEnumerable<IContentEntry> EnumerateEntries();
        IEnumerable<IContentEntry> EnumerateEntries(string searchPattern, SearchOption searchOption);
        IEnumerable<IContentEntry> EnumerateEntries(string directoryName, string searchPattern, SearchOption searchOption);
        IEnumerable<string> EnumerateFiles();
        IEnumerable<string> EnumerateFiles(string searchPattern, SearchOption searchOption);
        IEnumerable<string> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption);
        bool Contains(string name);
        IContentEntry GetEntry(string name);
        Stream GetInputStream(IContentEntry entry);
        Stream GetInputStream(string name);

        void BeginUpdate();
        void CommitUpdate();
        IContentEntry AddOrUpdate(string name, Stream source);
        IContentEntry AddOrUpdate(string name, Stream source, bool isCloseStream);
        bool Remove(IContentEntry entry);
        bool Remove(string name);
        Stream GetOutputStream(IContentEntry entry);
        Stream GetOutputStream(string name);
    }

    /// <summary>
    /// 只读的资源合集;
    /// </summary>
    public interface IReadOnlyContent
    {
        IEnumerable<IContentEntry> EnumerateEntries();
        IEnumerable<IContentEntry> EnumerateEntries(string searchPattern, SearchOption searchOption);
        IEnumerable<IContentEntry> EnumerateEntries(string directoryName, string searchPattern, SearchOption searchOption);

        IEnumerable<string> EnumerateFiles();
        IEnumerable<string> EnumerateFiles(string searchPattern, SearchOption searchOption);
        IEnumerable<string> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption);

        bool Contains(string name);
        IContentEntry GetEntry(string name);

        Stream GetInputStream(IContentEntry entry);
        Stream GetInputStream(string name);
    }
}
