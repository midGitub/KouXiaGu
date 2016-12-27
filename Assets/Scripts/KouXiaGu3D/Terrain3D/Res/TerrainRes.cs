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
    public sealed class TerrainRes : UnitySingleton<TerrainRes>
    {
        TerrainRes() { }

        /// <summary>
        /// 资源所在的资源包;
        /// </summary>
        string resAssetBundleName;

        /// <summary>
        /// 初始化地形资源;
        /// </summary>
        public static IEnumerator Initialize()
        {
            var request = LoadAssetAsync();
            while (!request.isDone)
                yield return null;

            yield break;
        }

        /// <summary>
        /// 异步读取资源包;
        /// </summary>
        public static AssetBundleCreateRequest LoadAssetAsync()
        {
            string filePath = ResourcePath.CombineAssetBundle(GetInstance.resAssetBundleName);
            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(filePath);
            return bundleLoadRequest;
        }

        /// <summary>
        /// 清除所有已经初始化的资源;
        /// </summary>
        public static void Clear()
        {

        }

    }

}
