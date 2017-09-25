using JiongXiaGu.Resources;
using UnityEngine;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 在程序一开始就存在的物体,保持该物体不随场景切换销毁;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class GlobalController : UnitySington<GlobalController>
    {   
        void Awake()
        {
            SetInstance(this);
            DontDestroyOnLoad(gameObject);

            XiaGu.Initialize();
            Resource.Initialize();
        }

        public static T GetSington<T>()
        {
            return Instance.GetComponentInChildren<T>();
        }

        [ContextMenu("Test")]
        void Test()
        {
        }
    }
}
