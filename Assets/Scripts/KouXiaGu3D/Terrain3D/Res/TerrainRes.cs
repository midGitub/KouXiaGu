using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形资源初始化控制;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TerrainRes : UnitySingleton<TerrainRes>
    {
        TerrainRes() { }

        /// <summary>
        /// 资源所在的资源包;
        /// </summary>
        [SerializeField]
        string resAssetBundleName = "terrain";

        /// <summary>
        /// 地貌配置文件名;
        /// </summary>
        [SerializeField]
        string landformDescrFile = "LandformDescr.xml";

        /// <summary>
        /// 道路配置文件名;
        /// </summary>
        [SerializeField]
        string roadDescrFile = "RoadDescr.xml";

        public static string ResAssetBundleName
        {
            get { return GetInstance.resAssetBundleName; }
        }

        public static string LandformDescrFile
        {
            get { return GetInstance.landformDescrFile; }
        }

        public static string RoadDescrFile
        {
            get { return GetInstance.roadDescrFile; }
        }

        /// <summary>
        /// 初始化地形资源;
        /// </summary>
        public static IEnumerator Initialize()
        {
            var bundleLoadRequest = LoadAssetAsync();
            while (!bundleLoadRequest.isDone)
                yield return null;

            AssetBundle assetBundle = bundleLoadRequest.assetBundle;
            if (assetBundle == null)
            {
                Debug.LogError("目录不存在贴图资源包或者在编辑器中进行读取,地形资源初始化失败;");
                yield break;
            }

            IEnumerator loader;

            loader = LoadLandformRes(assetBundle);
            while (loader.MoveNext())
                yield return null;

            loader = LoadRoadRes(assetBundle);
            while (loader.MoveNext())
                yield return null;

            assetBundle.Unload(false);
            yield break;
        }

        /// <summary>
        /// 异步读取资源包;
        /// </summary>
        static AssetBundleCreateRequest LoadAssetAsync()
        {
            string filePath = ResourcePath.CombineAssetBundle(ResAssetBundleName);
            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(filePath);
            return bundleLoadRequest;
        }

        /// <summary>
        /// 读取地貌资源;
        /// </summary>
        static IEnumerator LoadLandformRes(AssetBundle assetBundle)
        {
            string descrFilePath = TerrainResPath.Combine(LandformDescrFile);


            yield break;
        }

        /// <summary>
        /// 读取道路资源;
        /// </summary>
        /// <returns></returns>
        static IEnumerator LoadRoadRes(AssetBundle assetBundle)
        {

            yield break;
        }

        /// <summary>
        /// 清除所有已经初始化的资源;
        /// </summary>
        public static void Clear()
        {

        }

    }

}
