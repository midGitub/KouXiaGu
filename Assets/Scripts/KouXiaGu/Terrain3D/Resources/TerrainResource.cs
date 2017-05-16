using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.World;
using UnityEngine;
using System.IO;

namespace KouXiaGu.Terrain3D
{

    [Serializable]
    public class TerrainResource : AsyncOperation
    {
        const string assetBundleName = "terrain";

        public Stopwatch stopwatch;
        AssetBundle assetBundle;
        public Dictionary<int, LandformResource> LandformInfos { get; private set; }
        public Dictionary<int, RoadResource> RoadInfos { get; private set; }
        public Dictionary<int, BuildingResource> BuildingInfos { get; private set; }

        public static string AssetBundleFilePath
        {
            get { return ResourcePath.CombineAssetBundle(assetBundleName); }
        }

        public IAsyncOperation Init(WorldElementResource elementInfos)
        {
            GameInitializer._StartCoroutine(ReadAsync(elementInfos));
            return this;
        }

        public IEnumerator ReadAsync(WorldElementResource elementInfos)
        {
            yield return ReadAssetBundle();

            LandformResourceReader landformReader = new LandformResourceReader(stopwatch, assetBundle, elementInfos.LandformInfos.Values);
            yield return landformReader.ReadAsync();
            LandformInfos = landformReader.Result;

            RoadResourceReader roadReader = new RoadResourceReader(stopwatch, assetBundle, elementInfos.RoadInfos.Values);
            yield return roadReader.ReadAsync();
            RoadInfos = roadReader.Result;

            BuildingResourceReader buildingReader = new BuildingResourceReader(stopwatch, assetBundle, elementInfos.BuildingInfos.Values);
            yield return buildingReader.ReadAsync();
            BuildingInfos = buildingReader.Result;

            if (assetBundle != null)
            {
                assetBundle.Unload(false);
                assetBundle = null;
            }
            OnCompleted();
        }

        IEnumerator ReadAssetBundle()
        {
            AssetBundleCreateRequest bundleLoadRequest = AssetBundle.LoadFromFileAsync(AssetBundleFilePath);
            yield return bundleLoadRequest;
            assetBundle = bundleLoadRequest.assetBundle;
            if (assetBundle == null)
            {
                Exception ex = new FileNotFoundException("未找到地形资源包;");
                Debug.LogError(ex);
                OnFaulted(ex);
            }
        }
    }

}
