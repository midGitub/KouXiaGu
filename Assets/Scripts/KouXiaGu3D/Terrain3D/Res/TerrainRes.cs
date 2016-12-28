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
    public sealed class TerrainRes : UnitySington<TerrainRes>
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
        string landformDescrName = "LandformDescr.xml";

        /// <summary>
        /// 道路配置文件名;
        /// </summary>
        [SerializeField]
        string roadDescrName = "RoadDescr.xml";

        public static string ResAssetBundleFile
        {
            get { return ResourcePath.CombineAssetBundle(GetInstance.resAssetBundleName); }
        }

        public static string LandformDescrFile
        {
            get { return TerrainResPath.Combine(GetInstance.landformDescrName); }
        }

        public static string RoadDescrFile
        {
            get { return TerrainResPath.Combine(GetInstance.roadDescrName); }
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
                IEnumerator loader = LandformRes.Load(landformDescrs, assetBundle);
                while (loader.MoveNext())
                    yield return null;

                Debug.Log(LandformRes.initializedInstances.ToLog("地貌资源初始化完成;"));
            }
            else
            {
                throw new LackOfResourcesException("地貌:缺少必要的资源用于初始化;");
            }

            yield break;
        }

        /// <summary>
        /// 读取道路资源;
        /// </summary>
        /// <returns></returns>
        static IEnumerator LoadRoadRes(AssetBundle assetBundle)
        {
            RoadDescr[] roadDescrs;

            if (TryDeserialize(RoadDescr.ArraySerializer, RoadDescrFile, out roadDescrs))
            {
                IEnumerator loader = RoadRes.Load(roadDescrs, assetBundle);
                while (loader.MoveNext())
                    yield return null;

                Debug.Log(RoadRes.initializedInstances.ToEnumerableLog("道路资源初始化完成;"));
            }
            else
            {
                throw new LackOfResourcesException("道路:缺少必要的资源用于初始化;");
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
        }

    }

}
