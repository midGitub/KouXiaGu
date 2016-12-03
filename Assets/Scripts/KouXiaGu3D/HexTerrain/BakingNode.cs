using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.HexTerrain
{


    /// <summary>
    /// 需要烘焙的地貌节点;
    /// </summary>
    public struct BakingNode
    {

        Landform landform;
        TerrainData terrainData;

        BakingNode[] bakingNeighbors;

        /// <summary>
        /// 在场景中的位置;
        /// </summary>
        public ShortVector2 MapPoint { get; private set; }

        public Vector3 Position { get; private set; }

        /// <summary>
        /// 漫反射贴图;
        /// </summary>
        public Texture DiffuseTexture
        {
            get { return landform.DiffuseTexture; }
        }

        /// <summary>
        /// 高度贴图;
        /// </summary>
        public Texture HeightTexture
        {
            get { return landform.HeightTexture; }
        }

        /// <summary>
        /// 混合贴图;
        /// </summary>
        public Texture MixerTexture
        {
            get { return landform.MixerTexture; }
        }


        public BakingNode(Landform landform, TerrainData terrainData, ShortVector2 mapPoint)
        {
            this.landform = landform;
            this.terrainData = terrainData;
            this.MapPoint = mapPoint;
            this.Position = HexGrids.OffsetToPixel(mapPoint);

            this.bakingNeighbors = null;
        }

        /// <summary>
        /// 邻居节点;
        /// </summary>
        public IEnumerable<BakingNode> Neighbors()
        {
            throw new NotImplementedException();
        }

    }

}
