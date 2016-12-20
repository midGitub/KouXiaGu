

//异步的实例化地貌信息;
#define INIT_LANDFORM_ASYNC

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地貌定义;
    /// </summary>
    public class Landform
    {

        #region 地貌管理(静态)

        /// <summary>
        /// 已经初始化完毕的地貌信息;
        /// </summary>
        static readonly Dictionary<int, Landform> initializedLandforms = new Dictionary<int, Landform>();

        /// <summary>
        /// 保存所有ID;
        /// </summary>
        static int[] landformID;

        public static IEnumerable<int> Identifications
        {
            get { return initializedLandforms.Keys; }
        }

        public static IEnumerable<Landform> Landforms
        {
            get { return initializedLandforms.Values; }
        }

        /// <summary>
        /// 所有有效ID;
        /// </summary>
        static int[] LandformID
        {
            get
            {
                if (landformID == null || landformID.Length != initializedLandforms.Count)
                    landformID = initializedLandforms.Keys.ToArray();
                return landformID;
            }
        }

        /// <summary>
        /// 已经初始化完毕的地貌数量;
        /// </summary>
        public static int Count
        {
            get { return initializedLandforms.Count; }
        }

        /// <summary>
        /// 获取到地貌信息;
        /// </summary>
        public static Landform GetLandform(int id)
        {
            return initializedLandforms[id];
        }

        /// <summary>
        /// 获取到一个随机的地形;
        /// </summary>
        public static Landform GetRandomLandform()
        {
            int randomID = Random.Range(0, LandformID.Length);
            int id = LandformID[randomID];
            return GetLandform(id);
        }

        public static string StateLog()
        {
            string log = "地貌合集初始化完毕:\n" + initializedLandforms.Values.ToEnumerableLog();
            return log;
        }

        #endregion


        #region 地貌序列化(静态)

        /// <summary>
        /// 地貌信息描述文件文件名;
        /// </summary>
        const string ConfigFileName = "LandformDefinition.xml";

        /// <summary>
        /// 地貌信息描述文件路径;
        /// </summary>
        static string ConfigFilePath
        {
            get { return ResourcePath.CombineConfiguration(ConfigFileName); }
        }

        static readonly XmlSerializer serializerArray = new XmlSerializer(typeof(Landform[]));

        /// <summary>
        /// 序列化到地貌描述文件;
        /// </summary>
        public static void Serialize(Landform[] landforms)
        {
            serializerArray.SerializeFile(ConfigFilePath, landforms);
        }

        /// <summary>
        /// 从地貌描述文件反序列化;
        /// </summary>
        public static Landform[] Deserialize()
        {
            Landform[] landforms = (Landform[])serializerArray.DeserializeFile(ConfigFilePath);
            return landforms;
        }

        #endregion


        #region 资源初始化(静态)

        /// <summary>
        /// 地貌贴图资源包名;
        /// </summary>
        const string TextureAssetBundleFileName = "terrain";

        /// <summary>
        /// 地貌贴图资源包文件路径;
        /// </summary>
        static string TextureAssetBundleFilePath
        {
            get { return ResourcePath.CombineAssetBundle(TextureAssetBundleFileName); }
        }

        /// <summary>
        /// 对资源进行初始化;
        /// </summary>
        public static IEnumerator Initialize()
        {
            Landform[] landforms = Deserialize();
            return Initialize(landforms);
        }

        /// <summary>
        /// 对这些资源进行初始化,并且加入到合集;
        /// </summary>
        public static IEnumerator Initialize(IEnumerable<Landform> landforms)
        {
            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(TextureAssetBundleFilePath);
            while (!bundleLoadRequest.isDone)
                yield return null;

            AssetBundle assetBundle = bundleLoadRequest.assetBundle;
            if (assetBundle == null)
            {
                Debug.LogError("目录不存在贴图资源包或者在编辑器中进行读取,地貌资源初始化失败;" + TextureAssetBundleFilePath);
                yield break;
            }

            foreach (var landform in landforms)
            {
                if (initializedLandforms.ContainsKey(landform.ID))
                {
                    Debug.LogWarning("相同的地貌ID,跳过 :" + landform.ToString());
                    continue;
                }

#if INIT_LANDFORM_ASYNC
                var customYieldInstruction = landform.LoadTexturesAsync(assetBundle);

                while (customYieldInstruction.keepWaiting)
                    yield return null;
#else
                landform.Initialize(assetBundle);
                yield return null;
#endif
                if (landform.IsInitialized)
                {
                    initializedLandforms.Add(landform.ID, landform);
                }
                else
                {
                    Debug.LogWarning("剔除未能初始化成功的地貌 : " + landform.ToString());
                }
            }

            assetBundle.Unload(false);
            Debug.Log(StateLog());
            yield break;
        }

        /// <summary>
        /// 对初始化的资源进行清除;
        /// </summary>
        public static IEnumerator ClearRes()
        {
            foreach (var item in initializedLandforms.Values)
            {
                item.Destroy();
            }
            initializedLandforms.Clear();
            yield break;
        }

        #endregion


        #region 实例部分;

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

        void Destroy()
        {
            GameObject.Destroy(DiffuseTexture);
            GameObject.Destroy(HeightTexture);
            GameObject.Destroy(MixerTexture);
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

        /// <summary>
        /// 同步初始化设置到贴图;
        /// </summary>
        public void LoadTextures(AssetBundle assetBundle)
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
        public CustomYieldInstruction LoadTexturesAsync(AssetBundle assetBundle)
        {
            var asyncRequest = new LoadTexturesRequest(this, assetBundle);
            return asyncRequest;
        }

        /// <summary>
        /// 异步读取需要的贴图;
        /// </summary>
        class LoadTexturesRequest : CustomYieldInstruction
        {
            Landform landform;

            AssetBundleRequest diffuseRequest;
            AssetBundleRequest heightRequest;
            AssetBundleRequest mixerRequest;

            public LoadTexturesRequest(Landform landform, AssetBundle assetBundle)
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
