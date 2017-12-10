using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    internal class Texture2DAsset : UnityAsset<Texture2D>
    {
        public Texture2DAsset(Texture2D value) : base(value)
        {
        }
    }

    internal class Texture2DAssetReader : AssetReader<Texture2D>
    {
        public static Texture2DAssetReader Default { get; private set; } = new Texture2DAssetReader();

        public override AssetTypes AssetType
        {
            get { return AssetTypes.Texture2D; }
        }

        public override WeakReferenceObject<Texture2D> AsWeakReferenceObject(Texture2D value)
        {
            return new Texture2DAsset(value);
        }

        public override Texture2D Load(LoadableContent content, AssetInfo assetInfo)
        {
            XiaGu.ThrowIfNotUnityThread();
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            switch (assetInfo.From)
            {
                case LoadMode.AssetBundle:
                    return InternalFromAssetBundleReadTexture2D(content, assetInfo);

                case LoadMode.File:
                    return InternalFromFileReadTexture2D(content, assetInfo);

                default:
                    throw InvalidLoadModeException(nameof(Texture2D), assetInfo.From);
            }
        }

        /// <summary>
        /// 从 AssetBundle 读取 Texture2D
        /// </summary>
        private static Texture2D InternalFromAssetBundleReadTexture2D(LoadableContent content, AssetInfo assetInfo)
        {
            AssetBundle assetBundle = content.GetOrLoadAssetBundle(assetInfo.AssetBundleName);
            var texture = assetBundle.LoadAsset<Texture2D>(assetInfo.Name);
            if (texture == null)
            {
                throw new ArgumentException(string.Format("在AssetBundle[{0}]内未找到资源[{1}]", assetInfo.AssetBundleName, assetInfo.Name));
            }
            else
            {
                return texture;
            }
        }

        /// <summary>
        /// 从 文件 读取 Texture2D;
        /// </summary>
        private static Texture2D InternalFromFileReadTexture2D(LoadableContent content, AssetInfo assetInfo)
        {
            lock (content.AsyncLock)
            {
                var stream = content.GetInputStream(assetInfo.Name);
                byte[] imageData = new byte[stream.Length];
                stream.Read(imageData, 0, (int)stream.Length);

                var texture = new Texture2D(2, 2);
                texture.LoadImage(imageData);
                return texture;
            }
        }
    }

    public static class Texture2DAssetExtensions
    {
        public static Task<Texture2D> ReadAsTexture2D(this AssetPool assetPool, LoadableContent content, AssetInfo assetInfo, CancellationToken token = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public static Task<Texture2D> ReadAsTexture2D(this AssetPool assetPool, LoadableContent content, AssetInfo assetInfo, AssetLoadOptions options, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}


//    public static class Texture2DAssetExtensions
//    {

//        private static AssetReader<Texture2D> assetReader;

//        /// <summary>
//        /// 读取资源,创建为 Texture2D 格式;
//        /// </summary>
//        public static Task<Texture2D> ReadAsTexture2D1(this AssetPool assetPool, LoadableContent content, AssetInfo assetInfo, AssetLoadOptions options = AssetLoadOptions.None, CancellationToken token = default(CancellationToken))
//        {
//            try
//            {
//                string key = assetPool.GetKey(content, assetInfo);
//                AssetTask asset;
//                Texture2DAssetTask texture2DAsset;
//                Task<Texture2D> task;

//                using (assetPool.AssetDictionaryLock.UpgradeableReadLock())
//                {
//                    if (assetPool.AssetDictionary.TryGetValue(key, out asset))
//                    {
//                        switch (asset.Task.Status)
//                        {
//                            case TaskStatus.RanToCompletion:

//                                task = AsTexture2DTask(asset);

//                                if ((options | AssetLoadOptions.Reread) > AssetLoadOptions.None)
//                                {

//                                }
//                                else
//                                {
//                                    return task;
//                                }

//                                return task;

//                            case TaskStatus.Faulted:

//                                if (asset.Type == AssetTypes.Texture2D)
//                                {
//                                    return (Task<Texture2D>)asset.Task;
//                                }
//                                else
//                                {
//                                    texture2DAsset = new Texture2DAssetTask();
//                                    task = ReadAsTexture2D(assetPool, content, assetInfo, texture2DAsset);

//                                    using (assetPool.AssetDictionaryLock.WriteLock())
//                                    {
//                                        assetPool.AssetDictionary[key] = asset;
//                                    }

//                                    return AssetPool.CreateFollowTask(task, token);
//                                }

//                            case TaskStatus.WaitingToRun:
//                            case TaskStatus.Running:

//                                task = AsTexture2DTask(asset);
//                                return AssetPool.CreateFollowTask(task, token);

//                            default:
//                                throw new NotSupportedException(asset.Task.Status.ToString());
//                        }
//                    }
//                    else
//                    {
//                        texture2DAsset = new Texture2DAssetTask();
//                        task = ReadAsTexture2D(assetPool, content, assetInfo, texture2DAsset);

//                        using (assetPool.AssetDictionaryLock.WriteLock())
//                        {
//                            assetPool.AssetDictionary.Add(key, asset);
//                        }

//                        return AssetPool.CreateFollowTask(task, token);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                return Task.FromException<Texture2D>(ex);
//            }
//        }


//        /// <summary>
//        /// 读取资源,创建为 Texture2D 格式;
//        /// </summary>
//        public static Task<Texture2D> ReadAsTexture2D(this AssetPool assetPool, LoadableContent content, AssetInfo assetInfo, AssetLoadOptions options = AssetLoadOptions.None, CancellationToken token = default(CancellationToken))
//        {
//            try
//            {
//                string key = assetPool.GetKey(content, assetInfo);
//                AssetTask asset;
//                Texture2DAssetTask texture2DAsset;
//                Task<Texture2D> task;

//                using (assetPool.AssetDictionaryLock.UpgradeableReadLock())
//                {
//                    if (assetPool.AssetDictionary.TryGetValue(key, out asset))
//                    {
//                        switch (asset.Task.Status)
//                        {
//                            case TaskStatus.RanToCompletion:

//                                task = AsTexture2DTask(asset);

//                                if ((options | AssetLoadOptions.Reread) > AssetLoadOptions.None)
//                                {

//                                }
//                                else
//                                {
//                                    return task;
//                                }

//                                return task;

//                            case TaskStatus.Faulted:

//                                if (asset.Type == AssetTypes.Texture2D)
//                                {
//                                    return (Task<Texture2D>)asset.Task;
//                                }
//                                else
//                                {
//                                    texture2DAsset = new Texture2DAssetTask();
//                                    task = ReadAsTexture2D(assetPool, content, assetInfo, texture2DAsset);

//                                    using (assetPool.AssetDictionaryLock.WriteLock())
//                                    {
//                                        assetPool.AssetDictionary[key] = asset;
//                                    }

//                                    return AssetPool.CreateFollowTask(task, token);
//                                }

//                            case TaskStatus.WaitingToRun:
//                            case TaskStatus.Running:

//                                task = AsTexture2DTask(asset);
//                                return AssetPool.CreateFollowTask(task, token);

//                            default:
//                                throw new NotSupportedException(asset.Task.Status.ToString());
//                        }
//                    }
//                    else
//                    {
//                        texture2DAsset = new Texture2DAssetTask();
//                        task = ReadAsTexture2D(assetPool, content, assetInfo, texture2DAsset);

//                        using (assetPool.AssetDictionaryLock.WriteLock())
//                        {
//                            assetPool.AssetDictionary.Add(key, asset);
//                        }

//                        return AssetPool.CreateFollowTask(task, token);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                return Task.FromException<Texture2D>(ex);
//            }
//        }

//        /// <summary>
//        /// 转换成 Task<Texture2D> 若不能转换则返回异常;
//        /// </summary>
//        private static Task<Texture2D> AsTexture2DTask(AssetTask asset)
//        {
//            if (asset.Type == AssetTypes.Texture2D)
//            {
//                var task = (Task<Texture2D>)asset.Task;
//                return task;
//            }
//            else
//            {
//                throw new ArgumentException(string.Format("资产为 {0} 类型,并非 {1}", asset.Type, nameof(Texture2D)));
//            }
//        }

//        /// <summary>
//        /// 返回一个读取贴图的 Task,并且初始化 Asset;
//        /// </summary>
//        private static Task<Texture2D> ReadAsTexture2D(AssetPool assetPool, LoadableContent content, AssetInfo assetInfo, Texture2DAssetTask asset)
//        {
//            throw new NotImplementedException();

//            //var task = TaskHelper.Run(delegate ()
//            //{
//            //    return AssetReader0.ReadAsTexture2D(content, assetInfo);
//            //}, AssetPool.TaskScheduler);

//            //asset.Texture2DTask = task;

//            //return task;
//        }
//    }
//}
