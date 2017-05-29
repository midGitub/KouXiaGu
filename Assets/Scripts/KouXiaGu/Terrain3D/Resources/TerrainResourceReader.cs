using KouXiaGu.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 读取到所有地形资源;
    /// </summary>
    public class TerrainResourcesReader : IDisposable
    {
        internal LandformInfoXmlSerializer LandformFileSerializer = new LandformInfoXmlSerializer();
        internal BuildingInfoXmlSerializer BuildingFileSerializer = new BuildingInfoXmlSerializer();
        internal RoadInfoXmlSerializer RoadFileSerializer = new RoadInfoXmlSerializer();

        public TerrainResources Read()
        {
            TerrainResources Result = new TerrainResources()
            {
                Landform = LandformFileSerializer.Read(),
                Building = BuildingFileSerializer.Read(),
                Road = RoadFileSerializer.Read(),
            };

            InitAssetBundleResources(Result);
            InitLandformTag(Result);
            return Result;
        }

        const string assetBundleName = "terrain";
        AssetBundle assetBundle;
        IDisposable resourceReaderDisposer;
        bool isAssetBundleResourcesReadCompleted;
        bool isAssetBundleResourcesReadFaulted;
        Exception faultedException;
        internal LandformResourcesReader LandformResourcesReader = new LandformResourcesReader();
        internal BuildingResourcesReader BuildingResourcesReader = new BuildingResourcesReader();
        internal RoadResourcesReader RoadResourcesReader = new RoadResourcesReader();

        string AssetBundleFilePath
        {
            get { return Path.Combine(Resource.AssetBundleDirectoryPath, assetBundleName); }
        }

        Stopwatch stopwatch
        {
            get { return Resource.GlobalStopwatch; }
        }
       
        void InitAssetBundleResources(TerrainResources result)
        {
            UnityCoroutine coroutine = new UnityCoroutine("初始地形资源", ReadAssetBundleResources(result));
            resourceReaderDisposer = coroutine.SubscribeUpdate();
            while (!isAssetBundleResourcesReadCompleted)
            {
            }
            if (isAssetBundleResourcesReadFaulted)
            {
                throw faultedException;
            }
        }

        /// <summary>
        /// 在协程内读取资源;
        /// </summary>
        IEnumerator ReadAssetBundleResources(TerrainResources result)
        {
            AssetBundleCreateRequest bundleLoadRequest = AssetBundle.LoadFromFileAsync(AssetBundleFilePath);
            yield return bundleLoadRequest;
            assetBundle = bundleLoadRequest.assetBundle;
            if (assetBundle == null)
            {
                faultedException = new FileNotFoundException("未找到地形资源包;");
                resourceReaderDisposer.Dispose();
                isAssetBundleResourcesReadCompleted = true;
                isAssetBundleResourcesReadFaulted = true;
                yield break;
            }

            yield return LandformResourcesReader.ReadAsync(stopwatch, assetBundle, result.Landform);
            yield return BuildingResourcesReader.ReadAsync(stopwatch, assetBundle, result.Building);
            yield return RoadResourcesReader.ReadAsync(stopwatch, assetBundle, result.Road);

            assetBundle.Unload(false);
            resourceReaderDisposer.Dispose();
            isAssetBundleResourcesReadCompleted = true;
        }

        public void Dispose()
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
        }

        internal LandformTagXmlSerializer LandformTagFileSerializer = new LandformTagXmlSerializer();

        void InitLandformTag(TerrainResources result)
        {
            string[] tags = LandformTagFileSerializer.Read();
            result.Internal_tags = tags;


        }
    }
}
