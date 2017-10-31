using UnityEngine;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 在程序一开始就存在的物体,保持该物体不随场景切换销毁;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class GlobalController :MonoBehaviour
    {   
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        [ContextMenu("Test")]
        void Test()
        {
            string str = "0123456789";
            Debug.Log(str.IndexOf("23"));
            Debug.Log(str.Substring(4));
        }
    }
}
