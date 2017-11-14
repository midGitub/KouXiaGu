using JiongXiaGu.Unity.Resources;
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
            LoadOrder loadOrder = new LoadOrder();
            loadOrder.Order.AddLast(new LoadableContentInfo(new LoadableContentDescription("0", "Core"), LoadableContentType.Core));

            LoadOrder loadOrder2 = new LoadOrder(loadOrder);

            Debug.Log(loadOrder == loadOrder2);
        }
    }
}
