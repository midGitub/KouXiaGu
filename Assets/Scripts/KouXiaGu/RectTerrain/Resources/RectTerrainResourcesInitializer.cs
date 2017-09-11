using KouXiaGu.Concurrent;
using KouXiaGu.Resources;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Diagnostics;

namespace KouXiaGu.RectTerrain.Resources
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

        internal static SingleConfigFileName TerrainAssetBundleName = "terrain";
        internal static MultipleConfigFileName BuildingResourceName = "Terrain/Building";
        internal static MultipleConfigFileName RoadResourceName = "Terrain/Road";

        /// <summary>
        /// Unity线程处置器;
        /// </summary>
        [SerializeField]
        RequestUnityDispatcher Dispatcher;
        RectTerrainResources rectTerrainResources;

        Task IInitializer.StartInitialize(CancellationToken token)
        {
            return Task.Run(delegate ()
            {
                AssetBundle assetBundle = LoadAssetBundle(TerrainAssetBundleName.GetAssetBundleFullPath());

                List<Request> faults;
                rectTerrainResources = ReadRectTerrainResources(assetBundle, out faults);

                UnloadAssetBundle(assetBundle);

                OnCompleted(faults);
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

        RectTerrainResources ReadRectTerrainResources(AssetBundle assetBundle, out List<Request> faults)
        {
            var res = RectTerrainResourcesSerializer.DefaultInstance.Deserialize();
            var requests = new List<Request>();
            faults = new List<Request>();

            LoadLandformResInUnityThread(assetBundle, res.Landform.Values, requests);

            RequestBase.WaitAll(requests, faults);
            return res;
        }

        void LoadLandformResInUnityThread(AssetBundle assetBundle, IEnumerable<LandformResource> landformRes, ICollection<Request> requests)
        {
            foreach (var landform in landformRes)
            {
                LandformResourceLoadRequest request = new LandformResourceLoadRequest(landform, assetBundle);
                requests.Add(request);
                Dispatcher.Add(request);
            }
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

        [Conditional("EDITOR_LOG")]
        void OnCompleted(ICollection<Request> faults)
        {
            const string prefix = "[地形资源]";
            if (faults.Count == 0)
            {
                UnityEngine.Debug.Log(prefix + "初始化完成;" + GetRectTerrainResourcesInfo());
            }
            else
            {
                string fault = faults.ToLog(delegate (Request request)
                {
                    return request.Exception.ToString();
                });
                UnityEngine.Debug.Log(prefix + "初始化完成;" + GetRectTerrainResourcesInfo() + "异常:" + fault);
            }
        }

        string GetRectTerrainResourcesInfo()
        {
            if (rectTerrainResources != null)
            {
                return "[Landform:Count:" + rectTerrainResources.Landform.Count
                + "]";
            }
            return string.Empty;
        }
    }
}
