using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Xml.Serialization;
using UnityStandardAssets.Water;
using UniRx;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 水特效设置内容;
    /// </summary>
    [Serializable]
    public struct WaterSettings
    {
        public Material DaytimeMaterial;
        public Material NighttimeMaterial;
        public WaterChunk PrefabChunk;
        /// <summary>
        /// 海平面高度;
        /// </summary>
        public float SeaLevel;
        public WaterCustomizableSettings CustomizableSettings;
    }

    /// <summary>
    /// 可自定义的设置;
    /// </summary>
    [Serializable]
    public struct WaterCustomizableSettings
    {
        public Water.WaterMode WaterMode;
    }

    class WaterSettingsSerializer
    {

    }
}
