using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 线程安全;
    /// </summary>
    public class SharedContent
    {
        private List<ModificationContent> contents;
        private ReaderWriterLockSlim readerWriterLock;

        public SharedContent()
        {
            contents = new List<ModificationContent>();
            readerWriterLock = new ReaderWriterLockSlim();
        }

        public SharedContent(IEnumerable<ModificationContent> contents)
        {
            contents = new List<ModificationContent>(contents);
            readerWriterLock = new ReaderWriterLockSlim();
        }

        public void Add(ModificationContent content)
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

        public bool Remove(ModificationContent content)
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

        public ModificationContent Find(string contentID)
        {
            using (readerWriterLock.ReadLock())
            {
                int index = contents.FindIndex(item => item.OriginalDescription.ID == contentID);
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
        public Stream GetInputStream(ModificationContent main, AssetPath path)
        {
            string contentID;
            string relativePath;

            if (path.GetRelativePath(out contentID, out relativePath))
            {
                return main.BaseContent.GetInputStream(relativePath);
            }
            else
            {
                var sharedContent = Find(contentID);
                return sharedContent.BaseContent.GetInputStream(relativePath);
            }
        }

        /// <summary>
        /// 获取到对应的 AssetBundle,若还未读取则返回异常;
        /// </summary>
        public AssetBundle GetAssetBundle(ModificationContent main, AssetPath path)
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
        public AssetBundle GetOrLoadAssetBundle(ModificationContent main, AssetPath path)
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
