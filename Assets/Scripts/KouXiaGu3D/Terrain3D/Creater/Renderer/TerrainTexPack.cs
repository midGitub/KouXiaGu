using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形所使用的贴图合集;
    /// </summary>
    public class TerrainTexPack
    {

        /// <summary>
        /// 地形法线图;
        /// </summary>
        public Texture2D normalMap { get; set; }

        /// <summary>
        /// 地形高度贴图;
        /// </summary>
        public Texture2D heightMap { get; set; }

        /// <summary>
        /// 地形地表贴图;
        /// </summary>
        public Texture2D diffuseMap { get; set; }

        /// <summary>
        /// 销毁所有贴图;
        /// </summary>
        public void Destroy()
        {
            if (normalMap != null ||
                heightMap != null ||
                diffuseMap != null)
            {
                GameObject.Destroy(normalMap);
                GameObject.Destroy(heightMap);
                GameObject.Destroy(diffuseMap);

                normalMap = null;
                heightMap = null;
                diffuseMap = null;
            }
        }

    }

}
