using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Aerobation2D
{

    /// <summary>
    /// 飞行器模型信息;
    /// </summary>
    [Serializable]
    public struct AerobatModelInfo
    {
        public Vector2 ActivityNortheast;
        public Vector2 ActivitySouthwest;
    }

    /// <summary>
    /// 飞行器信息;
    /// </summary>
    [Serializable]
    public struct AerobatInfo
    {
        public float HorizontalSpeed;
        public float VerticalSpeed;
    }
}
