//using System;
//using System.Collections;
//using System.Collections.Generic;
//using KouXiaGu.Collections;
//using UnityEngine;

//namespace KouXiaGu.Terrain3D
//{


//    public class LandformRes : IDisposable
//    {

//        #region 已初始化合集(静态);

//        /// <summary>
//        /// 所有已经初始化完毕的道路信息;
//        /// </summary>
//        static readonly CustomDictionary<int, LandformRes> initializedDictionary = new CustomDictionary<int, LandformRes>();

//        /// <summary>
//        /// 所有已经初始化完毕的道路信息;
//        /// </summary>
//        public static IReadOnlyDictionary<int, LandformRes> initializedInstances
//        {
//            get { return initializedDictionary; }
//        }

//        #endregion


//        #region 初始化方法;

//        /// <summary>
//        /// 初始化所有信息;
//        /// </summary>
//        public static IEnumerator Load(IEnumerable<LandformDescr> descriptions, AssetBundle asset)
//        {
//            foreach (var description in descriptions)
//            {
//                if (initializedDictionary.ContainsKey(description.ID))
//                {
//                    Debug.LogWarning("地貌:已经存在相同的ID;跳过此:" + description.ToString());
//                    continue;
//                }

//                LandformRes res = new LandformRes(description);
//                res.Load(asset);

//                if (res.IsLoadComplete)
//                {
//                    initializedDictionary.Add(description.ID, res);
//                }
//                else
//                {
//                    Debug.LogWarning("地貌:初始化失败,跳过此:" + description.ToString());
//                    res.Dispose();
//                    continue;
//                }
//                yield return null;
//            }
//            yield break;
//        }

//        /// <summary>
//        /// 清除所有已经初始化的道路信息;
//        /// </summary>
//        public static void Clear()
//        {
//            foreach (var road in initializedDictionary.Values)
//            {
//                road.Dispose();
//            }
//            initializedDictionary.Clear();
//        }

//        #endregion


//        #region 实例部分;

//        LandformRes(LandformDescr description)
//        {
//            this.Description = description;
//        }

//        public LandformDescr Description { get; private set; }
//        public Texture HeightTex { get; private set; }
//        public Texture HeightBlendTex { get; private set; }
//        public Texture DiffuseTex { get; private set; }
//        public Texture DiffuseBlendTex { get; private set; }

//        /// <summary>
//        /// 是否不为空?
//        /// </summary>
//        public bool IsNotEmpty
//        {
//            get { return DiffuseTex != null || HeightTex != null || DiffuseBlendTex != null || HeightBlendTex != null; }
//        }

//        /// <summary>
//        /// 贴图读取完毕?
//        /// </summary>
//        public bool IsLoadComplete
//        {
//            get { return DiffuseTex != null && HeightTex != null && DiffuseBlendTex != null && HeightBlendTex != null; }
//        }

//        /// <summary>
//        /// 清除所有资源;
//        /// </summary>
//        public void Dispose()
//        {
//            if (IsNotEmpty)
//            {
//                GameObject.Destroy(this.DiffuseTex);
//                GameObject.Destroy(this.HeightTex);
//                GameObject.Destroy(this.DiffuseBlendTex);
//                GameObject.Destroy(this.HeightBlendTex);

//                this.DiffuseTex = null;
//                this.HeightTex = null;
//                this.DiffuseBlendTex = null;
//                this.HeightBlendTex = null;
//            }
//        }

//        /// <summary>
//        /// 从资源包同步读取到;
//        /// </summary>
//        void Load(AssetBundle assetBundle)
//        {
//            if (IsNotEmpty)
//                throw new ArgumentException();

//            try
//            {
//                Texture diffuse = LoadTexture(assetBundle, Description.DiffuseTex);
//                Texture height = LoadTexture(assetBundle, Description.HeightTex);
//                Texture diffuseBlend = LoadTexture(assetBundle, Description.DiffuseBlendTex);
//                Texture heightBlend = LoadTexture(assetBundle, Description.HeightBlendTex);

//                this.DiffuseTex = diffuse;
//                this.HeightTex = height;
//                this.DiffuseBlendTex = diffuseBlend;
//                this.HeightBlendTex = heightBlend;
//            }
//            catch (Exception e)
//            {
//                Dispose();
//                throw e;
//            }
//        }

//        Texture LoadTexture(AssetBundle assetBundle, string name)
//        {
//            return assetBundle.LoadAsset<Texture>(name);
//        }

//        public override bool Equals(object obj)
//        {
//            return this.Description.Equals(obj);
//        }

//        public override int GetHashCode()
//        {
//            return this.Description.GetHashCode();
//        }

//        public override string ToString()
//        {
//            string info = this.Description.ToString() + "[LoadComplete:" + IsLoadComplete + "]";
//            return info;
//        }

//        #endregion

//    }

//}
