using KouXiaGu.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using KouXiaGu.Concurrent;

namespace KouXiaGu.Terrain3D
{

    class AssetBundleReader : AsyncOperation<AssetBundle>, IAsyncRequest
    {
        const string assetBundleName = "terrain";

        string AssetBundleFilePath
        {
            get { return Path.Combine(Resource.AssetBundleDirectoryPath, assetBundleName); }
        }

        void IAsyncRequest.AddQueue() { }

        void IAsyncRequest.Operate()
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(AssetBundleFilePath);
            OnCompleted(assetBundle);
        }
    }

    /// <summary>
    /// 在非主线程初始化地形资源;
    /// </summary>
    public class TerrainResourcesFileReader
    {
        public TerrainResourcesFileReader()
        {
            dispatcher = GameResourceUnityDispatcher.Instance;
        }

        GameResourceUnityDispatcher dispatcher;
        internal LandformInfoXmlSerializer LandformFileSerializer = new LandformInfoXmlSerializer();
        internal BuildingInfoXmlSerializer BuildingFileSerializer = new BuildingInfoXmlSerializer();
        internal RoadInfoXmlSerializer RoadFileSerializer = new RoadInfoXmlSerializer();

        public TerrainResources Read(ISign sign)
        {
            if (XiaGu.IsMainThread)
            {
                throw new ArgumentException("只允许在非Unity线程调用;");
            }

            var assetBundleReader = new AssetBundleReader();
            dispatcher.Add(assetBundleReader);
            while (!assetBundleReader.IsCompleted)
            {
                if (sign.IsCanceled)
                    throw new OperationCanceledException();
            }
            AssetBundle assetBundle = assetBundleReader.Result;

            var landformInfos = LandformFileSerializer.Read();
            foreach (var landformInfo in landformInfos.Values)
            {
                LandformResourceReader request = new LandformResourceReader(assetBundle, landformInfo);
                dispatcher.Add(request);
            }

            var buildingInfos = BuildingFileSerializer.Read();
            foreach (var buildingInfo in buildingInfos.Values)
            {
                BuildingResourceReader request = new BuildingResourceReader(assetBundle, buildingInfo);
                dispatcher.Add(request);
            }

            var roadInfos = RoadFileSerializer.Read();
            foreach (var roadInfo in roadInfos.Values)
            {
                RoadResourceReader request = new RoadResourceReader(assetBundle, roadInfo);
                dispatcher.Add(request);
            }

            while (dispatcher.RequestCount != 0)
            {
                if (sign.IsCanceled)
                    throw new OperationCanceledException();
            }

            TerrainResources Result = new TerrainResources()
            {
                Landform = landformInfos,
                Building = buildingInfos,
                Road = roadInfos,
            };

            InitLandformTag(Result);
            return Result;
        }

        internal LandformTagXmlSerializer LandformTagFileSerializer = new LandformTagXmlSerializer();

        void InitLandformTag(TerrainResources result)
        {
            string[] tags = LandformTagFileSerializer.Read();
            LandformTag tagConverter = new LandformTag(tags);
            result.Tags = tagConverter;

            foreach (var info in result.Landform.Values)
            {
                info.TagsMask = tagConverter.TagsToMask(info.Tags);
            }

            foreach (var info in result.Building.Values)
            {
                info.TagsMask = tagConverter.TagsToMask(info.Tags);
            }
        }
    }
}
