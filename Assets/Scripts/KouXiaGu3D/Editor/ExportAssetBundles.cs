
#if UNITY_EDITOR

namespace KouXiaGu.Editor
{
    using UnityEditor;
    using UnityEngine;
    using System.IO;

    /// <summary>
    /// 构造资源包;
    /// </summary>
    public class ExportAssetBundles : MonoBehaviour
    {

        [MenuItem("Assets/Build AssetBundles")]
        static void BuildAllAssetBundles()
        {
            string directoryPath = ResourcePath.AssetBundleDirectoryPath;
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