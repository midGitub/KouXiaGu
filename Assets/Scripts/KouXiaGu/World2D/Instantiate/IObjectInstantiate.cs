using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 物体创建器接口;
    /// </summary>
    public interface IObjectInstantiate
    {

        UnityEngine.Object Instantiate(UnityEngine.Object original, Vector3 position, Quaternion rotation);
        UnityEngine.Object Instantiate(UnityEngine.Object original, Transform parent, bool worldPositionStays);
        UnityEngine.Object Instantiate(UnityEngine.Object original, Vector3 position, Quaternion rotation, Transform parent);
        T Instantiate<T>(T original) where T : UnityEngine.Object;

    }

}
