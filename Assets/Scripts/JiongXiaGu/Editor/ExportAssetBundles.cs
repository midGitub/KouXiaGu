using UnityEditor;
using UnityEngine;
using System.IO;
using JiongXiaGu.Unity.Resources;

namespace JiongXiaGu.EditorTool
{
    /// <summary>
    /// 构造资源包;
    /// </summary>
    public class ExportAssetBundles : MonoBehaviour
    {

        [MenuItem("Assets/Build AssetBundles")]
        static void BuildAllAssetBundles()
        {
            string directoryPath = AssetBundleReader.GetAssetBundlesDirectory();
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            BuildPipeline.BuildAssetBundles(directoryPath, BuildAssetBundleOptions.UncompressedAssetBundle,
                BuildTarget.StandaloneWindows);
        }
    }
}