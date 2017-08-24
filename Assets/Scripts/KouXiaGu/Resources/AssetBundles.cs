using KouXiaGu.Concurrent;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KouXiaGu.Resources
{

    /// <summary>
    /// AssteBundle 合集,线程安全的;
    /// </summary> 
    public class AssetBundles
    {
        public AssetBundles()
        {
            dispatcher = GameResourceUnityDispatcher.Instance;
            assetBundles = new Dictionary<string, AssetBundleLoader>();
        }

        GameResourceUnityDispatcher dispatcher;

        /// <summary>
        /// AssetBundle 合集,只存在状态为 Unloading, Completed;
        /// </summary>
        readonly Dictionary<string, AssetBundleLoader> assetBundles;
        readonly object asyncLock = new object();

        /// <summary>
        /// 允许非Unity线程调用,读取到 AssetBundle 资源(并非异步读取);
        /// </summary>
        public IAsyncOperation<AssetBundle> Load(string name)
        {
            lock (asyncLock)
            {
                AssetBundleLoader assetBundle;
                if (assetBundles.TryGetValue(name, out assetBundle))
                {
                    if (assetBundle.IsUnloading)
                    {
                        assetBundle.CancelUnload();
                    }
                }
                else
                {
                    assetBundle = new AssetBundleLoader(this, name);
                    assetBundles.Add(name, assetBundle);
                }
                return assetBundle;
            }
        }

        /// <summary>
        /// 卸载指定资源;
        /// </summary>
        public void Unload(string name, bool isUnloadAllLoadedObjects = false)
        {
            lock (asyncLock)
            {
                AssetBundleLoader assetBundle;
                if (assetBundles.TryGetValue(name, out assetBundle))
                {
                    if (assetBundle.IsCompleted)
                    {
                        if (!assetBundle.IsUnloading)
                        {
                            assetBundle.Unload(isUnloadAllLoadedObjects);
                        }
                    }
                    else
                    {
                        assetBundle.CancelLoad();
                        assetBundles.Remove(name);
                    }
                }
            }
        }

        /// <summary>
        /// 卸载所有;
        /// </summary>
        public void UnloadAll(bool isUnloadAllLoadedObjects)
        {
            lock (asyncLock)
            {
                foreach (var assetBundle in assetBundles.Values)
                {
                    assetBundle.Unload(isUnloadAllLoadedObjects);
                }
            }
        }

        void RemoveAssetBundle_internal(string name)
        {
            lock (asyncLock)
            {
                assetBundles.Remove(name);
            }
        }

        class AssetBundleLoader : AsyncOperation<AssetBundle>, IAsyncRequest
        {
            public AssetBundleLoader(AssetBundle assetBundle)
            {
                OnCompleted(assetBundle);
            }

            public AssetBundleLoader(AssetBundles parent, string name)
            {
                this.parent = parent;
                this.name = name;
                AddRequestQueue();
            }

            readonly AssetBundles parent;
            readonly string name;
            public bool InQueue { get; private set; }
            public bool IsUnloading { get; private set; }
            public bool IsUnloadAllLoadedObjects { get; private set; }

            void AddRequestQueue()
            {
                if (!InQueue)
                {
                    parent.dispatcher.Add(this);
                }
            }

            /// <summary>
            /// 取消加载;
            /// </summary>
            public void CancelLoad()
            {
                OnCanceled();
            }

            /// <summary>
            /// 取消卸载资源操作;
            /// </summary>
            public void CancelUnload()
            {
                IsUnloading = false;
                IsUnloadAllLoadedObjects = false;
            }

            /// <summary>
            /// 卸载此资源;
            /// </summary>
            public void Unload(bool isUnloadAllLoadedObjects)
            {
                if (Result == null)
                    throw new ArgumentException("卸载不存在的资源;");

                IsUnloading = true;
                IsUnloadAllLoadedObjects = isUnloadAllLoadedObjects;
                AddRequestQueue();
            }

            void IAsyncRequest.OnAddQueue()
            {
                if (InQueue)
                {
                    throw new ArgumentException("该请求已经加入了队列;");
                }
                InQueue = true;
            }

            bool IAsyncRequest.Prepare()
            {
                return true;
            }

            bool IAsyncRequest.Operate()
            {
                try
                {
                    if (IsCompleted)
                    {
                        return false;
                    }
                    if (IsUnloading)
                    {
                        Result.Unload(IsUnloadAllLoadedObjects);
                        parent.RemoveAssetBundle_internal(name);
                        OnFaulted(new ArgumentException("AssetBundle 已被销毁;"));
                    }
                    else
                    {
                        string filePath = Path.Combine(Resource.AssetBundleDirectoryPath, name);
                        var result = AssetBundle.LoadFromFile(filePath);
                        if (result == null)
                        {
                            OnFaulted(new ArgumentException("读取资源时发生异常;"));
                        }
                        else
                        {
                            OnCompleted(result);
                        }
                    }
                }
                catch(Exception ex)
                {
                    OnFaulted(ex);
                }
                return false;
            }

            void IAsyncRequest.OnQuitQueue()
            {
                InQueue = false;
            }
        }
    }
}
