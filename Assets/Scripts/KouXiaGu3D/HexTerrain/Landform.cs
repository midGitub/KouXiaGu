using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Collections;

namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 地貌定义;
    /// </summary>
    public class Landform
    {

        public Landform()
        {
        }
        public Landform(int id) : this()
        {
            this.ID = id;
        }

        /// <summary>
        /// 地形名;
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 地形唯一标示(0,-1作为保留);
        /// </summary>
        [XmlAttribute("id")]
        public int ID { get; set; }

        // 贴图名或路径定义;
        [XmlElement("diffusePath")]
        public string diffusePath { get; set; }

        [XmlElement("heightPath")]
        public string heightPath { get; set; }

        [XmlElement("mixerPath")]
        public string mixerPath { get; set; }

        /// <summary>
        /// 漫反射贴图;
        /// </summary>
        [XmlIgnore]
        public Texture DiffuseTexture { get; private set; }

        /// <summary>
        /// 高度贴图;
        /// </summary>
        [XmlIgnore]
        public Texture HeightTexture { get; private set; }

        /// <summary>
        /// 混合贴图;
        /// </summary>
        [XmlIgnore]
        public Texture MixerTexture { get; private set; }

        /// <summary>
        /// 是否已经初始化完毕?
        /// </summary>
        public bool IsInitialized
        {
            get
            {
                return DiffuseTexture != null || HeightTexture != null || MixerTexture != null;
            }
        }

        public override string ToString()
        {
            string info = string.Concat(
                this.GetType().Name,
                "id:", ID,
                " ,name:", Name,
                " ,IsInitialized:", IsInitialized);
            return info;
        }


        #region 地貌资源初始化;

        /// <summary>
        /// 同步初始化设置到贴图;
        /// </summary>
        public void Initialize(AssetBundle assetBundle)
        {
            Texture diffuse = assetBundle.LoadAsset<Texture>(diffusePath);
            Texture height = assetBundle.LoadAsset<Texture>(heightPath);
            Texture mixer = assetBundle.LoadAsset<Texture>(mixerPath);

            this.DiffuseTexture = diffuse;
            this.HeightTexture = height;
            this.MixerTexture = mixer;
        }

        /// <summary>
        /// 异步初始化设置到贴图信息;
        /// </summary>
        public CustomYieldInstruction InitializeAsync(AssetBundle assetBundle)
        {
            var asyncRequest = new LandformInitializeRequest(this, assetBundle);
            return asyncRequest;
        }

        /// <summary>
        /// 异步读取需要的贴图;
        /// </summary>
        class LandformInitializeRequest : CustomYieldInstruction
        {
            Landform landform;

            AssetBundleRequest diffuseRequest;
            AssetBundleRequest heightRequest;
            AssetBundleRequest mixerRequest;

            public LandformInitializeRequest(Landform landform, AssetBundle assetBundle)
            {
                this.landform = landform;
                LoadTexture(assetBundle, landform);
            }

            public override bool keepWaiting
            {
                get
                {
                    return KeepWaiting();
                }
            }

            bool KeepWaiting()
            {
                if (!diffuseRequest.isDone || !heightRequest.isDone || !mixerRequest.isDone)
                {
                    return true;
                }
                else
                {
                    Texture diffuse = (Texture)diffuseRequest.asset;
                    Texture height = (Texture)heightRequest.asset;
                    Texture mixer = (Texture)mixerRequest.asset;

                    landform.DiffuseTexture = diffuse;
                    landform.HeightTexture = height;
                    landform.MixerTexture = mixer;

                    return false;
                }
            }

            void LoadTexture(AssetBundle assetBundle, Landform landformXml)
            {
                diffuseRequest = assetBundle.LoadAssetAsync<Texture>(landformXml.diffusePath);
                heightRequest = assetBundle.LoadAssetAsync<Texture>(landformXml.heightPath);
                mixerRequest = assetBundle.LoadAssetAsync<Texture>(landformXml.mixerPath);
            }

        }

        #endregion


    }

}
