using System;
using System.Collections;
using System.Collections.Generic;
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
        IDisposable resourceReaderDisposer;

        internal static string AssetBundleFilePath
        {
            get { return ResourcePath.CombineAssetBundle(assetBundleName); }
        }

        public IAsyncOperation Init(WorldResource elementInfos)
        {
            if (IsCompleted)
            {
                throw new ArgumentException("已经初始化完毕;");
            }

            UnityCoroutine coroutine = new UnityCoroutine("初始地形资源", ReadAsync(elementInfos));
            resourceReaderDisposer = coroutine.SubscribeUpdate();
            return this;
        }

        protected override void OnCompleted()
        {
            base.OnCompleted();

            if (assetBundle != null)
            {
                assetBundle.Unload(false);
                assetBundle = null;
            }
            if (resourceReaderDisposer != null)
            {
                resourceReaderDisposer.Dispose();
                resourceReaderDisposer = null;
            }
        }

        IEnumerator ReadAsync(WorldResource elementInfos)
        {
            yield return ReadAssetBundle();

            LandformResourceReader landformReader = new LandformResourceReader(stopwatch, assetBundle, elementInfos.Landform.Values);
            yield return landformReader.ReadAsync();
            LandformInfos = landformReader.Result;

            RoadResourceReader roadReader = new RoadResourceReader(stopwatch, assetBundle, elementInfos.Road.Values);
            yield return roadReader.ReadAsync();
            RoadInfos = roadReader.Result;

            BuildingResourceReader buildingReader = new BuildingResourceReader(stopwatch, assetBundle, elementInfos.Building.Values);
            yield return buildingReader.ReadAsync();
            BuildingInfos = buildingReader.Result;

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
