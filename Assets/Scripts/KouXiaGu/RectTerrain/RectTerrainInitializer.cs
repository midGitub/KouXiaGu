using KouXiaGu.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.RectTerrain
{

    /// <summary>
    /// 地形资源;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class RectTerrainInitializer : MonoBehaviour
    {
        RectTerrainInitializer()
        {
        }

        /// <summary>
        /// Unity线程处置器;
        /// </summary>
        [SerializeField]
        RequestUnityDispatcher Dispatcher;
    }
}
