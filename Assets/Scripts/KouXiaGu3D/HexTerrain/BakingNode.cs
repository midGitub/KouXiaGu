using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 进行烘焙时的最小单位,代表地图节点;
    /// </summary>
    public struct BakingNode
    {

        public BakingNode(Vector3 position, float rotationY, Landform landform)
        {
            this.Position = position;
            this.RotationY = rotationY;
            this.landform = landform;
            NotBoundary = true;
        }

        /// <summary>
        /// 这个点不为边界(超出地图范围)?
        /// </summary>
        public bool NotBoundary { get; private set; }

        /// <summary>
        /// 节点的位置;
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// 贴图旋转角度;
        /// </summary>
        public float RotationY { get; set; }

        /// <summary>
        /// 地貌信息;
        /// </summary>
        Landform landform;

        /// <summary>
        /// 高度贴图;
        /// </summary>
        public Texture HeightTexture
        {
            get { return landform.HeightTexture; }
        }

    }

}
