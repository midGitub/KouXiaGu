using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 可读资源配置;(线程安全)
    /// </summary>
    [Obsolete]
    public static class LoadableResource
    {
        /// <summary>
        /// 核心资源;
        /// </summary>
        public static Modification Core { get; private set; }

        /// <summary>
        /// 所有资源;
        /// </summary>
        private static readonly List<Modification> all = new List<Modification>();

        /// <summary>
        /// 所有资源;
        /// </summary>
        public static IReadOnlyCollection<Modification> All => all;

        /// <summary>
        /// 获取到所有可读取的资源;
        /// </summary>
        internal static Task SearcheAll()
        {
            throw new NotImplementedException();
            //await Task.Run(delegate ()
            //{
            //    ModificationSearcher contentSearcher = new ModificationSearcher();
            //    Core = GetCore(contentSearcher.Factory);
            //    all.Add(Core);

            //    var mods = contentSearcher.Find(Resource.ModDirectory);
            //    all.AddRange(mods);

            //    var userMods = contentSearcher.Find(Resource.UserModDirectory);
            //    all.AddRange(userMods);
            //});

            //await Core.LoadAllAssetBundlesAsync();
        }

        /// <summary>
        /// 获取到核心资源;
        /// </summary>
        private static Modification GetCore(ModificationFactory factory)
        {
            string directory = Path.Combine(Resource.StreamingAssetsPath, "Data");
            var core = factory.Read(directory);
            return core;
        }

        /// <summary>
        /// 资源读取顺序,由 阶段控制器控制;
        /// </summary>
        public static ModificationLoadOrder Order { get; private set; }

        /// <summary>
        /// 获取到资源读取顺序;
        /// </summary>
        internal static ModificationLoadOrder GetOrLoadOrderConfig()
        {
            throw new NotImplementedException();
        }

        internal static void SetLoadOrder(ModificationLoadOrder order)
        {

        }



        //public static Task Load()
        //{

        //}

        //public static Task Load(LoadOrder order)
        //{
        //    return ResourceInitializer.Instance.Load(order);
        //}


        //private static readonly List<LoadableContent> contents = new List<LoadableContent>();
        //private static readonly ReaderWriterLockSlim readerWriterLock = new ReaderWriterLockSlim();

        //public static void Add(LoadableContent content)
        //{
        //    using (readerWriterLock.UpgradeableReadLock())
        //    {
        //        if (!contents.Contains(content))
        //        {
        //            using (readerWriterLock.WriteLock())
        //            {
        //                contents.Add(content);
        //            }
        //        }
        //    }
        //}

        //public static bool Remove(LoadableContent content)
        //{
        //    using (readerWriterLock.UpgradeableReadLock())
        //    {
        //        int index = contents.FindIndex(item => item == content);
        //        if (index >= 0)
        //        {
        //            using (readerWriterLock.WriteLock())
        //            {
        //                contents.RemoveAt(index);
        //                return true;
        //            }
        //        }
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// 获取到对应资源合集;
        ///// </summary>
        //public static LoadableContent Find(string contentID)
        //{
        //    using (readerWriterLock.ReadLock())
        //    {
        //        int index = contents.FindIndex(item => item.OriginalDescription.ID == contentID);
        //        if (index >= 0)
        //        {
        //            return contents[index];
        //        }
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// 获取到读取流;
        ///// </summary>
        //public static Stream GetInputStream(LoadableContent main, AssetPath path)
        //{
        //    string contentID;
        //    string relativePath;

        //    if (path.GetRelativePath(out contentID, out relativePath))
        //    {
        //        return main.GetInputStream(relativePath);
        //    }
        //    else
        //    {
        //        var sharedContent = Find(contentID);
        //        return sharedContent.GetInputStream(relativePath);
        //    }
        //}

        ///// <summary>
        ///// 获取到对应的 AssetBundle,若还未读取则返回异常;
        ///// </summary>
        //public static AssetBundle GetAssetBundle(LoadableContent main, AssetPath path)
        //{
        //    string contentID;
        //    string assetBundleName;

        //    if (path.GetRelativePath(out contentID, out assetBundleName))
        //    {
        //        return main.GetAssetBundle(assetBundleName);
        //    }
        //    else
        //    {
        //        var sharedContent = Find(contentID);
        //        return sharedContent.GetAssetBundle(assetBundleName);
        //    }
        //}

        ///// <summary>
        ///// 读取到指定的 AssetBundle,若未找到则返回异常;
        ///// </summary>
        //public static AssetBundle GetOrLoadAssetBundle(LoadableContent main, AssetPath path)
        //{
        //    string contentID;
        //    string assetBundleName;

        //    if (path.GetRelativePath(out contentID, out assetBundleName))
        //    {
        //        return main.GetOrLoadAssetBundle(assetBundleName);
        //    }
        //    else
        //    {
        //        var sharedContent = Find(contentID);
        //        return sharedContent.GetOrLoadAssetBundle(assetBundleName);
        //    }
        //}
    }
}
