//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace KouXiaGu.Terrain3D
//{


//    public static class TerrainAssetBundle
//    {

//        public const string assetBundleName = "terrain";

//        public static string AssetBundleFilePath
//        {
//            get { return ResourcePath.CombineAssetBundle(assetBundleName); }
//        }

//        public static AssetBundle Load()
//        {
//            return AssetBundle.LoadFromFile(AssetBundleFilePath);
//        }

//        public static AssetBundleCreateRequest LoadAsync()
//        {
//            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(AssetBundleFilePath);
//            return bundleLoadRequest;
//        }

//    }

//}
