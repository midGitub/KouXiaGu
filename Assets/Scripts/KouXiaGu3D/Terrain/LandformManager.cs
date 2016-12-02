

//异步的实例化地貌信息;
#define INIT_LANDFORM_ASYNC


using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;


namespace KouXiaGu.Terrain
{

    /// <summary>
    /// 获取到地貌信息和
    /// 负责提供地形贴图数据;
    /// </summary>
    public static class LandformManager
    {

        /// <summary>
        /// 地貌信息描述文件文件名;
        /// </summary>
        const string ConfigFileName = "LandformDefinition.xml";

        /// <summary>
        /// 地貌信息描述文件路径;
        /// </summary>
        public static readonly string ConfigFilePath = ResourcePath.CombineConfiguration(ConfigFileName);

        /// <summary>
        /// 地貌贴图资源包名;
        /// </summary>
        const string TextureAssetBundleFileName = "terrain";

        /// <summary>
        /// 地貌贴图资源包文件路径;
        /// </summary>
        public static readonly string TextureAssetBundleFilePath = ResourcePath.CombineAssetBundle(TextureAssetBundleFileName);


        /// <summary>
        /// 已经初始化完毕的地貌信息;
        /// </summary>
        static readonly Dictionary<int, Landform> initializedLandforms = new Dictionary<int, Landform>();

        /// <summary>
        /// 是否正在初始化中?
        /// </summary>
        public static bool IsInitializing { get; private set; }

        /// <summary>
        /// 已经初始化完毕的地貌信息;
        /// </summary>
        public static IEnumerable<Landform> InitializedLandforms
        {
            get { return initializedLandforms.Values; }
        }

        /// <summary>
        /// 已经实例化的地貌数量;
        /// </summary>
        public static int InitializedCount
        {
            get { return initializedLandforms.Count; }
        }

        static LandformManager()
        {
            IsInitializing = false;
        }

        /// <summary>
        /// 根据ID获取到地貌;
        /// </summary>
        public static Landform GetWithID(int id)
        {
            return initializedLandforms[id];
        }

        /// <summary>
        /// 根据ID移除地貌;
        /// </summary>
        public static bool Remove(int id)
        {
            return initializedLandforms.Remove(id);
        }

        /// <summary>
        /// 尝试获取到地貌信息;
        /// </summary>
        public static bool TryGetValue(int id, out Landform landform)
        {
            return initializedLandforms.TryGetValue(id, out landform);
        }


        /// <summary>
        /// 从定义的资源配置初始化地形信息;
        /// </summary>
        public static IEnumerator Initialize()
        {
            IEnumerable<Landform> landforms = Load();
            return Initialize(landforms);
        }

        /// <summary>
        /// 对这些资源进行初始化,并且加入到已经初始化字典;
        /// 若字典已经存在相同编号的地貌则不加入到;
        /// </summary>
        public static IEnumerator Initialize(IEnumerable<Landform> landforms)
        {
            if (IsInitializing)
            {
                Debug.LogWarning("尝试启动多个初始化协程;" + typeof(LandformManager).FullName);
                yield break;
            }
            IsInitializing = true;

            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(TextureAssetBundleFilePath);
            yield return bundleLoadRequest;

            AssetBundle assetBundle = bundleLoadRequest.assetBundle;
            if (assetBundle == null)
            {
                Debug.LogError("目录不存在贴图资源包,地貌资源初始化失败;" + TextureAssetBundleFilePath);
                yield break;
            }

            foreach (var landform in landforms)
            {
                if (InitializeOrNot(landform))
                {
#if INIT_LANDFORM_ASYNC
                    yield return InitializeAsync(landform, assetBundle);
#else
                Initialize(landform, assetBundle);
                yield return null;
#endif
                    TryAddInitialized(landform);
                }
                else
                {
                    Debug.LogWarning("相同的地貌ID,跳过此物体;" + landform.ToString());
                }
            }

            assetBundle.Unload(false);
            IsInitializing = false;
            yield break;
        }


        /// <summary>
        /// 是否对这个地貌信息进行初始化?返回true则初始化;
        /// </summary>
        static bool InitializeOrNot(Landform landform)
        {
            return !initializedLandforms.ContainsKey(landform.id) && !landform.IsInitialized;
        }

        /// <summary>
        /// 若初始化完毕则加入到已初始化合集内,返回true,否则返回false;
        /// </summary>
        static bool TryAddInitialized(Landform landform)
        {
            if (landform.IsInitialized)
            {
                initializedLandforms.Add(landform.id, landform);
                return true;
            }
            else
            {
                Debug.LogWarning("初始化失败;" + landform.ToString());
                return false;
            }
        }

        /// <summary>
        /// 同步初始化设置到贴图;
        /// </summary>
        static void Initialize(Landform landform, AssetBundle assetBundle)
        {
            Texture diffuse = assetBundle.LoadAsset<Texture>(landform.diffusePath);
            Texture height = assetBundle.LoadAsset<Texture>(landform.heightPath);
            Texture mixer = assetBundle.LoadAsset<Texture>(landform.mixerPath);

            landform.SetLandform(diffuse, height, mixer);
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

                    landform.SetLandform(diffuse, height, mixer);
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

        /// <summary>
        /// 将现有地貌定义输出到文件;
        /// </summary>
        public static void Save()
        {
            List<Landform> landforms = initializedLandforms.Values.ToList();
            Save(landforms);
        }

        /// <summary>
        /// 将地貌定义输出到文件;
        /// </summary>
        public static void Save(List<Landform> landforms)
        {
            Save(landforms, ConfigFilePath);
        }

        /// <summary>
        /// 将地貌定义输出到文件;
        /// </summary>
        public static void Save(List<Landform> landforms, string filePath)
        {
            SerializeHelper.SerializeXml(filePath, landforms);
        }

        /// <summary>
        /// 将此地貌结构附加到地貌定义文件中;
        /// </summary>
        public static void Append(IEnumerable<Landform> landforms)
        {
            var originalLandforms = Load();
            originalLandforms.AddRange(landforms);
            Save(originalLandforms);
        }

        /// <summary>
        /// 从地貌定义文件读取到地貌信息;
        /// </summary>
        public static List<Landform> Load()
        {
            return Load(ConfigFilePath);
        }

        /// <summary>
        /// 从文件读取到地貌信息;
        /// </summary>
        public static List<Landform> Load(string filePath)
        {
            List<Landform> landforms = SerializeHelper.DeserializeXml<List<Landform>>(filePath);
            return landforms;
        }


    }

}
