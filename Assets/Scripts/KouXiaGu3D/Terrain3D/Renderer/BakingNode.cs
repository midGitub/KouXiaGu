using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 进行烘焙时的最小单位,代表地图节点;
    /// </summary>
    public struct BakingNode
    {

        /// <summary>
        /// 地貌地图节点;
        /// </summary>
        TerrainNode mapNode;

        /// <summary>
        /// 地貌信息;
        /// </summary>
        Landform landform;

        /// <summary>
        /// 节点在地图上的坐标;
        /// </summary>
        public CubicHexCoord MapPosition { get; private set; }

        public CubicHexCoord MapCenter { get; private set; }

        /// <summary>
        /// 地形贴图旋转角度;
        /// </summary>
        public float RotationY
        {
            get { return mapNode.RotationAngle; }
        }

        public Texture DiffuseTex
        {
            get { return landform.DiffuseTexture; }
        }

        public Texture DiffuseBlendTex
        {
            get { return landform.MixerTexture; }
        }

        public Texture HeightTex
        {
            get { return landform.HeightTexture; }
        }

        public Texture HeightBlendTex
        {
            get { return landform.MixerTexture; }
        }

        public BakingNode(CubicHexCoord position, CubicHexCoord center, TerrainNode mapNode) : this()
        {
            this.MapPosition = position;
            this.MapCenter = center;
            this.mapNode = mapNode;
            this.landform = GetLandform(mapNode);
        }

        /// <summary>
        /// 根据地貌节点获取到地貌信息;
        /// </summary>
        Landform GetLandform(TerrainNode landformNode)
        {
            if (landformNode.ID == 0)
                return default(Landform);

            Landform landform;
            try
            {
                landform = Landform.GetLandform(landformNode.ID);
            }
            catch (KeyNotFoundException)
            {
                landform = Landform.GetRandomLandform();
                Debug.LogWarning("获取到不存在的地貌ID: " + landformNode.ID + ";随机替换为: " + landform.ID);
            }
            return landform;
        }

    }

}
