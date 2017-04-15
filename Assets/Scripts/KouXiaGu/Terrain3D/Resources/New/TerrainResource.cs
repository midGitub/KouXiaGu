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
        }

        public TerrainResource(bool none)
        {
        }

        public Dictionary<int, TerrainLandform> LandformInfos { get; private set; }

        class Tt : ISegmented
        {
            bool ISegmented.KeepWait()
            {
                return true;
            }
        }

        /// <summary>
        /// 初始化方法;
        /// </summary>
        class TerrainResourceCreater : AsyncOperation<TerrainResource>
        {
            const string assetBundleName = "terrain";
            static readonly ISegmented DefaultSegmented = new SegmentedBlock();

            public static string AssetBundleFilePath
            {
                get { return ResourcePath.CombineAssetBundle(assetBundleName); }
            }

            public TerrainResourceCreater(WorldElementResource elementInfos)
            {
                this.elementInfos = elementInfos;
                resource = new TerrainResource(false);
                GameInitializer._StartCoroutine(Read());
            }

            WorldElementResource elementInfos;
            TerrainResource resource;

            IEnumerator Read()
            {
                AssetBundleCreateRequest bundleLoadRequest = AssetBundle.LoadFromFileAsync(AssetBundleFilePath);
                yield return bundleLoadRequest;
                AssetBundle assetBundle = bundleLoadRequest.assetBundle;
                if (assetBundle == null)
                {
                    Exception ex = new FileNotFoundException("未找到地形资源包;");
                    Debug.LogError(ex);
                    OnFaulted(ex);
                    yield break;
                }

                var landformRequest = new LandformReadRequest(assetBundle, DefaultSegmented, elementInfos);
                yield return landformRequest;
                if (landformRequest.IsFaulted)
                {
                    Debug.LogError(landformRequest.Exception);
                    resource.LandformInfos = new Dictionary<int, TerrainLandform>();
                }
                else
                {
                    resource.LandformInfos = landformRequest.Result;
                }

                assetBundle.Unload(false);
                OnCompleted(resource);
            }


        }

    }

}
