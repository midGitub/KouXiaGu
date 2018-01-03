using System;
using System.Collections.Generic;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 包装资源合集,用于继承的结构;
    /// </summary>
    public class PackagedContent : Content
    {
        protected Content MainContent { get; private set; }
        public override bool IsUpdating => MainContent.IsUpdating;
        public override bool CanRead => MainContent.CanRead;
        public override bool CanWrite => MainContent.CanWrite;
        public override bool IsDisposed => MainContent.IsDisposed;

        public PackagedContent(Content content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            MainContent = content;
        }

        public override IEnumerable<string> EnumerateFiles()
        {
            return MainContent.EnumerateFiles();
        }

        public override IEnumerable<string> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return MainContent.EnumerateFiles(searchPattern, searchOption);
        }

        public override IEnumerable<string> EnumerateFiles(string directoryName, string searchPattern, SearchOption searchOption)
        {
            return MainContent.EnumerateFiles(directoryName, searchPattern, searchOption);
        }

        public override bool Contains(string relativePath)
        {
            return MainContent.Contains(relativePath);
        }

        public override string Find(Func<string, bool> func)
        {
            return MainContent.Find(func);
        }

        public override Stream GetInputStream(string relativePath)
        {
            return MainContent.GetInputStream(relativePath);
        }

        public override IDisposable BeginUpdate()
        {
            return MainContent.BeginUpdate();
        }

        public override void CommitUpdate()
        {
            MainContent.CommitUpdate();
        }

        public override bool Remove(string relativePath)
        {
            return MainContent.Remove(relativePath);
        }

        public override Stream GetOutputStream(string relativePath)
        {
            return MainContent.GetOutputStream(relativePath);
        }

        public override void Dispose()
        {
            MainContent.Dispose();
        }

        public override bool Equals(object obj)
        {
            return MainContent.Equals(obj);
        }

        public override int GetHashCode()
        {
            return MainContent.GetHashCode();
        }

        public override string ToString()
        {
            return MainContent.ToString();
        }
    }
}
