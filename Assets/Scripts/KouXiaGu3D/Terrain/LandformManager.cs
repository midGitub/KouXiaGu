

//异步的实例化地貌信息;
#define INIT_LANDFORM_ASYNC


using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

namespace KouXiaGu.Terrain
{

    /// <summary>
    /// 负责提供地貌信息初始化和获取;
    /// </summary>
    public class LandformManager : UnitySingleton<LandformManager>
    {
        LandformManager() { }

        /// <summary>
        /// 已经初始化完毕的地貌信息;
        /// </summary>
        internal Dictionary<int, Landform> initializedLandforms;

        /// <summary>
        /// 返回地貌信息;
        /// </summary>
        public Landform this[int id]
        {
            get { return initializedLandforms[id]; }
        }

        /// <summary>
        /// 已经初始化完毕的地貌数量;
        /// </summary>
        public int Count
        {
            get { return initializedLandforms.Count; }
        }

        void Awake()
        {
            initializedLandforms = new Dictionary<int, Landform>();
        }

        #region 初始化地貌;

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
        /// 序列化定义文件的地貌信息;
        /// </summary>
        public IEnumerator Initialize()
        {
            Landform[] landforms = Deserialize();
            return Initialize(landforms);
        }

        /// <summary>
        /// 对这些资源进行初始化;
        /// </summary>
        public IEnumerator Initialize(IEnumerable<Landform> landforms)
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
                if (initializedLandforms.ContainsKey(landform.ID))
                {
                    Debug.LogWarning("相同的地貌ID,跳过 :" + landform.ToString());
                    continue;
                }

#if INIT_LANDFORM_ASYNC
                yield return landform.InitializeAsync(assetBundle);
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
            yield break;
        }

        #endregion


        #region 地貌序列化

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
            serializerArray.Serialize(ConfigFilePath, landforms);
        }

        /// <summary>
        /// 从地貌描述文件反序列化;
        /// </summary>
        public static Landform[] Deserialize()
        {
            Landform[] landforms = (Landform[])serializerArray.Deserialize(ConfigFilePath);
            return landforms;
        }

        #endregion

    }

}
