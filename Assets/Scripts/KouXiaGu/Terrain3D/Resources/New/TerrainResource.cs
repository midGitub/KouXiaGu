using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public IAsync<TerrainResource> ReadAsync(WorldElementResource elementInfos)
        {
            return TerrainResourceReader.ReadAsync(elementInfos);
        }

        /// <summary>
        /// 初始化为空;
        /// </summary>
        public TerrainResource()
        {
            LandformInfos = new Dictionary<int, TerrainLandform>();
        }

        public Dictionary<int, TerrainLandform> LandformInfos { get; private set; }


        /// <summary>
        /// 初始化方法;
        /// </summary>
        class TerrainResourceReader : MonoBehaviour, IAsync<TerrainResource>
        {
            const string assetBundleName = "terrain";
            static readonly ISegmented DefaultSegmented = new SegmentedBlock();
            internal static readonly LandformReader LandformReader = new LandformReader(DefaultSegmented);

            public static string AssetBundleFilePath
            {
                get { return ResourcePath.CombineAssetBundle(assetBundleName); }
            }

            /// <summary>
            /// 需要在Unity线程内调用;
            /// </summary>
            public static TerrainResourceReader ReadAsync(WorldElementResource elementInfos)
            {
                var gameObject = new GameObject("TerrainResourceReader", typeof(TerrainResourceReader));
                var item = gameObject.GetComponent<TerrainResourceReader>();
                item.elementInfos = elementInfos;
                return item;
            }


            TerrainResourceReader()
            {
            }

            WorldElementResource elementInfos;
            TerrainResource resource;
            public TerrainResource Result { get; private set; }
            public bool IsCompleted { get; private set; }
            public bool IsFaulted { get; private set; }
            public Exception Ex { get; private set; }

            void Awake()
            {
                resource = new TerrainResource();
                StartCoroutine(Read());

                Result = null;
                IsCompleted = false;
                IsFaulted = false;
                Ex = null;
            }

            IEnumerator Read()
            {
                AssetBundleCreateRequest bundleLoadRequest = AssetBundle.LoadFromFileAsync(AssetBundleFilePath);
                yield return bundleLoadRequest;
                AssetBundle assetBundle = bundleLoadRequest.assetBundle;
                if (assetBundle == null)
                {
                    IsFaulted = true;
                    IsCompleted = true;
                    Debug.LogError("未找到地形资源包;");
                    Ex = new FileNotFoundException("未找到地形资源包;");
                    yield break;
                }

                yield return LandformReader.Read(assetBundle, resource.LandformInfos, elementInfos.LandformInfos);

                assetBundle.Unload(false);
                IsCompleted = true;
                Result = resource;
            }
        }

    }

}
