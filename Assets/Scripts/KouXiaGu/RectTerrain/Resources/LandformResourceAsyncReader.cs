//using KouXiaGu.Concurrent;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace KouXiaGu.RectTerrain
//{

//    /// <summary>
//    /// 地貌资源读取;
//    /// </summary>
//    public class LandformResourceAsyncReader : LandformResource, IAsyncRequest
//    {
//        public LandformResourceAsyncReader(AssetBundle assetBundle, LandformResourceInfo info)
//        {
//            this.assetBundle = assetBundle;
//            Info = info;
//        }

//        AssetBundle assetBundle;
//        public bool InQueue { get; private set; }

//        void IAsyncRequest.OnAddQueue()
//        {
//            InQueue = true;
//        }

//        void IAsyncRequest.OnQuitQueue()
//        {
//            InQueue = false;
//            assetBundle = null;
//        }

//        bool IAsyncRequest.Prepare()
//        {
//            return true;
//        }

//        bool IAsyncRequest.Operate()
//        {
//            DiffuseTex = ReadTexture(Info.DiffuseTex);
//            DiffuseBlendTex = ReadTexture(Info.DiffuseBlendTex);
//            HeightTex = ReadTexture(Info.HeightTex);
//            HeightBlendTex = ReadTexture(Info.HeightBlendTex);

//            if (!IsComplete)
//            {
//                Debug.LogWarning("无法读取[LandformResource],Info:" + ToString());
//            }
//            return false;
//        }

//        Texture ReadTexture(string name)
//        {
//            return assetBundle.LoadAsset<Texture>(name);
//        }
//    }
//}
