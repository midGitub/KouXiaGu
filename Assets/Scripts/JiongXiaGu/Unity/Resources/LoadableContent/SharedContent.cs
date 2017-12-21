using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 线程安全;
    /// </summary>
    public class SharedContent
    {
        private List<LoadableContent> contents;
        private ReaderWriterLockSlim readerWriterLock;

        public SharedContent()
        {
            contents = new List<LoadableContent>();
            readerWriterLock = new ReaderWriterLockSlim();
        }

        public void Add(LoadableContent content)
        {
            using (readerWriterLock.UpgradeableReadLock())
            {
                if (!contents.Contains(content))
                {
                    using (readerWriterLock.WriteLock())
                    {
                        contents.Add(content);
                    }
                }
            }
        }

        public bool Remove(LoadableContent content)
        {
            using (readerWriterLock.UpgradeableReadLock())
            {
                int index = contents.FindIndex(item => item == content);
                if (index >= 0)
                {
                    using (readerWriterLock.WriteLock())
                    {
                        contents.RemoveAt(index);
                        return true;
                    }
                }
                return false;
            }
        }

        public LoadableContent Find(string contentID)
        {
            using (readerWriterLock.ReadLock())
            {
                int index = contents.FindIndex(item => item.ID == contentID);
                if (index >= 0)
                {
                    return contents[index];
                }
                return null;
            }
        }

        /// <summary>
        /// 获取到读取流;
        /// </summary>
        public Stream GetInputStream(LoadableContent main, AssetPath path)
        {
            string contentID;
            string relativePath;

            if (path.GetRelativePath(out contentID, out relativePath))
            {
                return main.Content.GetInputStream(relativePath);
            }
            else
            {
                var sharedContent = Find(contentID);
                return sharedContent.Content.GetInputStream(relativePath);
            }
        }

        /// <summary>
        /// 获取到对应的 AssetBundle,若还未读取则返回异常;
        /// </summary>
        public AssetBundle GetAssetBundle(LoadableContent main, AssetPath path)
        {
            string contentID;
            string assetBundleName;

            if (path.GetRelativePath(out contentID, out assetBundleName))
            {
                return main.GetAssetBundle(assetBundleName);
            }
            else
            {
                var sharedContent = Find(contentID);
                return sharedContent.GetAssetBundle(assetBundleName);
            }
        }

        /// <summary>
        /// 读取到指定的 AssetBundle,若未找到则返回异常;
        /// </summary>
        public AssetBundle GetOrLoadAssetBundle(LoadableContent main, AssetPath path)
        {
            string contentID;
            string assetBundleName;

            if (path.GetRelativePath(out contentID, out assetBundleName))
            {
                return main.GetOrLoadAssetBundle(assetBundleName);
            }
            else
            {
                var sharedContent = Find(contentID);
                return sharedContent.GetOrLoadAssetBundle(assetBundleName);
            }
        }
    }
}
