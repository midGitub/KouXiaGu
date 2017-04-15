using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.World;
using UnityEngine;
using System.IO;

namespace KouXiaGu.Terrain3D
{

    public class TerrainResource
    {

        /// <summary>
        /// 需要在Unity线程内调用;
        /// </summary>
        public static IAsyncOperation<TerrainResource> ReadAsync(WorldElementResource elementInfos)
        {
            return new TerrainResourceCreater(elementInfos);
        }

        /// <summary>
        /// 初始化为空;
        /// </summary>
        public TerrainResource()
        {
            LandformInfos = new Dictionary<int, TerrainLandform>();
            RoadInfos = new Dictionary<int, TerrainRoad>();
        }

        public TerrainResource(bool none)
        {
        }

        public Dictionary<int, TerrainLandform> LandformInfos { get; private set; }
        public Dictionary<int, TerrainRoad> RoadInfos { get; private set; }

        /// <summary>
        /// 初始化方法;
        /// </summary>
        class TerrainResourceCreater : AsyncOperation<TerrainResource>
        {
            const string assetBundleName = "terrain";
            static readonly ISegmented DefaultSegmented = new SegmentedFalse();

            public static string AssetBundleFilePath
            {
                get { return ResourcePath.CombineAssetBundle(assetBundleName); }
            }

            public TerrainResourceCreater(WorldElementResource elementInfos)
            {
                this.elementInfos = elementInfos;
                resource = new TerrainResource();
                GameInitializer._StartCoroutine(Read());
            }

            TerrainResource resource;
            WorldElementResource elementInfos;
            AssetBundle assetBundle;

            IEnumerator Read()
            {
                if (IsFaulted)
                    goto _Last_;
                yield return ReadAssetBundle();

                if (IsFaulted)
                    goto _Last_;
                yield return ReadLandform(assetBundle);

                if (IsFaulted)
                    goto _Last_;
                yield return ReadRoad(assetBundle);

                _Last_:
                LastOperation();
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

            void LastOperation()
            {
                if(assetBundle != null)
                    assetBundle.Unload(false);

                if(!IsCompleted)
                    OnCompleted(resource);
            }

            IEnumerator ReadLandform(AssetBundle assetBundle)
            {
                var request = new LandformReadRequest(assetBundle, DefaultSegmented, elementInfos);
                yield return request;

                if (request.IsFaulted)
                {
                    Debug.LogError(request.Exception);
                    OnFaulted(request.Exception);
                }
                else
                {
                    resource.LandformInfos = request.Result;
                }
            }

            IEnumerator ReadRoad(AssetBundle assetBundle)
            {
                var request = new RoadReadRequest(assetBundle, DefaultSegmented, elementInfos);
                yield return request;

                if (request.IsFaulted)
                {
                    Debug.LogError(request.Exception);
                    OnFaulted(request.Exception);
                }
                else
                {
                    resource.RoadInfos = request.Result;
                }
            }

        }

    }

}
