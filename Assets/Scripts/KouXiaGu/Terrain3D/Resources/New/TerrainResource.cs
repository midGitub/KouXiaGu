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
        public static IAsyncOperation<TerrainResource> ReadAsync(WorldElementResource elementInfos)
        {
            return TerrainResourceCreater.Create(elementInfos);
        }

        /// <summary>
        /// 初始化为空;
        /// </summary>
        public TerrainResource()
        {
            LandformInfos = new Dictionary<int, TerrainLandform>();
        }

        public Dictionary<int, TerrainLandform> LandformInfos { get; private set; }


        public string ToLog()
        {
            string str = "Terrain:" +
                "\nLandform:" + LandformInfos.Count +
                "\n";

            return str;
        }

        /// <summary>
        /// 初始化方法;
        /// </summary>
        class TerrainResourceCreater : CoroutineOperation<TerrainResource>
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
            public static TerrainResourceCreater Create(WorldElementResource elementInfos)
            {
                var gameObject = new GameObject("TerrainResourceReader", typeof(TerrainResourceCreater));
                var item = gameObject.GetComponent<TerrainResourceCreater>();
                item.elementInfos = elementInfos;
                return item;
            }


            TerrainResourceCreater()
            {
            }

            WorldElementResource elementInfos;
            TerrainResource resource;

            protected override void Awake()
            {
                base.Awake();
                resource = new TerrainResource();
                StartCoroutine(Read());
            }

            IEnumerator Read()
            {
                AssetBundleCreateRequest bundleLoadRequest = AssetBundle.LoadFromFileAsync(AssetBundleFilePath);
                yield return bundleLoadRequest;
                AssetBundle assetBundle = bundleLoadRequest.assetBundle;
                if (assetBundle == null)
                {
                    Exception ex = new FileNotFoundException("未找到地形资源包;");
                    Debug.LogError(ex);
                    OnError(ex);
                    yield break;
                }

                yield return LandformReader.Read(assetBundle, resource.LandformInfos, elementInfos.LandformInfos);

                assetBundle.Unload(false);
                Destroy(gameObject);
                OnCompleted(resource);
            }

        }

    }

}
