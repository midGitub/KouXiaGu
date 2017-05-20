using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Xml.Serialization;
using UnityStandardAssets.Water;

namespace KouXiaGu.Terrain3D
{

    [Serializable]
    public class WaterSettings
    {
        public Material DaytimeMaterial;
        public Material NighttimeMaterial;
        public WaterChunk PrefabChunk;
        public WaterCustomizableSettings CustomizableSettings;
    }

    /// <summary>
    /// 可自定义的设置;
    /// </summary>
    [Serializable]
    public class WaterCustomizableSettings
    {
        public Water.WaterMode WaterMode;
    }

    class WaterSettingsSerializer
    {

    }
}
