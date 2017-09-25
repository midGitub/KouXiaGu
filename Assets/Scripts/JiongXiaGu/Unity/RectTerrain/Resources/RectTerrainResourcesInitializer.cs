using JiongXiaGu.Concurrent;
using JiongXiaGu.Resources;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Diagnostics;

namespace JiongXiaGu.Unity.RectTerrain.Resources
{

    /// <summary>
    /// 地形资源初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class RectTerrainResourcesInitializer : MonoBehaviour, IGameInitializeHandle
    {
        RectTerrainResourcesInitializer()
        {
        }

        internal static string TerrainAssetBundleName = "terrain";

        /// <summary>
        /// Unity线程处置器;
        /// </summary>
        [SerializeField]
        RequestUnityDispatcher Dispatcher;

        /// <summary>
        /// 地形资源,若未完成初始化则为Null;
        /// </summary>
        public static RectTerrainResources RectTerrainResources { get; private set; }

        Task IGameInitializeHandle.StartInitialize(CancellationToken token)
        {
            return Task.Run(delegate ()
            {
                AssetBundle assetBundle = LoadAssetBundle();

                List<Request> faults;
                var rectTerrainResources = ReadRectTerrainResources(assetBundle, out faults);

                UnloadAssetBundle(assetBundle);

                RectTerrainResources = rectTerrainResources;
                OnCompleted(faults);
            }, token);
        }

        /// <summary>
        /// 读取地形资源包;
        /// </summary>
        AssetBundle LoadAssetBundle()
        {
            var request = Dispatcher.Add(delegate()
            {
                string path = Path.Combine(Resource.AssetBundleDirectoryPath, TerrainAssetBundleName);
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
            if (RectTerrainResources != null)
            {
                return "[Landform:Count:" + RectTerrainResources.Landform.Count
                + "]";
            }
            return string.Empty;
        }
    }
}
