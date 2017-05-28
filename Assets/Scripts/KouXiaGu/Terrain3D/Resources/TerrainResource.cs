using System;
using System.Collections;
using System.Collections.Generic;
using KouXiaGu.World;
using UnityEngine;
using System.IO;
using KouXiaGu.Resources;

namespace KouXiaGu.Terrain3D
{

    public class TerrainResourceReader
    {
        const string assetBundleName = "terrain";
        TerrainResource resource;
        AssetBundle assetBundle;
        IDisposable resourceReaderDisposer;
        bool isCompleted;

        Stopwatch stopwatch
        {
            get { return Resource.GlobalStopwatch; }
        }

        internal static string AssetBundleFilePath
        {
            get { return Path.Combine(Resource.AssetBundleDirectoryPath, assetBundleName); }
        }

        public TerrainResource Read(BasicTerrainResource basicResource)
        {
            UnityCoroutine coroutine = new UnityCoroutine("初始地形资源", ReadAsync(basicResource));
            resourceReaderDisposer = coroutine.SubscribeUpdate();
            while (!isCompleted)
            {
            }
            return resource;
        }

        IEnumerator ReadAsync(BasicTerrainResource basicResource)
        {
            yield return ReadAssetBundle();
            resource = new TerrainResource();

            LandformResourceReader landformReader = new LandformResourceReader(stopwatch, assetBundle, basicResource.Landform.Values);
            yield return landformReader.ReadAsync();
            resource.Landform = landformReader.Result;

            RoadResourceReader roadReader = new RoadResourceReader(stopwatch, assetBundle, basicResource.Road.Values);
            yield return roadReader.ReadAsync();
            resource.Road = roadReader.Result;

            BuildingResourceReader buildingReader = new BuildingResourceReader(stopwatch, assetBundle, basicResource.Building.Values);
            yield return buildingReader.ReadAsync();
            resource.Building = buildingReader.Result;

            OnCompleted();
        }

        void OnCompleted()
        {
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
            isCompleted = true;
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
                throw ex;
            }
        }
    }

    [Serializable]
    public class TerrainResource : AsyncOperation
    {
        //const string assetBundleName = "terrain";

        //public Stopwatch stopwatch;
        //AssetBundle assetBundle;
        public Dictionary<int, LandformResource> Landform { get; internal set; }
        public Dictionary<int, RoadResource> Road { get; internal set; }
        public Dictionary<int, BuildingResource> Building { get; internal set; }
        //IDisposable resourceReaderDisposer;

        //internal static string AssetBundleFilePath
        //{
        //    get { return Path.Combine(Resource.AssetBundleDirectoryPath, assetBundleName); }
        //}

        //public IAsyncOperation Init(BasicTerrainResource elementInfos)
        //{
        //    if (IsCompleted)
        //    {
        //        throw new ArgumentException("已经初始化完毕;");
        //    }

        //    UnityCoroutine coroutine = new UnityCoroutine("初始地形资源", ReadAsync(elementInfos));
        //    resourceReaderDisposer = coroutine.SubscribeUpdate();
        //    return this;
        //}

        //protected override void OnCompleted()
        //{
        //    base.OnCompleted();

        //    if (assetBundle != null)
        //    {
        //        assetBundle.Unload(false);
        //        assetBundle = null;
        //    }
        //    if (resourceReaderDisposer != null)
        //    {
        //        resourceReaderDisposer.Dispose();
        //        resourceReaderDisposer = null;
        //    }
        //}

        //IEnumerator ReadAsync(BasicTerrainResource elementInfos)
        //{
        //    yield return ReadAssetBundle();

        //    LandformResourceReader landformReader = new LandformResourceReader(stopwatch, assetBundle, elementInfos.Landform.Values);
        //    yield return landformReader.ReadAsync();
        //    LandformInfos = landformReader.Result;

        //    RoadResourceReader roadReader = new RoadResourceReader(stopwatch, assetBundle, elementInfos.Road.Values);
        //    yield return roadReader.ReadAsync();
        //    RoadInfos = roadReader.Result;

        //    BuildingResourceReader buildingReader = new BuildingResourceReader(stopwatch, assetBundle, elementInfos.Building.Values);
        //    yield return buildingReader.ReadAsync();
        //    BuildingInfos = buildingReader.Result;

        //    OnCompleted();
        //}

        //IEnumerator ReadAssetBundle()
        //{
        //    AssetBundleCreateRequest bundleLoadRequest = AssetBundle.LoadFromFileAsync(AssetBundleFilePath);
        //    yield return bundleLoadRequest;
        //    assetBundle = bundleLoadRequest.assetBundle;
        //    if (assetBundle == null)
        //    {
        //        Exception ex = new FileNotFoundException("未找到地形资源包;");
        //        Debug.LogError(ex);
        //        OnFaulted(ex);
        //    }
        //}
    }
}
