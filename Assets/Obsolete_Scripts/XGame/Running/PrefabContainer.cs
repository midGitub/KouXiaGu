using System.Collections.Generic;
using UnityEngine;

namespace XGame.Running
{

    /// <summary>
    /// 预制保存,预制物体合集,游戏中使用的预制物体合集;
    /// </summary>
    [DisallowMultipleComponent]
    public class PrefabContainer : MonoBehaviour
    {

        /// <summary>
        /// 存放已经创建物体的目录;
        /// </summary>
        [SerializeField]
        protected Transform prefabCatalog;

        /// <summary>
        /// 提供游戏使用的预制合集;
        /// </summary>
        private Dictionary<string, XGameObject> m_XGameObjectDictionary;

        public IEnumerable<string> PrefabNames { get { return m_XGameObjectDictionary.Keys; } }

        /// <summary>
        /// 获取到对应预制物体;
        /// </summary>
        /// <param name="prefabName"></param>
        /// <returns></returns>
        public XGameObject this[string prefabName]
        {
            get { return m_XGameObjectDictionary[prefabName]; }
        }

        protected virtual void Awake()
        {
            m_XGameObjectDictionary = new Dictionary<string, XGameObject>();
            prefabCatalog.gameObject.SetActive(false);
            AddFromPrefabCatalog();
        }

        /// <summary>
        /// 将这个物体加入到预制物体合集(加入后不允许销毁!);
        /// 若已存在相同名称的物体,则返回异常;
        /// </summary>
        /// <param name="prefabObject"></param>
        /// <returns></returns>
        public void Add(XGameObject prefabObject)
        {
            m_XGameObjectDictionary.Add(prefabObject.name, prefabObject);
            prefabObject.transform.SetParent(prefabCatalog);
        }

        public bool TryGetValue(string prefabName, out XGameObject xGameObject)
        {
            return m_XGameObjectDictionary.TryGetValue(prefabName, out xGameObject);
        }

        /// <summary>
        /// 从预制物体下面获取到预制物体;
        /// </summary>
        private void AddFromPrefabCatalog()
        {
            XGameObject xGameObject;
            foreach (Transform child in prefabCatalog)
            {
                xGameObject = child.GetComponent<XGameObject>();
                if (xGameObject != null)
                {
                    xGameObject.name = child.name;
                    m_XGameObjectDictionary.Add(xGameObject.name, xGameObject);
                }
                else
                {
#if UNITY_EDITOR
                    Debug.LogWarning("存在无效的游戏预制,根物体不存在XGameObject组件;" + child.name);
#endif
                    Destroy(child.gameObject);
                }
            }
        }


#if UNITY_EDITOR

        [ContextMenu("信息")]
        protected void Test_Log()
        {
            string str = "PrefabManager :  \n";

            str += "预制物体个数 :" + m_XGameObjectDictionary.Count + "\n";

            str += "分别为 : \n";
            foreach (var pair in m_XGameObjectDictionary)
            {
                str += pair.Key + "    " + pair.Value.name + " ; \n";
            }

            Debug.Log(str);
        }

#endif

    }

}
