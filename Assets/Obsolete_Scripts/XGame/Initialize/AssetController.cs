using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

namespace XGame
{

    /// <summary>
    /// 资源文件管理;
    /// 在模组资源初始化阶段允许使用内部资源,在模组资源读取完毕后卸载读取的所有资源;
    /// </summary>
    [DisallowMultipleComponent]
    public class AssetController : Controller<AssetController>, IModLoad
    {

        /// <summary>
        /// 调用次序;
        /// </summary>
        [SerializeField]
        private CallOrder callOrder;

        /// <summary>
        /// 资源说明文件;
        /// </summary>
        [SerializeField]
        private string assetInfoPath;

        private Dictionary<string, AssetBundle> m_AssetBundleDictionary;

        protected override AssetController This { get { return this; } }
        CallOrder ICallOrder.CallOrder { get { return callOrder; } }

        protected override void Awake()
        {
            base.Awake();
            m_AssetBundleDictionary = new Dictionary<string, AssetBundle>();
            GetComponentInParent<ModResLoader>().OnComplete.AddListener(UnLoadAll);     //读取完毕卸载所有资源包;
        }

        public AssetBundle this[string modName, string assetBundleName]
        {
            get
            {
                string key = TransfromKey(modName, assetBundleName);
                return m_AssetBundleDictionary[key];
            }
        }

        public bool TryGetValue(string modName, string assetBundleName, out AssetBundle assetBundle)
        {
            string key = TransfromKey(modName, assetBundleName);
            return m_AssetBundleDictionary.TryGetValue(key, out assetBundle);
        }

        /// <summary>
        /// 转换保存在m_AssetBundleDictionary内的值格式;
        /// </summary>
        /// <param name="modName"></param>
        /// <param name="assetBundleName"></param>
        /// <returns></returns>
        private static string TransfromKey(string modName, string assetBundleName)
        {
            return string.Concat(modName, "_", assetBundleName);
        }

        /// <summary>
        /// 卸载所有资源包;
        /// </summary>
        /// <param name="unloadAllLoadedObjects"></param>
        private void UnLoadAll()
        {
            bool unloadAllLoadedObjects = false;
            foreach (var assetBundle in m_AssetBundleDictionary.Values)
            {
                assetBundle.Unload(unloadAllLoadedObjects);
            }
            m_AssetBundleDictionary.Clear();
        }

        /// <summary>
        /// 根据Xml文件获取到资源;
        /// </summary>
        /// <param name="modInfo"></param>
        /// <param name="assetInfo"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        private static IEnumerator LoadFromXml(ModInfo modInfo, XElement assetInfo, IDictionary<string, AssetBundle> dictionary)
        {
            XAttribute name;
            XAttribute path;
            string assetBundleName;
            string assetBundleFullPath;
            AssetBundle assetBundle;
            AssetBundleCreateRequest assetBundleCreateRequest;

            IEnumerable<XElement> assetBundleElements = assetInfo.Elements("AssetBundle");

            foreach (var assetBundleElement in assetBundleElements)
            {
                name = assetBundleElement.Attribute("Name");
                path = assetBundleElement.Attribute("Path");

                if (name != null || path != null)
                {
                    assetBundleName = TransfromKey(modInfo.Name, (string)name);
                    assetBundleFullPath = Path.Combine(modInfo.ModPath, (string)path);

                    assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(assetBundleFullPath);
                    yield return new WaitUntil(() => assetBundleCreateRequest.isDone);

                    assetBundle = assetBundleCreateRequest.assetBundle;
                    if (assetBundle != null)
                    {
                        dictionary.Add(assetBundleName, assetBundle);
                    }
                }
            }
            yield break;
        }

        /// <summary>
        /// 根据模组读取资源;
        /// </summary>
        /// <param name="modInfo"></param>
        /// <returns></returns>
        IEnumerator IModLoad.Load(ModInfo modInfo)
        {
            string assetInfoPath = Path.Combine(modInfo.ModPath, this.assetInfoPath);

            if (!File.Exists(assetInfoPath))
            {
#if UNITY_EDITOR
                Debug.LogWarning(modInfo.Name + "不存在资源文件,跳过;");
#endif
                yield break;
            }

            XElement assetInfoElement = XElement.Load(assetInfoPath);
            IEnumerator enumerator = LoadFromXml(modInfo, assetInfoElement, m_AssetBundleDictionary);

            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }


#if UNITY_EDITOR

        [ContextMenu("信息;")]
        private void Test_Log()
        {
            string log = "AssetManager : \n";
            log += "已读取的资源包个数 :" + m_AssetBundleDictionary.Count + "\n";

            log += "分别有: \n";
            foreach (var item in m_AssetBundleDictionary)
            {
                log += item.Key + "\n";
            }

            Debug.Log(log);
        }

#endif


    }

}
