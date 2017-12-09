using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JiongXiaGu.Collections;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资产池,贴图,模型等资产池;
    /// </summary>
    public class AssetPool
    {
        public static AssetPool Default { get; private set; } = new AssetPool();

        private Dictionary<string, Asset> dictionary = new Dictionary<string, Asset>();
        private ReaderWriterLockSlim dictionaryLockSlim = new ReaderWriterLockSlim();

        private TaskScheduler TaskScheduler
        {
            get { return UnityTaskScheduler.TaskScheduler; }
        }


        public Task<Texture2D> ReadAsTexture2D(LoadableContent content, AssetInfo assetInfo, CancellationToken token)
        {
            try
            {
                string key = GetKey(content, assetInfo);
                Asset asset;
                Task<Texture2D> task;

                using (dictionaryLockSlim.UpgradeableReadLock())
                {
                    if (dictionary.TryGetValue(key, out asset))
                    {
                        switch (asset.Task.Status)
                        {
                            case TaskStatus.RanToCompletion:

                                task = AsTexture2DTask(asset);
                                return task;

                            case TaskStatus.Faulted:

                                if (asset.Type == AssetType.Texture2D)
                                {
                                    return (Task<Texture2D>)asset.Task;
                                }
                                else
                                {
                                    asset = ReadAsTexture2D(content, assetInfo, out task);

                                    using (dictionaryLockSlim.WriteLock())
                                    {
                                        dictionary[key] = asset;
                                    }

                                    return CreateFollowTask(task, token);
                                }

                            case TaskStatus.WaitingToRun:
                            case TaskStatus.Running:

                                task = AsTexture2DTask(asset);
                                return CreateFollowTask(task, token);

                            default:
                                throw new NotSupportedException(asset.Task.Status.ToString());
                        }
                    }
                    else
                    {
                        asset = ReadAsTexture2D(content, assetInfo, out task);

                        using (dictionaryLockSlim.WriteLock())
                        {
                            dictionary.Add(key, asset);
                        }

                        return CreateFollowTask(task, token);
                    }
                }
            }
            catch (Exception ex)
            {
                return Task.FromException<Texture2D>(ex);
            }
        }

        private Task<Texture2D> AsTexture2DTask(Asset asset)
        {
            if (asset.Type == AssetType.Texture2D)
            {
                var task = (Task<Texture2D>)asset.Task;
                return task;
            }
            else
            {
                throw new ArgumentException(string.Format("资产为 {0} 类型,并非 {1}", asset.Type, nameof(Texture2D)));
            }
        }

        private Asset ReadAsTexture2D(LoadableContent content, AssetInfo assetInfo, out Task<Texture2D> task)
        {
            task = TaskHelper.Run(delegate ()
            {
                return AssetReader.ReadAsTexture2D(content, assetInfo);
            }, TaskScheduler);

            var asset = new Asset(AssetType.Texture2D, task);
            return asset;
        }

        /// <summary>
        /// 创建一个后续任务,等待 task 完成;
        /// </summary>
        private Task<T> CreateFollowTask<T>(Task<T> task, CancellationToken token)
        {
            return task.ContinueWith(delegate (Task<T> task0)
            {
                return task0.Result;
            }, token);
        }

        private string GetKey(LoadableContent content, AssetInfo assetInfo)
        {
            return GetKey(content.Description, assetInfo);
        }

        private string GetKey(LoadableContentDescription description, AssetInfo assetInfo)
        {
            const string separator = ":";
            string key;

            switch (assetInfo.From)
            {
                case LoadMode.AssetBundle:
                    key = string.Join(separator, description.ID, assetInfo.AssetBundleName, assetInfo.Name);
                    break;

                case LoadMode.File:
                    key = string.Join(separator, description.ID, assetInfo.Name);
                    break;

                default:
                    throw new NotSupportedException(assetInfo.From.ToString());
            }

            return key;
        }


        private class Asset
        {
            public AssetType Type { get; private set; }
            public Task Task { get; private set; }

            public Asset(AssetType type, Task task)
            {
                Type = type;
                Task = task;
            }
        }
    }

    public enum AssetType
    {
        Texture2D,
    }
}
