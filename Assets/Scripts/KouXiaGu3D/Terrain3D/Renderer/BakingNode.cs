using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 进行烘焙时的最小单位,代表地图节点;
    /// </summary>
    public struct BakingNode
    {

        public BakingNode(Vector3 position, TerrainNode mapNode) : this()
        {
            this.Position = position;
            this.mapNode = mapNode;
            this.landform = GetLandform(mapNode);
            NotBoundary = true;
        }

        /// <summary>
        /// 地貌地图节点;
        /// </summary>
        TerrainNode mapNode;

        /// <summary>
        /// 地貌信息;
        /// </summary>
        Landform landform;

        /// <summary>
        /// 这个点不为边界(超出地图范围)?
        /// </summary>
        public bool NotBoundary { get; private set; }

        /// <summary>
        /// 节点的位置;
        /// </summary>
        public Vector3 Position { get; private set; }

        /// <summary>
        /// 贴图旋转角度;
        /// </summary>
        public float RotationY
        {
            get { return mapNode.RotationAngle; }
        }

        public Texture DiffuseTexture
        {
            get { return landform.DiffuseTexture; }
        }

        public Texture HeightTexture
        {
            get { return landform.HeightTexture; }
        }

        public Texture MixerTexture
        {
            get { return landform.MixerTexture; }
        }


        /// <summary>
        /// 根据地貌节点获取到地貌信息;
        /// </summary>
        Landform GetLandform(TerrainNode landformNode)
        {
            if (landformNode.ID == 0)
                return default(Landform);

            Landform landform = Landform.GetLandform(landformNode.ID);
            return landform;
        }

    }

}
