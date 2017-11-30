using UnityEditor;
using System.IO;
using UnityEngine;
using JiongXiaGu.Unity.Resources;

namespace JiongXiaGu.EditorTool
{
    /// <summary>
    /// 构造资源包;
    /// </summary>
    public static class ExportAssetBundles
    {

        //public static string AssetBundleExtension
        //{
        //    get { return ResourcePath.AssetBundleExtension; }
        //}

        [MenuItem("Assets/Build AssetBundles")]
        public static void BuildAssetBundleAll()
        {
            string directoryPath = Path.Combine(ResourcePath.GetCoreDirectoryInfo(), "AssetBundles");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var assetBundleManifest = BuildPipeline.BuildAssetBundles(directoryPath, BuildAssetBundleOptions.UncompressedAssetBundle,
                BuildTarget.StandaloneWindows);
            //ChangeExtension(directoryPath, assetBundleManifest);
        }

        [MenuItem("Assets/Rebuild AssetBundles")]
        public static void RebuildAssetBundleAll()
        {
            string directoryPath = Path.Combine(ResourcePath.GetCoreDirectoryInfo(), "AssetBundles");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var assetBundleManifest = BuildPipeline.BuildAssetBundles(directoryPath, BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.ForceRebuildAssetBundle,
                BuildTarget.StandaloneWindows);
            //ChangeExtension(directoryPath, assetBundleManifest);
        }

        //private static void ChangeExtension(string directory, AssetBundleManifest assetBundleManifest)
        //{
        //    var names = assetBundleManifest.GetAllAssetBundles();
        //    foreach (var name in names)
        //    {
        //        ChangeExtension(directory, name);
        //    }
        //}

        //private static void ChangeExtension(string directory, string name)
        //{
        //    string filePath = Path.Combine(directory, name);
        //    string newFilePath = Path.ChangeExtension(filePath, AssetBundleExtension);
        //    FileInfo fileInfo = new FileInfo(filePath);
        //    if (fileInfo.Exists)
        //    {
        //        fileInfo.MoveTo(newFilePath);
        //    }
        //    else
        //    {
        //        if (!File.Exists(newFilePath))
        //        {
        //            throw new FileNotFoundException(newFilePath);
        //        }
        //    }
        //}
    }
}