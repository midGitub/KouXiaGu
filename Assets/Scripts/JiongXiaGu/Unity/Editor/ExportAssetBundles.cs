using UnityEditor;
using UnityEngine;
using System.IO;
using JiongXiaGu.Unity.Resources;

namespace JiongXiaGu.EditorTool
{
    /// <summary>
    /// 构造资源包;
    /// </summary>
    public static class ExportAssetBundles
    {
        [MenuItem("Assets/Build AssetBundles")]
        public static void BuildAssetBundleAll()
        {
            string directoryPath = Path.Combine(ResourcePath.GetCoreDirectoryInfo().FullName, "AssetBundles");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            BuildPipeline.BuildAssetBundles(directoryPath, BuildAssetBundleOptions.UncompressedAssetBundle,
                BuildTarget.StandaloneWindows);
        }

        [MenuItem("Assets/Rebuild AssetBundles")]
        public static void RebuildAssetBundleAll()
        {
            string directoryPath = Path.Combine(ResourcePath.GetCoreDirectoryInfo().FullName, "AssetBundles");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            BuildPipeline.BuildAssetBundles(directoryPath, BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.ForceRebuildAssetBundle,
                BuildTarget.StandaloneWindows);
        }
    }
}