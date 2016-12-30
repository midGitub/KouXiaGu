using System.Collections.Generic;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{


    /// <summary>
    /// 道路节点;分别检测点周围是否存在道路,若存在道路则标记;
    /// </summary>
    public class BuildingNode
    {

        /// <summary>
        /// 道路使用的贴图;
        /// </summary>
        public RoadRes Road { get; private set; }

        /// <summary>
        /// 存在道路的节点方向;
        /// </summary>
        public IEnumerable<float> RoadAngles { get; private set; }

        /// <summary>
        /// 建筑物使用的贴图;
        /// </summary>
        public BuildingRes BuildingRes { get; private set; }

        /// <summary>
        /// 建筑物旋转的方向;
        /// </summary>
        public float BuildingAngle { get; private set; }

        /// <summary>
        /// 存在道路?
        /// </summary>
        public bool ExistRoad
        {
            get { return Road != null; }
        }

        /// <summary>
        /// 存在建筑物?
        /// </summary>
        public bool ExistBuild
        {
            get { return BuildingRes != null; }
        }

        /// <summary>
        /// 创建该节点的道路信息,若无法创建则返回异常;
        /// </summary>
        public BuildingNode(IDictionary<CubicHexCoord, TerrainNode> map, CubicHexCoord coord)
        {
            InitRoad(map, coord);
            InitBuilding(map, coord);
        }

        /// <summary>
        /// 初始化道路信息;
        /// </summary>
        void InitRoad(IDictionary<CubicHexCoord, TerrainNode> map, CubicHexCoord coord)
        {
            var node = map[coord];

            if (node.ExistRoad)
            {
                this.Road = GetRoadRes(node.Road);
                this.RoadAngles = GetRoadAngles(map, coord);
            }
            else
            {
                this.Road = null;
                this.RoadAngles = null;
            }
        }

        /// <summary>
        /// 获取到道路资源信息;
        /// </summary>
        RoadRes GetRoadRes(int id)
        {
            return RoadRes.initializedInstances[id];
        }

        /// <summary>
        /// 获取到这个点周围存在道路的方向角度;
        /// </summary>
        List<float> GetRoadAngles(IDictionary<CubicHexCoord, TerrainNode> map, CubicHexCoord coord)
        {
            List<float> angles = new List<float>();
            TerrainNode node;
            foreach (var dir in CubicHexCoord.Directions)
            {
                CubicHexCoord dirCoord = coord.GetDirection(dir);
                if (map.TryGetValue(dirCoord, out node))
                {
                    if (node.ExistRoad)
                    {
                        float angle = GridConvert.GetAngle(dir);
                        angles.Add(angle);
                    }
                }
            }
            return angles;
        }

        /// <summary>
        /// 初始化建筑信息;
        /// </summary>
        void InitBuilding(IDictionary<CubicHexCoord, TerrainNode> map, CubicHexCoord coord)
        {
            var node = map[coord];

            if (node.ExistBuild)
            {

            }
            else
            {
                this.BuildingRes = null;
                this.BuildingAngle = default(float);
            }
        }

    }

}
