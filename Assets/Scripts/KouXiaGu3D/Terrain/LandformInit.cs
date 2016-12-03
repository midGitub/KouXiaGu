

//异步的实例化地貌信息;
#define INIT_LANDFORM_ASYNC


using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;


namespace KouXiaGu.Terrain
{

    /// <summary>
    /// 负责提供地貌的贴图数据;
    /// </summary>
    public class LandformInit
    {

        #region 初始化完毕的地貌信息;

        /// <summary>
        /// 已经初始化完毕的地貌信息;
        /// </summary>
        readonly Dictionary<int, Landform> initializedLandforms = new Dictionary<int, Landform>();

        /// <summary>
        /// 已经初始化完毕的地貌信息;
        /// </summary>
        public IEnumerable<ILandform> InitializedLandforms
        {
            get { return initializedLandforms.Values.OfType<ILandform>(); }
        }

        /// <summary>
        /// 已经实例化的地貌数量;
        /// </summary>
        public int InitializedCount
        {
            get { return initializedLandforms.Count; }
        }

        /// <summary>
        /// 根据ID获取到地貌;
        /// </summary>
        public ILandform GetWithID(int id)
        {
            return initializedLandforms[id];
        }

        /// <summary>
        /// 根据ID移除地貌;
        /// </summary>
        public bool Remove(int id)
        {
            return initializedLandforms.Remove(id);
        }

        /// <summary>
        /// 尝试获取到地貌信息;
        /// </summary>
        public bool TryGetValue(int id, out ILandform landform)
        {
            Landform Landform;
            bool isFind = initializedLandforms.TryGetValue(id, out Landform);
            landform = Landform;
            return isFind;
        }

        /// <summary>
        /// 从定义的资源配置初始化地形信息;
        /// </summary>
        public IEnumerator Initialize()
        {
            IEnumerable<Landform> landforms = Landform.Load().Where(item => !initializedLandforms.ContainsKey(item.ID));
            var landformsArray = landforms.ToDictionary(item => item.ID);
            initializedLandforms.AddOrReplace(landformsArray);

            return Initialize(initializedLandforms.Values);
        }

        #endregion


        #region 地貌资源初始化;

        /// <summary>
        /// 地貌贴图资源包名;
        /// </summary>
        const string TextureAssetBundleFileName = "terrain";

        /// <summary>
        /// 地貌贴图资源包文件路径;
        /// </summary>
        public static readonly string TextureAssetBundleFilePath = ResourcePath.CombineAssetBundle(TextureAssetBundleFileName);

        /// <summary>
        /// 对这些资源进行初始化,并且加入到已经初始化字典;
        /// 若字典已经存在相同编号的地貌则不加入到;
        /// </summary>
        public static IEnumerator Initialize(IEnumerable<Landform> landforms)
        {
            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(TextureAssetBundleFilePath);
            yield return bundleLoadRequest;

            AssetBundle assetBundle = bundleLoadRequest.assetBundle;
            if (assetBundle == null)
            {
                Debug.LogError("目录不存在贴图资源包或者在编辑器中进行读取,地貌资源初始化失败;" + TextureAssetBundleFilePath);
                yield break;
            }

            foreach (var landform in landforms)
            {
#if INIT_LANDFORM_ASYNC
                yield return InitializeAsync(landform, assetBundle);
#else
                Initialize(landform, assetBundle);
                yield return null;
#endif
            }

            assetBundle.Unload(false);
            yield break;
        }

        /// <summary>
        /// 同步初始化设置到贴图;
        /// </summary>
        static void Initialize(Landform landform, AssetBundle assetBundle)
        {
            Texture diffuse = assetBundle.LoadAsset<Texture>(landform.diffusePath);
            Texture height = assetBundle.LoadAsset<Texture>(landform.heightPath);
            Texture mixer = assetBundle.LoadAsset<Texture>(landform.mixerPath);

            landform.DiffuseTexture = diffuse;
            landform.HeightTexture = height;
            landform.MixerTexture = mixer;
        }

        /// <summary>
        /// 异步初始化设置到贴图信息;
        /// </summary>
        static CustomYieldInstruction InitializeAsync(Landform landform, AssetBundle assetBundle)
        {
            var asyncRequest = new LandformInitializeRequest(landform, assetBundle);
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
