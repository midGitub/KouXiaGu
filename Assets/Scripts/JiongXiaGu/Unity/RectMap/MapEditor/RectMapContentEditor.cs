using JiongXiaGu.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.World.RectMap.MapEditor
{


    /// <summary>
    /// 地形编辑器类,仅在UnityEditor下有效;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class RectMapContentEditor : MonoBehaviour, IWorldCompletedHandle
    {
        RectMapContentEditor()
        {
        }

        public bool IsCompleted { get; private set; }
        public WorldMap Map { get; private set; }

        void IWorldCompletedHandle.OnWorldCompleted()
        {
            Map = RectMapDataInitializer.Instance.WorldMap;
            IsCompleted = true;
        }
    }
}
