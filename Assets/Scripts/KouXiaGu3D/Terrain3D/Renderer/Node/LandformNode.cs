using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形进行烘焙时的最小单位;
    /// </summary>
    public class LandformNode
    {

        /// <summary>
        /// 节点在地图上的坐标;
        /// </summary>
        public CubicHexCoord Position { get; private set; }

        /// <summary>
        /// 地貌资源;
        /// </summary>
        public LandformRes Landform { get; private set; }

        /// <summary>
        /// 地形贴图旋转角度;
        /// </summary>
        public float LandformAngle { get; private set; }

        /// <summary>
        ///  初始化该节点建地貌信息;
        /// </summary>
        public LandformNode(IDictionary<CubicHexCoord, TerrainNode> map, CubicHexCoord coord)
        {
            var node = map[coord];

            if (node.ExistLandform)
            {
                this.Landform = GetLandform(node);
                this.LandformAngle = node.LandformAngle;
            }
            else
            {
                this.Landform = default(LandformRes);
                this.LandformAngle = default(float);
            }

            this.Position = coord;
        }

        /// <summary>
        /// 根据地貌节点获取到地貌信息;
        /// </summary>
        LandformRes GetLandform(TerrainNode landformNode)
        {
            LandformRes landform;
            try
            {
                landform = LandformRes.initializedInstances[landformNode.Landform];
                return landform;
            }
            catch (KeyNotFoundException)
            {
                Debug.LogWarning("获取到不存在的地貌ID: " + landformNode.Landform + ";");
                return default(LandformRes);
            }
        }

    }

}
