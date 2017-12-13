using JiongXiaGu.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 关联的资源,处理资源共享的 AssetBundle 和 其它资源;(线程安全);
    /// </summary>
    public class SharedContentSource
    {
        private List<SharedContent> contentCollection = new List<SharedContent>();
        private ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim();

        /// <summary>
        /// 添加新的资源,若重复加入则返回false;
        /// </summary>
        internal SharedContent Add(LoadableContent loadableContent)
        {
            using (readerWriterLock.UpgradeableReadLock())
            {
                if (contentCollection.Contains(item => item.ID == loadableContent.Description.ID))
                {
                    throw new ArgumentException("重复加入");
                }
                else
                {
                    SharedContent sharedContent = new SharedContent(this, loadableContent);
                    using (readerWriterLock.WriteLock())
                    {
                        contentCollection.Add(sharedContent);
                    }
                    return sharedContent;
                }
            }
        }

        /// <summary>
        /// 移除资源,若移除成功则返回true;
        /// </summary>
        internal bool Remove(LoadableContent loadableContent)
        {
            using (readerWriterLock.WriteLock())
            {
                return contentCollection.Remove(item => item.ID == loadableContent.Description.ID);
            }
        }

        /// <summary>
        /// 获取到自定资源,若不存在则返回异常;
        /// </summary>
        public SharedContent Find(string contentID)
        {
            if (string.IsNullOrWhiteSpace(contentID))
                throw new ArgumentNullException(nameof(contentID));

            using (readerWriterLock.ReadLock())
            {
                var sharedContent = contentCollection.Find(item => item.ID == contentID);
                if (sharedContent != null)
                {
                    return sharedContent;
                }
                else
                {
                    throw new FileNotFoundException(string.Format("未找到资源[ID : {0}]", contentID));
                }
            }
        }

        /// <summary>
        /// 获取到自定资源,若不存在则返回异常;
        /// </summary>
        public SharedContent FindOrDefault(string contentID)
        {
            if (string.IsNullOrWhiteSpace(contentID))
                throw new ArgumentNullException(nameof(contentID));

            using (readerWriterLock.ReadLock())
            {
                var sharedContent = contentCollection.Find(item => item.ID == contentID);
                return sharedContent;
            }
        }

        public bool Contains(string contentID)
        {
            using (readerWriterLock.ReadLock())
            {
                return contentCollection.Contains(item => item.ID == contentID);
            }
        }
    }
}
