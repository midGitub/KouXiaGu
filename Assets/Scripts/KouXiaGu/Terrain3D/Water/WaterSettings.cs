using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Xml.Serialization;

namespace KouXiaGu.Terrain3D
{

    [Serializable]
    public class WaterSettings
    {
        [SerializeField]
        Material daytimeMaterial;
        [SerializeField]
        Material nighttimeMaterial;
        [SerializeField]
        WaterChunk prefabChunk;
        [SerializeField]
        WaterCustomizableSettings customizableSettings;

        public Material DaytimeMaterial
        {
            get { return daytimeMaterial; }
        }

        public Material NighttimeMaterial
        {
            get { return nighttimeMaterial; }
        }

    }

    /// <summary>
    /// 可自定义的设置;
    /// </summary>
    [Serializable]
    public class WaterCustomizableSettings
    {

    }

    class WaterSettingsSerializer
    {

    }
}
