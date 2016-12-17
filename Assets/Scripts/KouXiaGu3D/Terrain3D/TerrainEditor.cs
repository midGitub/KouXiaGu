using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形编辑工具;
    /// </summary>
    public class TerrainEditor : MonoBehaviour
    {
        TerrainEditor() { }

        [SerializeField]
        public MapDescription description;


        public static TerrainMap CreateMap(MapDescription description)
        {
            var map = new TerrainMap(description);
            map.UpdateDescription();
            return map;
        }

    }

}
