using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 游戏地形控制;负责地形的更新和创建;
    /// </summary>
    [DisallowMultipleComponent]
    public class LandformController : SceneSington<LandformController>
    {

        /// <summary>
        /// unity线程执行;
        /// </summary>
        [SerializeField]
        LandformUnityDispatcher landformUnityDispatcher;

        void Awake()
        {
            SetInstance(this);
        }
    }
}
