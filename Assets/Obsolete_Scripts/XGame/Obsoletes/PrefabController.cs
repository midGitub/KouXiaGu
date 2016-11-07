//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Xml.Linq;
//using UnityEngine;
//using UnityEngine.Rendering;
//using XGame.GameRunning;

//namespace XGame
//{

//    /// <summary>
//    /// 初始化游戏中存在的预制物体;
//    /// </summary>
//    [DisallowMultipleComponent]
//    public class PrefabController : Controller<PrefabController>, IModLoad
//    {

//        [SerializeField]
//        private CallOrder moduleType;

//        /// <summary>
//        /// 存放可创建的物体的XML文件;
//        /// </summary>
//        [SerializeField]
//        private string xmlPath;

//        /// <summary>
//        /// 存放已经创建物体的目录;
//        /// </summary>
//        [SerializeField]
//        private Transform prefabCatalog;

//        /// <summary>
//        /// Preab获取;
//        /// </summary>
//        [SerializeField]
//        private PrefabAssemble assemble;

//        /// <summary>
//        /// 保存已经初始化的可创建物体;
//        /// </summary>
//        private Dictionary<string, XGameObjectPair> m_XGameObjectSet;

//        protected override PrefabController This
//        {
//            get { return this; }
//        }

//        CallOrder ICallOrder.CallOrder
//        {
//            get { return moduleType; }
//        }

//        /// <summary>
//        /// 默认的创建方法;
//        /// </summary>
//        public CreateInfo DefaultCreateInfo
//        {
//            get { return assemble.createInfoModule; }
//        }

//        /// <summary>
//        /// 保存已经初始化的可创建物体;
//        /// </summary>
//        public Dictionary<string, XGameObjectPair> XGameObjectSet
//        {
//            get { return m_XGameObjectSet; }
//        }

//        protected override void Awake()
//        {
//            base.Awake();
//            assemble.Awake();
//            prefabCatalog.gameObject.SetActive(false);
//            m_XGameObjectSet = new Dictionary<string, XGameObjectPair>();
//        }

//        /// <summary>
//        /// 实例化为游戏物体,并且一直保存在这个游戏状态中;
//        /// </summary>
//        /// <param name="prefab"></param>
//        /// <returns></returns>
//        public GameObject Instantiate(GameObject prefab)
//        {
//            GameObject gameObject = Instantiate(prefab, prefabCatalog) as GameObject;
//            return gameObject;
//        }

//        /// <summary>
//        /// 资源读取接口;
//        /// </summary>
//        /// <param name="modInfo">读取的目录;</param>
//        /// <returns></returns>
//        IEnumerator IModLoad.Load(ModInfo modInfo)
//        {
//            string fullPath = Path.Combine(modInfo.ModPath, xmlPath);

//            if (File.Exists(fullPath))
//            {
//                PrefabRes resInfo = new PrefabRes(modInfo);
//                XElement root = XElement.Load(fullPath);

//                IEnumerator enumerator = assemble.LoadPrefab(root, resInfo, 3);

//                while (enumerator.MoveNext())
//                    yield return enumerator.Current;
//            }
//            yield break;
//        }

//        /// <summary>
//        /// 获取到预览物体;若不存在则返回异常;
//        /// </summary>
//        /// <returns></returns>
//        public XGameObject GetPrefab(string objectName)
//        {
//            return m_XGameObjectSet[objectName].XGameObject;
//        }

//        /// <summary>
//        /// 获取到创建信息,若不存在则返回异常;
//        /// </summary>
//        /// <param name="objectName"></param>
//        /// <returns></returns>
//        public CreateInfo GetCreateInfo(string objectName)
//        {
//            return m_XGameObjectSet[objectName].CreateInfo;
//        }


//        [ContextMenu("Test")]
//        private void Test()
//        {
//            foreach (var item in m_XGameObjectSet)
//            {
//                Debug.Log(item.Key + "   " + item.Value.XGameObject.name);
//                Debug.Log(item.Value.CreateInfo.LimitLongestRow);
//            }
//        }

//    }

//    /// <summary>
//    /// 预制物体保存结构;
//    /// </summary>
//    public class XGameObjectPair
//    {
//        public XGameObject XGameObject;

//        public CreateInfo CreateInfo;
//    }


//    /// <summary>
//    /// 资源读取接口;
//    /// </summary>
//    public class PrefabRes
//    {

//        public PrefabRes(ModInfo modInfo)
//        {
//            this.ModInfo = modInfo;
//        }

//        /// <summary>
//        /// 正在读取的Mod
//        /// </summary>
//        public ModInfo ModInfo { get; private set; }

//        /// <summary>
//        /// 最后一个放入的XGameObject;
//        /// </summary>
//        public string XGameObjectName { get; private set; }

//        /// <summary>
//        /// 资源包组件;
//        /// </summary>
//        public static AssetController assetController{ get { return AssetController.GetInstance; }}

//        public static Dictionary<string, XGameObjectPair> XGameObjectSet { get { return PrefabController.GetInstance.XGameObjectSet; } }

//        /// <summary>
//        /// 加入到已经创建完成的预制;
//        /// </summary>
//        /// <param name="xGameObject"></param>
//        public void Add(XGameObject xGameObject)
//        {
//            var pair = new XGameObjectPair() { XGameObject = xGameObject };
//            string xGameObjectName = xGameObject.name;

//            XGameObjectSet.Add(xGameObjectName, pair);
//            this.XGameObjectName = xGameObjectName;
//        }

//        /// <summary>
//        /// 向这个创建物体加入一个创建方法;
//        /// </summary>
//        /// <param name="createInfo"></param>
//        public void Add(string xGameObjectName, CreateInfo createInfo)
//        {
//            XGameObjectSet[xGameObjectName].CreateInfo = createInfo;
//        }

//        /// <summary>
//        /// 根据现在读取资源的类型,获取到完整的目录;
//        /// </summary>
//        /// <param name="path">XML文件定义的本地目录</param>
//        /// <returns>完整路径信息;</returns>
//        public virtual string GetFullPath(string path)
//        {
//            return Path.Combine(ModInfo.ModPath, path);
//        }

//        /// <summary>
//        /// 根据现在读取资源的类型,获取到完整的物体名;
//        /// </summary>
//        /// <param name="name"></param>
//        /// <returns></returns>
//        public virtual string GetFullName(string name)
//        {
//            return ModInfo.Name + "_" + name;
//        }

//        /// <summary>
//        /// 从AssetBundle读取到资源;
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="assetBundleName"></param>
//        /// <param name="resName"></param>
//        /// <returns></returns>
//        public T Load<T>(string assetBundleName, string resName)
//            where T : UnityEngine.Object
//        {
//            return Load<T>(ModInfo.Name, assetBundleName, resName);
//        }

//        /// <summary>
//        /// 从AssetBundle读取到资源;
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="modName"></param>
//        /// <param name="assetBundleName"></param>
//        /// <param name="resName"></param>
//        /// <returns></returns>
//        public static T Load<T>(string modName, string assetBundleName, string resName)
//            where T : UnityEngine.Object
//        {
//            try
//            {
//                return assetController[modName, assetBundleName].LoadAsset<T>(resName);
//            }
//            catch (KeyNotFoundException e)
//            {
//                throw new KeyNotFoundException("读取失败或者未定义的 AssetBundle!" + assetBundleName + "\n", e);
//            }
//        }

//    }


//    /// <summary>
//    /// 组合Prefab;
//    /// </summary>
//    [Serializable]
//    public class PrefabAssemble
//    {

//        /// <summary>
//        /// 预制物体合集;
//        /// </summary>
//        [SerializeField]
//        private GameObject[] Prefab;


//        /// <summary>
//        /// 预制表;
//        /// </summary>
//        private Dictionary<string, GameObject> m_PrefabDictionary;

//        /// <summary>
//        /// 组合模块合集;
//        /// </summary>
//        private Dictionary<string, IXmlModule> m_AssembleModuleSet;

//        #region Module

//        //[SerializeField]
//        //public XmlCapitalModule capitalModule;

//        [SerializeField]
//        public XmlCreateInfoModule createInfoModule;

//        #endregion

//        /// <summary>
//        /// 获取到读取的组件;
//        /// </summary>
//        /// <returns></returns>
//        private Dictionary<string, IXmlModule> GetModule()
//        {
//            var assembleModuleSet = new Dictionary<string, IXmlModule>();
//            Action<IXmlModule> AddAssembleModuleSet = module => assembleModuleSet.Add(module.XElementName, module);

//            //AddAssembleModuleSet(capitalModule);
//            AddAssembleModuleSet(createInfoModule);

//            return assembleModuleSet;
//        }

//        /// <summary>
//        /// 初始化信息;
//        /// </summary>
//        public void Awake()
//        {
//            m_PrefabDictionary = Prefab.Where(item => item != null).ToDictionary(item => item.name);
//            m_AssembleModuleSet = GetModule();
//        }

//        /// <summary>
//        /// 初始化XML节点GameObjectInfo内的信息;
//        /// </summary>
//        /// <param name="gameObjectInfo"></param>
//        /// <param name="insObject"></param>
//        private void InitializeModule(XElement gameObjectInfo, XGameObject insObject, PrefabRes resources)
//        {
//            IXmlModule assembleModule;
//            HashSet<IXmlModule> AddModuleSet = new HashSet<IXmlModule>();     //已加入的组件合集剔除不允许重复加入部分;

//            foreach (var module in gameObjectInfo.Elements())
//            {
//                string moduleName = module.Name.LocalName;

//                try
//                {
//                    assembleModule = m_AssembleModuleSet[moduleName];
//                }
//                catch (KeyNotFoundException)
//                {
//                    Debug.LogWarning("未找到合适调用的组件!" + moduleName);
//                    continue;
//                }

//                //允许多个放置,或者未加入;
//                if (!assembleModule.IsDisallowMultiple || !AddModuleSet.Contains(assembleModule))
//                {
//                    assembleModule.Add(insObject, module, resources);
//                    AddModuleSet.Add(assembleModule);
//                }
//            }
//        }

//        /// <summary>
//        /// 根据XML文件异步读取并且创建预制物体;
//        /// </summary>
//        /// <param name="gameInfos"></param>
//        /// <param name="resources"></param>
//        /// <param name="pauses"></param>
//        /// <returns></returns>
//        public IEnumerator LoadPrefab(XElement gameInfos, PrefabRes resources, int pauses)
//        {
//            XGameObject xGameObject;
//            IEnumerable<XElement> gameInfo = gameInfos.Elements("CombinationObject");
//            int temp_i = 0;

//            foreach (var info in gameInfo)
//            {
//                temp_i++;
//                xGameObject = null;
//                //若在读取过程中出现错误,则跳过;
//                try
//                {
//                    xGameObject = GetPrefabObject(info, resources);
//                    resources.Add(xGameObject);
//                    InitializeModule(info, xGameObject, resources);
//                }
//                catch (Exception e)
//                {
//                    Debug.LogWarning("资源读取失败!   " + e.Message + "   " + e);
//                    if (xGameObject != null)
//                    {
//                        GameObject.Destroy(xGameObject.gameObject);
//                    }
//                    continue;
//                }

//                if (pauses < temp_i)
//                {
//                    yield return AsyncHelper.WaitForFixedUpdate;
//                    temp_i = 0;
//                }
//            }
//        }

//        /// <summary>
//        /// 根据XML节点获取到实例化后的预览物体;
//        /// </summary>
//        /// <param name="gameInfo"></param>
//        /// <param name="resources"></param>
//        /// <returns></returns>
//        public XGameObject GetPrefabObject(XElement gameInfo, PrefabRes resources)
//        {
//            XAttribute PrefabName = gameInfo.Attribute("PrefabName");
//            XAttribute AssetBundleName = gameInfo.Attribute("AssetBundleName");
//            XAttribute ModName = gameInfo.Attribute("ModName");
//            XAttribute Name = gameInfo.Attribute("Name");

//            GameObject prefab = GetPrefabObject(ModName, AssetBundleName, PrefabName, resources);

//            GameObject insObject = PrefabController.GetInstance.Instantiate(prefab);
//            XGameObject xGameObject = insObject.GetComponent<XGameObject>();

//            if (xGameObject == null)
//            {
//                throw new DataNotFoundException("实例化物体缺少XGameObject组件!");
//            }

//            xGameObject.name = resources.GetFullName((string)Name);
//            return xGameObject;
//        }

//        /// <summary>
//        /// 先从缓存中获取到预制物体,若不存在再从资源包内获取;
//        /// </summary>
//        /// <param name="assetBundleName"></param>
//        /// <param name="prefabName"></param>
//        /// <returns></returns>
//        private GameObject GetPrefabObject(
//            XAttribute modName, 
//            XAttribute assetBundleName,
//            XAttribute prefabName,
//            PrefabRes resources)
//        {
//            GameObject prefab;
//            string strPrefabName;
//            string strAssetBundleName;
//            string strResName;

//            if (prefabName == null || assetBundleName == null)
//            {
//                throw new DataNotFoundException("缺少必要数据,无法指定Prefab!");
//            }
//            else
//            {
//                strPrefabName = (string)prefabName;
//                strAssetBundleName = (string)assetBundleName;
//            }

//            strResName = assetBundleName + strPrefabName;

//            if (m_PrefabDictionary.TryGetValue(strResName, out prefab))
//            {
//                return prefab;
//            }
//            else
//            {
//                if (modName == null)
//                    prefab = resources.Load<GameObject>(strAssetBundleName, strPrefabName);
//                else
//                    prefab = PrefabRes.Load<GameObject>((string)modName, strAssetBundleName, strPrefabName);

//                if (prefab != null)
//                {
//                    m_PrefabDictionary.Add(strResName, prefab);
//                    return prefab;
//                }
//                else
//                {
//                    throw new DataNotFoundException("无法获取到Prefab;" + assetBundleName + "  " + strPrefabName);
//                }
//            }
//        }

//    }


//    /// <summary>
//    /// 从XML文件读取Unity组件部分信息抽象类\方法类,支持多线程;
//    /// </summary>
//    public abstract class AssembleXml
//    {

//        public static Sprite GetSprite(string assetBundleName, string textrueName, PrefabRes resources)
//        {
//            return resources.Load<Sprite>(assetBundleName, textrueName);
//        }

//        public static Sprite GetSprite(string path, PrefabRes resources)
//        {
//            path = resources.GetFullPath(path);
//            return DirectoryHelper.LoadSprite(path);
//        }

//        public static Sprite GetSprite(string path, PrefabRes resources, Vector2 pivot)
//        {
//            path = resources.GetFullPath(path);
//            return DirectoryHelper.LoadSprite(path, pivot);
//        }

//        public static Texture GetTexture(string assetBundleName, string textrueName, PrefabRes resources)
//        {
//            return resources.Load<Texture>(assetBundleName, textrueName);
//        }

//        public static Texture GetTexture(string path, PrefabRes resources)
//        {
//            path = resources.GetFullPath(path);
//            return DirectoryHelper.LoadTexture(path);
//        }

//        /// <summary>
//        /// 获取到Sprite类型的贴图;
//        /// </summary>
//        /// <param name="spriteInfo"></param>
//        /// <param name="resources"></param>
//        /// <returns></returns>
//        public static Sprite GetSprite(XElement spriteInfo, PrefabRes resources)
//        {
//            if (spriteInfo != null)
//            {
//                Sprite sprite = GetResource<Sprite>(spriteInfo, resources, GetSprite, GetSprite);
//                return sprite;
//            }
//            return null;
//        }

//        /// <summary>
//        /// 获取到Texture类型的贴图;
//        /// </summary>
//        /// <param name="textureInfo"></param>
//        /// <param name="resources"></param>
//        /// <returns></returns>
//        public static Texture GetTexture(XElement textureInfo, PrefabRes resources)
//        {
//            return GetResource<Texture>(textureInfo, resources, GetTexture, GetTexture);
//        }

//        /// <summary>
//        /// 从XML获取到对应资源;
//        /// </summary>
//        /// <typeparam name="T">获取到的资源类型;</typeparam>
//        /// <param name="info">获取到信息的XML节点;</param>
//        /// <param name="resources">资源接口</param>
//        /// <param name="assetBundleFunc">从 AssetBundle 获取到此资源的方法;</param>
//        /// <param name="loadFileFunc">从 File 获取到此资源的方法;</param>
//        /// <returns></returns>
//        public static T GetResource<T>(
//            XElement info,
//            PrefabRes resources,
//            Func<string, string, PrefabRes, T> assetBundleFunc,
//            Func<string, PrefabRes, T> loadFileFunc)
//        {
//            //if (info == null)
//            //    throw new NullReferenceException("尝试读取空的节点信息;");

//            T item;
//            XAttribute loadForm = info.Attribute("From");
//            if ("AssetBundle" == (string)loadForm)
//            {
//                item = AssetBundle<T>(info, resources, assetBundleFunc);
//            }
//            else if ("File" == (string)loadForm)
//            {
//                item = LoadFile<T>(info, resources, loadFileFunc);
//            }
//            else
//            {
//                Debug.Log("未指定读取方式或者错误的读取方式!" + (string)loadForm);
//                item = default(T);
//            }
//            return item;
//        }

//        /// <summary>
//        /// 从XML元素特性获取到AssetBundle内的资源;
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="assetBundle">存在 AssetBundle 特性的节点;</param>
//        /// <param name="resources"></param>
//        /// <param name="func">定义从AssetBundle获取的方法,获取到的元素;</param>
//        /// <returns></returns>
//        public static T AssetBundle<T>(XElement assetBundle, PrefabRes resources, Func<string, string, PrefabRes, T> func)
//        {
//            if (assetBundle != null)
//            {
//                XAttribute AssetBundleName = assetBundle.Attribute("AssetBundleName");
//                XAttribute TextrueName = assetBundle.Attribute("TextrueName");
//                if (AssetBundleName == null || TextrueName == null)
//                {
//                    Debug.LogWarning("缺少必要的参数,无法读取或创建;");
//                    return default(T);
//                }
//                else
//                {
//                    return func((string)AssetBundleName, (string)TextrueName, resources);
//                }
//            }
//            Debug.Log("传入空的XML节点!");
//            return default(T);
//        }

//        /// <summary>
//        /// 从XML元素特性获取到文件资源;
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="file">存在 LoadFile 特性的节点;</param>
//        /// <param name="resources"></param>
//        /// <param name="func">定义从文件获取的方法和获取到的文件;</param>
//        /// <returns></returns>
//        public static T LoadFile<T>(XElement file, PrefabRes resources, Func<string, PrefabRes, T> func)
//        {
//            if (file != null)
//            {
//                XAttribute Path = file.Attribute("Path");
//                if (Path == null)
//                {
//                    Debug.LogWarning("缺少必要的参数,无法读取或创建;");
//                    return default(T);
//                }
//                else
//                {
//                    return func((string)Path, resources);
//                }
//            }
//            Debug.Log("传入空的XML节点!");
//            return default(T);
//        }

//        /// <summary>
//        /// 根据XML节点子元素信息对 SpriteRenderer 进行变更;
//        /// </summary>
//        /// <param name="spriteInfo"></param>
//        /// <param name="spriteRenderer"></param>
//        /// <param name="defaultSpriteRenderer"></param>
//        public static void Load(XElement spriteInfo, ref SpriteRenderer spriteRenderer, SpriteRenderer defaultSpriteRenderer)
//        {
//            XElement sortingLayer = spriteInfo.Element("SortingLayer");
//            if (sortingLayer != null)
//            {
//                XAttribute sortingLayerName = sortingLayer.Attribute("Name");
//                XAttribute sortingOrder = sortingLayer.Attribute("Order");

//                spriteRenderer.sortingLayerName = sortingLayerName != null ?
//                    (string)sortingLayerName :
//                    defaultSpriteRenderer.sortingLayerName;
//                spriteRenderer.sortingOrder = sortingOrder != null ?
//                    (int)sortingOrder :
//                    defaultSpriteRenderer.sortingOrder;
//            }

//            XElement shadow = spriteInfo.Element("Shadow");
//            if (shadow != null)
//            {
//                XAttribute shadowCastingMode = shadow.Attribute("ShadowCastingMode");
//                XAttribute IsReceiveShadows = shadow.Attribute("IsReceiveShadows");

//                try
//                {
//                    spriteRenderer.shadowCastingMode = shadowCastingMode != null ?
//                        (ShadowCastingMode)Enum.Parse(typeof(ShadowCastingMode), (string)shadowCastingMode, true) :
//                        defaultSpriteRenderer.shadowCastingMode;
//                }
//                //若转换枚举时出错
//                catch (ArgumentException)
//                {
//                    Debug.LogWarning("ShadowCastingMode 特性无法获取,检查XML文件是否正确!" + (string)shadowCastingMode);
//                    spriteRenderer.shadowCastingMode = defaultSpriteRenderer.shadowCastingMode;
//                }

//                spriteRenderer.receiveShadows = IsReceiveShadows != null ?
//                    (bool)IsReceiveShadows :
//                    defaultSpriteRenderer.receiveShadows;
//            }

//            return;
//        }

//        /// <summary>
//        /// 根据XML节点子元素信息对 Transform 进行变更;
//        /// </summary>
//        /// <param name="transformInfo"></param>
//        /// <param name="transform"></param>
//        public static void Load(XElement transformInfo, ref Transform transform, Transform defaultTransform)
//        {
//            XElement Scale = transformInfo.Element("Scale");
//            if (Scale != null)
//            {
//                XAttribute x = Scale.Attribute("x");
//                XAttribute y = Scale.Attribute("y");
//                XAttribute z = Scale.Attribute("z");

//                transform.transform.localScale = new Vector3(
//                    x != null ? (float)x : defaultTransform.localScale.x,
//                    y != null ? (float)y : defaultTransform.localScale.y,
//                    z != null ? (float)z : defaultTransform.localScale.z
//                    );
//            }
//            return;
//        }


//        public static T LoadEnum<T>(XAttribute xAttribute, T defaultValue)
//            where T : struct
//        {
//            return xAttribute == null ?
//                defaultValue :
//                (T)Enum.Parse(typeof(T), (string)xAttribute);
//        }

//        public static int Load(XAttribute xAttribute, int defaultValue)
//        {
//            return xAttribute == null ? defaultValue : (int)xAttribute;
//        }

//        public static bool Load(XAttribute xAttribute, bool defaultValue)
//        {
//            return xAttribute == null ? defaultValue : (bool)xAttribute;
//        }

//        public static byte Load(XAttribute xAttribute, byte defaultValue)
//        {
//            return xAttribute == null ? defaultValue : Convert.ToByte((string)xAttribute);
//        }

//        public static IntVector2 Load(XAttribute xAttributeX, XAttribute xAttributeY, IntVector2 intVector2)
//        {
//            intVector2.x = xAttributeX == null ? intVector2.x : (short)xAttributeX;
//            intVector2.y = xAttributeY == null ? intVector2.y : (short)xAttributeY;
//            return intVector2;
//        }

//    }


//    /// <summary>
//    /// 组合模块接口;
//    /// </summary>
//    public interface IXmlModule
//    {
//        /// <summary>
//        /// 节点名;
//        /// </summary>
//        string XElementName { get; }

//        /// <summary>
//        /// 是否不允许多重放置?
//        /// </summary>
//        bool IsDisallowMultiple { get; }

//        /// <summary>
//        /// 向物体添加对应信息;
//        /// </summary>
//        /// <param name="insObject"></param>
//        /// <param name="module"></param>
//        void Add(XGameObject insObject, XElement module, PrefabRes resources);
//    }


//    /// <summary>
//    /// 向物体添加贴图;
//    /// </summary>
//    [Serializable]
//    public class AssembleSprite : IXmlModule
//    {
//        /// <summary>
//        /// 贴图预制模块;
//        /// </summary>
//        [SerializeField]
//        private SpriteRenderer module;

//        /// <summary>
//        /// XElement名;
//        /// </summary>
//        private const string m_XElementName = "Sprites";

//        private const bool m_IsDisallowMultiple = false;

//        /// <summary>
//        /// XElement名;
//        /// </summary>
//        public string XElementName
//        {
//            get { return m_XElementName; }
//        }

//        /// <summary>
//        /// 是否不允许多重放置?
//        /// </summary>
//        public bool IsDisallowMultiple
//        {
//            get { return m_IsDisallowMultiple; }
//        }

//        /// <summary>
//        /// 向物体添加贴图;
//        /// </summary>
//        /// <param name="insObject"></param>
//        /// <param name="sprites"></param>
//        public void Add(XGameObject insObject, XElement sprites, PrefabRes resources)
//        {
//            Sprite sprite;
//            Transform transform;
//            SpriteRenderer spriteRenderer;
//            foreach (var spriteInfo in sprites.Elements("Sprite"))
//            {
//                sprite = AssembleXml.GetSprite(spriteInfo, resources);

//                if (sprite == null)
//                {
//                    Debug.Log("缺少贴图,跳过!");
//                    continue;
//                }

//                transform = Instantiate(insObject, module.gameObject).transform;
//                spriteRenderer = transform.GetComponent<SpriteRenderer>();
//                spriteRenderer.sprite = sprite;

//                AssembleXml.Load(spriteInfo, ref transform, module.transform);
//                AssembleXml.Load(spriteInfo, ref spriteRenderer, module);
//            }
//        }

//        /// <summary>
//        /// 实例化物体;为物体添加上贴图;
//        /// </summary>
//        /// <param name="insObject"></param>
//        /// <param name="gameObject"></param>
//        private static GameObject Instantiate(XGameObject insObject, GameObject gameObject)
//        {
//            return (GameObject)GameObject.Instantiate(gameObject, insObject.transform);
//        }

//    }


//    /// <summary>
//    /// 向物体添加阴影;
//    /// </summary>
//    [Serializable]
//    public class AssembleShadow : IXmlModule
//    {

//        /// <summary>
//        /// 阴影预制模块;
//        /// </summary>
//        [SerializeField]
//        private MeshRenderer module;

//        /// <summary>
//        /// XElement名;
//        /// </summary>
//        private const string m_XElementName = "ShadowTextrues";

//        private const bool m_IsDisallowMultiple = false;

//        /// <summary>
//        /// XElement名;
//        /// </summary>
//        public string XElementName
//        {
//            get { return m_XElementName; }
//        }

//        /// <summary>
//        /// 是否不允许多重放置?
//        /// </summary>
//        public bool IsDisallowMultiple
//        {
//            get { return m_IsDisallowMultiple; }
//        }

//        /// <summary>
//        /// 向物体阴影贴图;
//        /// </summary>
//        /// <param name="insObject"></param>
//        /// <param name="shadowTextrues"></param>
//        public void Add(XGameObject insObject, XElement shadowTextrues, PrefabRes resources)
//        {
//            Texture texture;
//            Transform tranform = module.transform;
//            foreach (var textrueInfo in shadowTextrues.Elements("Textrue"))
//            {
//                texture = AssembleXml.GetTexture(textrueInfo, resources);

//                if (texture == null)
//                {
//                    Debug.Log("缺少贴图,跳过!");
//                    continue;
//                }

//                module.material.mainTexture = texture;

//                AssembleXml.Load(textrueInfo, ref tranform, module.transform);
//                Instantiate(insObject, module);
//            }

//        }

//        /// <summary>
//        /// 实例化物体;为物体添加上贴图;
//        /// </summary>
//        /// <param name="insObject"></param>
//        /// <param name="spriteRenderer"></param>
//        private static void Instantiate(XGameObject insObject, MeshRenderer spriteRenderer)
//        {
//            GameObject.Instantiate(spriteRenderer.gameObject, insObject.transform);
//        }

//    }


//    ///// <summary>
//    ///// 墙读取;
//    ///// </summary>
//    //[Serializable]
//    //public class AssembleWall : IAssembleModule
//    //{
//    //    /// <summary>
//    //    /// XElement名;
//    //    /// </summary>
//    //    private const string m_XElementName = "GameWall";

//    //    private const bool m_IsDisallowMultiple = true;

//    //    /// <summary>
//    //    /// XElement名;
//    //    /// </summary>
//    //    public string XElementName
//    //    {
//    //        get { return m_XElementName; }
//    //    }

//    //    /// <summary>
//    //    /// 是否不允许多重放置?
//    //    /// </summary>
//    //    public bool IsDisallowMultiple
//    //    {
//    //        get { return m_IsDisallowMultiple; }
//    //    }

//    //    public void Add(PrefabScript insObject, XElement module, PrefabRes resources)
//    //    {
//    //        Load(module, insObject, resources);
//    //    }

//    //    public void Load(XElement gameWallElement, PrefabScript insObject, PrefabRes resources)
//    //    {
//    //        Wall gameWall;

//    //        XElement SpriteLeftElement = gameWallElement.Element("SpriteLeft");
//    //        XElement SpriteRightElement = gameWallElement.Element("SpriteRight");

//    //        Sprite SpriteLeft = AssembleXml.GetSprite(SpriteLeftElement, resources);
//    //        Sprite SpriteRight = AssembleXml.GetSprite(SpriteRightElement, resources);

//    //        gameWall = insObject.GetComponent<Wall>();
//    //        if (gameWall == null)
//    //        {
//    //            gameWall = insObject.gameObject.AddComponent<Wall>();
//    //        }

//    //        gameWall.Wall_Left = SpriteLeft;
//    //        gameWall.Wall_Right = SpriteRight;
//    //    }

//    //}

//}
