using System.Collections.Generic;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{


    public class BuildingNode
    {

        public CubicHexCoord Position { get; private set; }

        /// <summary>
        /// 建筑物使用的贴图;
        /// </summary>
        public BuildingRes BuildingRes { get; private set; }

        /// <summary>
        /// 建筑物旋转的方向;
        /// </summary>
        public float BuildingAngle { get; private set; }

        /// <summary>
        /// 存在建筑物?
        /// </summary>
        public bool ExistBuild
        {
            get { return BuildingRes != null; }
        }

        /// <summary>
        /// 初始化该节点建筑物信息;
        /// </summary>
        public BuildingNode(IDictionary<CubicHexCoord, TerrainNode> map, CubicHexCoord coord)
        {
            var node = map[coord];

            if (node.ExistBuild)
            {

            }
            else
            {
                throw new ObjectNotExistedException("该节点不存在建筑物;");
            }

            this.Position = coord;
        }

    }

}
