
#if UNITY_EDITOR

namespace JiongXiaGu.EditorTool
{
    using UnityEditor;
    using UnityEngine;
    using System.IO;
    using JiongXiaGu.Resources;

    /// <summary>
    /// 构造资源包;
    /// </summary>
    public class ExportAssetBundles : MonoBehaviour
    {

        [MenuItem("Assets/Build AssetBundles")]
        static void BuildAllAssetBundles()
        {
            string directoryPath = Resource.AssetBundleDirectoryPath;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            BuildPipeline.BuildAssetBundles(directoryPath, BuildAssetBundleOptions.None,
                BuildTarget.StandaloneWindows);
        }

    }
}

#endif