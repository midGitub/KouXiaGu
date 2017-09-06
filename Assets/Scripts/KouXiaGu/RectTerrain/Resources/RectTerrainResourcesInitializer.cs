using KouXiaGu.Concurrent;
using KouXiaGu.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.RectTerrain
{

    /// <summary>
    /// 地形资源初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class RectTerrainResourcesInitializer : MonoBehaviour, IInitializer
    {
        RectTerrainResourcesInitializer()
        {
        }

        internal const string TerrainAssetBundleName = "terrain";
        internal const string LandformResourceName = "Terrain/Landform";
        internal const string BuildingResourceName = "Terrain/Building";
        internal const string RoadResourceName = "Terrain/Road";

        /// <summary>
        /// Unity线程处置器;
        /// </summary>
        [SerializeField]
        RequestUnityDispatcher Dispatcher;

        internal static string TerrainAssetBundlePath
        {
            get { return Path.Combine(Resource.AssetBundleDirectoryPath, TerrainAssetBundleName); }
        }

        Task IInitializer.StartInitialize(CancellationToken token)
        {
            return Task.Run(delegate ()
            {
                Debug.Log("[开始地形初始化]");
                AssetBundle assetBundle = LoadAssetBundle(TerrainAssetBundlePath);
                Debug.Log("[完成地形资源包读取]");
                UnloadAssetBundle(assetBundle);
                Debug.Log("[完成地形初始化]");
            }, token);
        }

        /// <summary>
        /// 读取地形资源包;
        /// </summary>
        AssetBundle LoadAssetBundle(string path)
        {
            var request = Dispatcher.Add(delegate()
            {
                return AssetBundle.LoadFromFile(path);
            });
            while (!request.IsCompleted)
            {
            }
            return request.Result;
        }

        /// <summary>
        /// 卸载资源包;
        /// </summary>
        void UnloadAssetBundle(AssetBundle assetBundle)
        {
            var request = Dispatcher.Add(delegate()
            {
                assetBundle.Unload(false);
            });
            while (!request.IsCompleted)
            {
            }
        }
    }
}
