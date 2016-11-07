using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using XGame.Running;

namespace XGame.XmlModule
{

    /// <summary>
    /// 预制物体管理器;调用挂载在组件之下的 IXmlModule 接口;
    /// </summary>
    [DisallowMultipleComponent]
    public class PrefabCreater : MonoBehaviour, IModLoad
    {

        [Header("预制读取:")]

        /// <summary>
        /// 调用次序;
        /// </summary>
        [SerializeField]
        private CallOrder callOrder;

        /// <summary>
        /// 存放可创建的物体的XML文件;
        /// </summary>
        [SerializeField]
        private string prefabXmlPath;

        private Dictionary<string, IXmlModule> m_XmlModuleDictionary;

        /// <summary>
        /// 缓存单独预制物体已经读取过的组件名;
        /// 避免重复放置组件;
        /// </summary>
        private HashSet<string> m_ModuleNameSet;

        /// <summary>
        /// 预制物体存放;
        /// </summary>
        private PrefabContainer prefabContainer;

        CallOrder ICallOrder.CallOrder { get { return callOrder; } }
        private AssetController assetManager { get { return AssetController.GetInstance; } }

        protected void Awake()
        {
            prefabContainer = ControllerHelper.GameController.GetComponentInChildren<PrefabContainer>();
            IXmlModule[] xmlModules = GetComponentsInChildren<IXmlModule>();
            m_XmlModuleDictionary = xmlModules.ToDictionary(module => module.XElementName);
            m_ModuleNameSet = new HashSet<string>();
        }

        /// <summary>
        /// 获取到新实例化的物体名;
        /// </summary>
        /// <param name="name"></param>
        /// <param name="modInfo"></param>
        /// <returns></returns>
        private string GetXGameObjectName(string name, ModInfo modInfo)
        {
            return string.Concat(modInfo.Name, "_", name);
        }

        /// <summary>
        /// 通过预制合集获取到预制物体;
        /// </summary>
        /// <param name="combinationObject"></param>
        /// <param name="modInfo"></param>
        /// <returns></returns>
        private XGameObject GetXGameObject_Fast(XElement combinationObject, ModInfo modInfo)
        {
            XGameObject xGameObject;
            string strName;
            string strPrefabName;

            XAttribute Name = combinationObject.Attribute("Name");
            XAttribute PrefabName = combinationObject.Attribute("PrefabName");

            if (PrefabName != null && Name != null)
            {
                strName = (string)Name;
                strPrefabName = (string)PrefabName;
                if(prefabContainer.TryGetValue(strPrefabName, out xGameObject))
                {
                    xGameObject = Instantiate(xGameObject.gameObject).GetComponent<XGameObject>();
                    xGameObject.name = GetXGameObjectName(strName, modInfo);
                    return xGameObject;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new DataNotFoundException("未定义预制名; \n" + combinationObject);
            }
        }

        /// <summary>
        /// 通过资源包获取到预制物体;
        /// </summary>
        /// <param name="combinationObject"></param>
        /// <param name="modInfo"></param>
        /// <returns></returns>
        private XGameObject GetXGameObject_Asset(XElement combinationObject, ModInfo modInfo)
        {
            XGameObject xGameObject;
            GameObject prefabObject;
            string strName;
            string strModName;
            string strAssetBundleName;
            string strPrefabName;

            XAttribute Name = combinationObject.Attribute("Name");
            XAttribute ModName = combinationObject.Attribute("ModName");
            XAttribute AssetBundleName = combinationObject.Attribute("AssetBundleName");
            XAttribute PrefabName = combinationObject.Attribute("PrefabName");

            if (AssetBundleName == null || PrefabName == null || Name == null)
            {
                throw new DataNotFoundException("不完整的预制获取方式; \n" + combinationObject);
            }
            else
            {
                strName = (string)Name;
                strAssetBundleName = (string)AssetBundleName;
                strPrefabName = (string)PrefabName;
            }

            if (ModName != null)
            {
                strModName = (string)ModName;
                prefabObject = assetManager[strModName, strAssetBundleName].LoadAsset<GameObject>(strPrefabName);
            }
            else
            {
                prefabObject = assetManager[modInfo.Name, strAssetBundleName].LoadAsset<GameObject>(strPrefabName);
            }

            if(prefabObject == null)
                throw new DataNotFoundException("无法获取到预制物体;" + strPrefabName);

            prefabObject = Instantiate(prefabObject);
            xGameObject = prefabObject.GetComponent<XGameObject>();

            if (xGameObject == null)
            {
                GameObject.Destroy(xGameObject.gameObject);
                throw new DataNotFoundException("获取到的预制物体缺少 XGameObject 组件;" + strPrefabName);
            }

            xGameObject.name = GetXGameObjectName(strName, modInfo);
            return xGameObject;
        }

        /// <summary>
        /// 通过预制合集 或者 资源包 获取到预制物体;
        /// </summary>
        /// <param name="combinationObject"></param>
        /// <param name="modInfo"></param>
        /// <returns></returns>
        private XGameObject GetXGameObject(XElement combinationObject, ModInfo modInfo)
        {
            XGameObject xGameObject;

            xGameObject = GetXGameObject_Fast(combinationObject, modInfo);
            if (xGameObject == null)
                xGameObject = GetXGameObject_Asset(combinationObject, modInfo);

            return xGameObject;
        }

        /// <summary>
        /// 向实例化的物体添加定义的模块组件;
        /// </summary>
        /// <param name="combinationObject"></param>
        /// <param name="insObject"></param>
        /// <param name="modInfo"></param>
        private void LoadModule(XElement combinationObject, XGameObject insObject, ModInfo modInfo)
        {
            IXmlModule xmlModule;
            string strModuleName;
            IEnumerable<XElement> moduleElements = combinationObject.Elements();
            foreach (var moduleElement in moduleElements)
            {
                strModuleName = moduleElement.Name.LocalName;

                if (m_XmlModuleDictionary.TryGetValue(strModuleName, out xmlModule))
                {
                    //若允许重复放置 或者 为加入过;
                    if (!xmlModule.IsDisallowMultiple || !m_ModuleNameSet.Contains(strModuleName))
                    {
                        xmlModule.Add(moduleElement, insObject, modInfo);
                        m_ModuleNameSet.Add(strModuleName);
                    }
                    else
                    {
                        Debug.LogWarning("跳过重复组件;" +
                       "    模组名 :" + modInfo.Name +
                       "    组件名 :" + strModuleName);
                    }
                }
                else
                {
                    Debug.LogWarning("跳过未知组件;" + 
                        "   模组名 :" + modInfo.Name +
                        "   组件名 :" + strModuleName);
                }

            }
            m_ModuleNameSet.Clear();
        }

        /// <summary>
        /// 接口实现,实例化模组下的资源;
        /// </summary>
        /// <param name="modInfo"></param>
        /// <returns></returns>
        IEnumerator IModLoad.Load(ModInfo modInfo)
        {
            XGameObject xGameObject;
            string fullPath;

            if (!modInfo.PathCombine(prefabXmlPath, out fullPath))
            {
                yield break;
            }

            XElement combinationObjects = XElement.Load(fullPath);
            var Combinations = combinationObjects.Elements("CombinationObject");

            foreach (var combinationObject in Combinations)
            {
                xGameObject = null;
                try
                {
                    xGameObject = GetXGameObject(combinationObject, modInfo);
                    LoadModule(combinationObject, xGameObject, modInfo);
                    prefabContainer.Add(xGameObject);
                }
                catch (Exception e)
                {
                    Debug.LogWarning("预制物体创建失败!\n" +
                        "\n   模组名 :" + modInfo.Name +
                        "\n 错误原因 : " + e +
                        "\n  XML节点 :" + combinationObject);

                    if (xGameObject != null)
                        GameObject.Destroy(xGameObject.gameObject);
                }
                yield return AsyncHelper.WaitForFixedUpdate;
            }
            yield break;
        }

    }

}
