using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 道路贴图资源;
    /// </summary>
    public sealed class Road
    {

        #region 已初始化合集(静态);

        /// <summary>
        /// 所有已经初始化完毕的道路信息;
        /// </summary>
        static readonly CustomDictionary<int, Road> initializedDictionary = new CustomDictionary<int, Road>();

        /// <summary>
        /// 所有已经初始化完毕的道路信息;
        /// </summary>
        public static IReadOnlyDictionary<int, Road> initializedInstances
        {
            get { return initializedDictionary; }
        }

        #endregion


        #region 初始化方法;

        /// <summary>
        /// 初始化所有信息;
        /// </summary>
        public static IEnumerator LoadAll(IEnumerable<RoadDescr> descriptions, AssetBundle asset)
        {
            foreach (var description in descriptions)
            {
                if (initializedDictionary.ContainsKey(description.ID))
                {
                    Debug.LogWarning("道路:已经存在相同的ID;跳过此:" + description.ToString());
                    continue;
                }

                Road road = new Road(description);
                road.Load(asset);

                if (road.IsLoadComplete)
                {
                    initializedDictionary.Add(description.ID, road);
                }
                else
                {
                    Debug.LogWarning("道路:初始化失败,跳过此:" + description.ToString());
                    road.Destroy();
                    continue;
                }
                yield return null;
            }
            yield break;
        }

        /// <summary>
        /// 清除所有已经初始化的道路信息;
        /// </summary>
        public static void ClearAll()
        {
            foreach (var road in initializedDictionary.Values)
            {
                road.Destroy();
            }
            initializedDictionary.Clear();
        }

        #endregion


        #region 实例部分;

        Road(RoadDescr description)
        {
            this.Description = description;
        }

        public RoadDescr Description { get; private set; }
        public Texture HeightAdjustTex { get; private set; }
        public Texture HeightAdjustBlendTex { get; private set; }
        public Texture DiffuseTex { get; private set; }
        public Texture DiffuseBlendTex { get; private set; }

        /// <summary>
        /// 是否不为空?
        /// </summary>
        public bool IsNotEmpty
        {
            get { return DiffuseTex != null || HeightAdjustTex != null || DiffuseBlendTex != null || HeightAdjustBlendTex != null; }
        }

        /// <summary>
        /// 贴图读取完毕?
        /// </summary>
        public bool IsLoadComplete
        {
            get { return DiffuseTex != null && HeightAdjustTex != null && DiffuseBlendTex != null && HeightAdjustBlendTex != null; }
        }

        /// <summary>
        /// 清除所有资源;
        /// </summary>
        void Destroy()
        {
            if (IsNotEmpty)
            {
                GameObject.Destroy(this.DiffuseTex);
                GameObject.Destroy(this.HeightAdjustTex);
                GameObject.Destroy(this.DiffuseBlendTex);
                GameObject.Destroy(this.HeightAdjustBlendTex);

                this.DiffuseTex = null;
                this.HeightAdjustTex = null;
                this.DiffuseBlendTex = null;
                this.HeightAdjustBlendTex = null;
            }
        }

        /// <summary>
        /// 从资源包同步读取到;
        /// </summary>
        void Load(AssetBundle assetBundle)
        {
            if (IsNotEmpty)
                throw new ArgumentException();

            try
            {
                Texture diffuse = assetBundle.LoadAsset<Texture>(Description.DiffuseTex);
                Texture height = assetBundle.LoadAsset<Texture>(Description.HeightAdjustTex);
                Texture diffuseBlend = assetBundle.LoadAsset<Texture>(Description.DiffuseBlendTex);
                Texture heightBlend = assetBundle.LoadAsset<Texture>(Description.HeightAdjustTex);

                this.DiffuseTex = diffuse;
                this.HeightAdjustTex = height;
                this.DiffuseBlendTex = diffuseBlend;
                this.HeightAdjustBlendTex = heightBlend;
            }
            catch (Exception e)
            {
                Destroy();
                throw e;
            }
        }

        public override bool Equals(object obj)
        {
            return this.Description.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Description.GetHashCode();
        }

        public override string ToString()
        {
            string info = this.Description.ToString() + "[LoadComplete:" + IsLoadComplete + "]";
            return info;
        }

        #endregion

    }

}
