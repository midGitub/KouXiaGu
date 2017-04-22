using System;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 对象池;
    /// </summary>
    [Serializable]
    public class ReusableObjectPool<T> : OGameObjectPool<T>
        where T : MonoBehaviour, IReusable
    {


        protected override void ReleaseAction(T item)
        {
            item.Reset();
            base.ReleaseAction(item);
        }

    }

}
