//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Xml.Linq;
//using UnityEngine;


//namespace XGame
//{

//    /// <summary>
//    /// 资源包控制;
//    /// </summary>
//    public class AssetController : Controller<AssetController>, IModLoad
//    {

//        /// <summary>
//        /// 调用次序;
//        /// </summary>
//        [SerializeField]
//        private CallOrder moduleType;

//        /// <summary>
//        /// 资源说明文件;
//        /// </summary>
//        [SerializeField]
//        private string assetInfoPath;

//        /// <summary>
//        /// 已经读取的资源包;
//        /// </summary>
//        private Dictionary<string, ModAsset> m_assetBundles;

//        protected override AssetController This
//        {
//            get { return this; }
//        }

//        CallOrder ICallOrder.CallOrder
//        {
//            get { return moduleType; }
//        }

//        public AssetBundle this[string modName, string assetName]
//        {
//            get { return m_assetBundles[modName].assetBundles[assetName]; }
//        }

//        protected override void Awake()
//        {
//            base.Awake();
//            m_assetBundles = new Dictionary<string, ModAsset>();
//        }

//        private void Start()
//        {
//            GetComponent<ModResLoader>().OnComplete.AddListener(UnLoadAll);
//        }

//        IEnumerator IModLoad.Load(ModInfo modInfo)
//        {
//            string assetInfoPath = Path.Combine(modInfo.ModPath, this.assetInfoPath);
//            XElement assetInfoElement = XElement.Load(assetInfoPath);
//            ModAsset modAsset = new ModAsset(modInfo.Name);
//            m_assetBundles.Add(modInfo.Name, modAsset);
//            return modAsset.Load(modInfo, assetInfoElement);
//        }

//        public bool TryGetValue(string modName, string assetName, out AssetBundle assetBundle)
//        {
//            ModAsset modAsset;
//            if (m_assetBundles.TryGetValue(modName, out modAsset))
//            {
//                if (modAsset.assetBundles.TryGetValue(assetName,out assetBundle))
//                {
//                    return true;
//                }
//            }
//            assetBundle = default(AssetBundle);
//            return false;
//        }

//        /// <summary>
//        /// 卸载所有资源包;
//        /// </summary>
//        public void UnLoadAll()
//        {
//            foreach (var item in m_assetBundles.Values)
//            {
//                item.UnLoadAll();
//            }
//        }

//        /// <summary>
//        /// Mod资源包;
//        /// </summary>
//        private sealed class ModAsset
//        {
//            public ModAsset(string modName)
//            {
//                this.ModName = modName;
//                this.assetBundles = new Dictionary<string, AssetBundle>();
//            }

//            public string ModName { get; private set; }

//            public Dictionary<string, AssetBundle> assetBundles { get; private set; }

            
//            /// <summary>
//            /// 根据Xml文件获取到资源包;
//            /// </summary>
//            /// <param name="modInfo"></param>
//            /// <param name="assetInfoElement"></param>
//            public IEnumerator Load(ModInfo modInfo, XElement assetInfoElement)
//            {
//                IEnumerable<XElement> assetBundleElements = assetInfoElement.Elements("AssetBundle");

//                foreach (var assetBundleElement in assetBundleElements)
//                {
//                    XAttribute name = assetBundleElement.Attribute("Name");
//                    XAttribute path = assetBundleElement.Attribute("Path");

//                    if (name != null && path != null)
//                    {
//                        string fullPath = Path.Combine(modInfo.ModPath, (string)path);
//                        AssetBundleCreateRequest assetBundle = AssetBundle.LoadFromFileAsync(fullPath);

//                        yield return new WaitUntil(() => assetBundle.isDone);

//                        if (assetBundle.assetBundle != null)
//                        {
//                            assetBundles.Add((string)name, assetBundle.assetBundle);
//                        }
//                    }
//                }
//            }

//            /// <summary>
//            /// 卸载所有资源包;
//            /// </summary>
//            /// <param name="unloadAllLoadedObjects">是否卸载已经使用的资源;</param>
//            public void UnLoadAll(bool unloadAllLoadedObjects = false)
//            {
//                foreach (var assetBundle in assetBundles.Values)
//                {
//                    assetBundle.Unload(unloadAllLoadedObjects);
//                }
//            }

//        }

//    }

//}
