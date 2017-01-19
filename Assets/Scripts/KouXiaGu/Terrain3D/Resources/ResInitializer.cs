using System;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形资源初始化控制;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ResInitializer : SceneSington<ResInitializer>
    {
        static ResInitializer()
        {
            IsInitialized = false;
        }

        ResInitializer() { }

        /// <summary>
        /// 资源所在的资源包;
        /// </summary>
        [SerializeField]
        string assetBundleName = "terrain";

        /// <summary>
        /// 地貌配置文件名;
        /// </summary>
        [SerializeField]
        string landformDescrName = "LandformDescr.xml";

        /// <summary>
        /// 道路配置文件名;
        /// </summary>
        [SerializeField]
        string roadDescrName = "RoadDescr.xml";

        /// <summary>
        /// 建筑信息配置文件名;
        /// </summary>
        [SerializeField]
        string buildingDescrName = "Building.xml";


        public static bool IsInitialized { get; private set; }

        public static string ResAssetBundleFile
        {
            get { return ResourcePath.CombineAssetBundle(GetInstance.assetBundleName); }
        }

        public static string LandformDescrFile
        {
            get { return TerrainFiler.Combine(GetInstance.landformDescrName); }
        }

        public static string RoadDescrFile
        {
            get { return TerrainFiler.Combine(GetInstance.roadDescrName); }
        }

        public static string BuildingDescrFile
        {
            get { return TerrainFiler.Combine(GetInstance.buildingDescrName); }
        }


        /// <summary>
        /// 初始化地形资源;
        /// </summary>
        public static IEnumerator Initialize()
        {
            if (IsInitialized)
            {
                Debug.LogWarning("地形资源已经初始化完毕,无需再次初始化;");
                yield break;
            }

            var bundleLoadRequest = LoadAssetAsync();
            yield return bundleLoadRequest;
            AssetBundle assetBundle = bundleLoadRequest.assetBundle;
            if (assetBundle == null)
            {
                Debug.LogError("目录不存在贴图资源包或者在编辑器中进行读取,地形资源初始化失败;");
                yield break;
            }

            yield return LoadLandformRes(assetBundle);
            yield return LoadRoadRes(assetBundle);
            yield return LoadBuildingRes(assetBundle);

            assetBundle.Unload(false);
            IsInitialized = true;
            yield break;
        }

        /// <summary>
        /// 异步读取资源包;
        /// </summary>
        static AssetBundleCreateRequest LoadAssetAsync()
        {
            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(ResAssetBundleFile);
            return bundleLoadRequest;
        }

        /// <summary>
        /// 读取地貌资源;
        /// </summary>
        static IEnumerator LoadLandformRes(AssetBundle assetBundle)
        {
            LandformDescr[] landformDescrs;

            if (TryDeserialize(LandformDescr.ArraySerializer, LandformDescrFile, out landformDescrs))
            {
                yield return LandformRes.Load(landformDescrs, assetBundle);
                Debug.Log(LandformRes.initializedInstances.ToLog("地貌资源初始化完成;"));
            }
            else
            {
                throw new LackOfResourcesException("缺少必要的描述文件用于初始化;");
            }

            yield break;
        }

        /// <summary>
        /// 读取道路资源;
        /// </summary>
        static IEnumerator LoadRoadRes(AssetBundle assetBundle)
        {
            RoadDescr[] roadDescrs;

            if (TryDeserialize(RoadDescr.ArraySerializer, RoadDescrFile, out roadDescrs))
            {
                yield return RoadRes.Load(roadDescrs, assetBundle);
                Debug.Log(RoadRes.initializedInstances.ToLog("道路资源初始化完成;"));
            }
            else
            {
                throw new LackOfResourcesException("缺少必要的描述文件用于初始化;");
            }

            yield break;
        }
         
        static IEnumerator LoadBuildingRes(AssetBundle assetBundle)
        {
            BuildingDescr[] buildingDescr;

            if (TryDeserialize(BuildingDescr.ArraySerializer, BuildingDescrFile, out buildingDescr))
            {
                yield return BuildingRes.Load(buildingDescr, assetBundle);
                Debug.Log(BuildingRes.initializedInstances.ToLog("建筑资源初始化完成;"));
            }
            else
            {
                throw new LackOfResourcesException("缺少必要的描述文件用于初始化;");
            }

            yield break;
        }

        /// <summary>
        /// 反序列化,若无法序列化则返回false;
        /// </summary>
        static bool TryDeserialize<T>(XmlSerializer serializer, string filePath, out T item)
        {
            try
            {
                item = (T)serializer.DeserializeXiaGu(filePath);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError("地形:无法找到文件或读取文件:" + filePath + ";\n" + ex);
                item = default(T);
                return false;
            }
        }

        /// <summary>
        /// 清除所有已经初始化的资源;
        /// </summary>
        public static void Clear()
        {
            LandformRes.Clear();
            RoadRes.Clear();
            BuildingRes.Clear();

            IsInitialized = false;
        }

    }

}
