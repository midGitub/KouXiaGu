using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    public class TerrainResource
    {

        static readonly ISegmented DefaultSegmented = new SegmentedBlock();
        static readonly LandformReader LandformReader = new LandformReader();

        /// <summary>
        /// 资源所在的资源包;
        /// </summary>
        const string assetBundleName = "terrain";

        public static string AssetBundleFilePath
        {
            get { return ResourcePath.CombineAssetBundle(assetBundleName); }
        }

        /// <summary>
        /// 通过协程初始化;
        /// </summary>
        public CoroutineOperation<TerrainResource> CreateAsync(WorldElementManager elementInfos)
        {
            return new AsyncInitializer(elementInfos);
        }


        public TerrainResource()
        {
            LandformInfos = new Dictionary<int, TerrainLandform>();
        }

        public TerrainResource(WorldElementManager elementInfos)
        {
            Initialize(elementInfos);
        }

        TerrainResource(bool none)
        {
        }


        public Dictionary<int, TerrainLandform> LandformInfos { get; private set; }


        void Initialize(WorldElementManager elementInfos)
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(AssetBundleFilePath);

            LandformInfos = LandformReader.Read(assetBundle, elementInfos.LandformInfos.Values);

            assetBundle.Unload(false);
            Log();
        }

        void Log()
        {
            string str = LandformInfos.ToLog();

            Debug.Log(str);
        }

        class AsyncInitializer : CoroutineOperation<TerrainResource>
        {
            public AsyncInitializer(WorldElementManager elementInfos)
            {
                Current = new TerrainResource(false);
                enumerator = InitializeAsync(elementInfos);
            }

            IEnumerator enumerator;

            public override bool MoveNext()
            {
                return enumerator.MoveNext();
            }

            IEnumerator InitializeAsync(WorldElementManager elementInfos)
            {
                AssetBundleCreateRequest bundleLoadRequest = AssetBundle.LoadFromFileAsync(AssetBundleFilePath);
                yield return bundleLoadRequest;
                AssetBundle assetBundle = bundleLoadRequest.assetBundle;
                if (assetBundle == null)
                {
                    Debug.LogError("目录不存在贴图资源包或者在编辑器中进行读取,地形资源初始化失败;");
                    yield break;
                }

                var landformReader = LandformReader.ReadAsync(assetBundle, elementInfos.LandformInfos.Values, DefaultSegmented);
                yield return landformReader;
                Current.LandformInfos = landformReader.Current;

                assetBundle.Unload(false);
                Current.Log();
            }
        }

    }

}
