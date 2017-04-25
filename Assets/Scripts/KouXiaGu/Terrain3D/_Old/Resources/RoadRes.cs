//using System;
//using System.Collections;
//using System.Collections.Generic;
//using KouXiaGu.Collections;
//using UnityEngine;

//namespace KouXiaGu.Terrain3D
//{

//    /// <summary>
//    /// 道路贴图资源;
//    /// </summary>
//    public sealed class RoadRes : IDisposable
//    {

//        #region 已初始化合集(静态);

//        /// <summary>
//        /// 所有已经初始化完毕的道路信息;
//        /// </summary>
//        static readonly CustomDictionary<int, RoadRes> initializedDictionary = new CustomDictionary<int, RoadRes>();

//        /// <summary>
//        /// 所有已经初始化完毕的道路信息;
//        /// </summary>
//        public static IReadOnlyDictionary<int, RoadRes> initializedInstances
//        {
//            get { return initializedDictionary; }
//        }

//        #endregion


//        #region 初始化方法;

//        /// <summary>
//        /// 初始化所有信息;
//        /// </summary>
//        public static IEnumerator Load(IEnumerable<RoadDescr> descriptions, AssetBundle asset)
//        {
//            foreach (var description in descriptions)
//            {
//                if (initializedDictionary.ContainsKey(description.ID))
//                {
//                    Debug.LogWarning("道路:已经存在相同的ID;跳过此:" + description.ToString());
//                    continue;
//                }

//                RoadRes res = new RoadRes(description);
//                res.Load(asset);

//                if (res.IsLoadComplete)
//                {
//                    initializedDictionary.Add(description.ID, res);
//                }
//                else
//                {
//                    Debug.LogWarning("道路:初始化失败,跳过此:" + description.ToString());
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

//        RoadRes(RoadDescr description)
//        {
//            this.Description = description;
//        }

//        public RoadDescr Description { get; private set; }
//        public Texture HeightAdjustTex { get; private set; }
//        public Texture HeightAdjustBlendTex { get; private set; }
//        public Texture DiffuseTex { get; private set; }
//        public Texture DiffuseBlendTex { get; private set; }

//        /// <summary>
//        /// 是否不为空?
//        /// </summary>
//        public bool IsNotEmpty
//        {
//            get { return DiffuseTex != null || HeightAdjustTex != null || DiffuseBlendTex != null || HeightAdjustBlendTex != null; }
//        }

//        /// <summary>
//        /// 贴图读取完毕?
//        /// </summary>
//        public bool IsLoadComplete
//        {
//            get { return DiffuseTex != null && HeightAdjustTex != null && DiffuseBlendTex != null && HeightAdjustBlendTex != null; }
//        }

//        /// <summary>
//        /// 清除所有资源;
//        /// </summary>
//        public void Dispose()
//        {
//            if (IsNotEmpty)
//            {
//                GameObject.Destroy(this.DiffuseTex);
//                GameObject.Destroy(this.HeightAdjustTex);
//                GameObject.Destroy(this.DiffuseBlendTex);
//                GameObject.Destroy(this.HeightAdjustBlendTex);

//                this.DiffuseTex = null;
//                this.HeightAdjustTex = null;
//                this.DiffuseBlendTex = null;
//                this.HeightAdjustBlendTex = null;
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
//                Texture height = LoadTexture(assetBundle, Description.HeightAdjustTex);
//                Texture diffuseBlend = LoadTexture(assetBundle, Description.DiffuseBlendTex);
//                Texture heightBlend = LoadTexture(assetBundle, Description.HeightAdjustTex);

//                this.DiffuseTex = diffuse;
//                this.HeightAdjustTex = height;
//                this.DiffuseBlendTex = diffuseBlend;
//                this.HeightAdjustBlendTex = heightBlend;
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
