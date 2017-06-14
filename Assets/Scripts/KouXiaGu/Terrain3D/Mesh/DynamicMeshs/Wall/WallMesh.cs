using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World;
using UnityEngine;
using KouXiaGu.World.Map;

namespace KouXiaGu.Terrain3D.DynamicMeshs
{

    /// <summary>
    /// 墙体建筑;
    /// </summary>
    [RequireComponent(typeof(DynamicMeshScript))]
    public class WallMesh : Building, IBuilding
    {
        DynamicMeshScript dynamicMesh;
        Vector3[] path;

        IReadOnlyDictionary<CubicHexCoord, MapNode> map
        {
            get { return World.WorldData.MapData.ReadOnlyMap; }
        }

        void Awake()
        {
            dynamicMesh = GetComponent<DynamicMeshScript>();
        }

        public override void Build(IWorld world, CubicHexCoord position, float angle, BuildingInfo info)
        {
            World = world;
            Position = position;
            this.angle = angle;
            Info = info;

            Vector3 pos = transform.position;
            pos.y = LandformSettings.Instance.WaterSettings.SeaLevel + 0.3f;
            transform.position = pos;

            BuildWall();
        }

        public override void UpdateHeight()
        {
            return;
        }

        public override void NeighborChanged(CubicHexCoord position)
        {
            base.NeighborChanged(position);
            BuildWall();
        }

        void BuildWall()
        {
            Vector3[] newPath = GetWallRoute(map, position, Info.ID);
            if (newPath != null && !path.IsSameContent(newPath))
            {
                BuildWall(newPath);
            }
        }

        void BuildWall(Vector3[] path)
        {
            ISpline spline = new CatmullRomSpline(path);
            dynamicMesh.Transformation(spline, path[0], path[path.Length - 1]);
            this.path = path;
        }

        /// <summary>
        /// 获取 from 到 to 的角度(忽略Y),单位弧度;原型:Mathf.Atan2();
        /// </summary>
        float AngleY(Vector3 from, Vector3 to)
        {
            return Mathf.Atan2((to.x - from.x), (to.z - from.z));
        }

        /// <summary>
        /// 获取到墙体路径,5个路径点,若为末端墙体,则为4个路径点;
        /// </summary>
        public static Vector3[] GetWallRoute(IReadOnlyDictionary<CubicHexCoord, MapNode> map, CubicHexCoord target, int buildingType)
        {
            TryGetPeripheralValue tryGetValue = delegate (CubicHexCoord position, out uint value)
            {
                MapNode node;
                if (map.TryGetValue(position, out node))
                {
                    BuildingNode buildingNode = node.Building;
                    if (buildingNode.Exist(buildingType))
                    {
                        value = buildingNode.ID;
                        return true;
                    }
                }
                value = default(uint);
                return false;
            };
            return GetWallRoute(target, tryGetValue);
        }

        /// <summary>
        /// 获取到墙体路径,5个路径点,若为末端墙体,则为4个路径点,若不存在路径则返回Null;
        /// </summary>
        public static Vector3[] GetWallRoute(CubicHexCoord target, TryGetPeripheralValue tryGetValue)
        {
            IList<CoordPack<CubicHexCoord, HexDirections>> sortList = PeripheralRoute.SortNeighbours(target, tryGetValue, UintAscendingComparer.Default).Values;

            if (sortList.Count >= 2)
            {
                CoordPack<CubicHexCoord, HexDirections> min = sortList[0];
                CoordPack<CubicHexCoord, HexDirections> max = sortList[1];
                Vector3 targetPixel = target.GetTerrainPixel();
                Vector3[] path = new Vector3[5];
                path[0] = min.Point.GetTerrainPixel();
                path[1] = GetEdgeMidpoint(targetPixel, min.Direction);
                path[2] = targetPixel;
                path[3] = GetEdgeMidpoint(targetPixel, max.Direction);
                path[4] = max.Point.GetTerrainPixel();
                return ConvertToLocalPath(targetPixel, path);
            }
            else if (sortList.Count == 1)
            {
                CoordPack<CubicHexCoord, HexDirections> min = sortList[0];
                Vector3 targetPixel = target.GetTerrainPixel();
                Vector3[] path = new Vector3[5];
                path[0] = min.Point.GetTerrainPixel();
                path[1] = GetEdgeMidpoint(targetPixel, min.Direction);
                path[2] = targetPixel;
                HexDirections opposite = CubicHexCoord.GetOppositeDirection(min.Direction);
                path[3] = GetEdgeMidpoint(targetPixel, opposite);
                path[4] = target.GetDirection(opposite).GetTerrainPixel();
                return ConvertToLocalPath(targetPixel, path);
            }
            else
            {
                return null;
            }
        }

        static readonly Dictionary<int, Vector3> edgeMidpointOffsets = new Dictionary<int, Vector3>()
        {
            { (int)HexDirections.North, CubicHexCoord.DIR_North.GetTerrainPixel() / 2 },
            { (int)HexDirections.Northeast, CubicHexCoord.DIR_Northeast.GetTerrainPixel() / 2 },
            { (int)HexDirections.Southeast, CubicHexCoord.DIR_Southeast.GetTerrainPixel() / 2 },
            { (int)HexDirections.South, CubicHexCoord.DIR_South.GetTerrainPixel() / 2 },
            { (int)HexDirections.Southwest, CubicHexCoord.DIR_Southwest.GetTerrainPixel() / 2 },
            { (int)HexDirections.Northwest, CubicHexCoord.DIR_Northwest.GetTerrainPixel() / 2 },
        };

        /// <summary>
        /// 获取到指定方向边的中点;
        /// </summary>
        public static Vector3 GetEdgeMidpoint(Vector3 target, HexDirections direction)
        {
            Vector3 offset = edgeMidpointOffsets[(int)direction];
            return target + offset;
        }

        /// <summary>
        /// 转换为本地路径;
        /// </summary>
        public static Vector3[] ConvertToLocalPath(Vector3 target, Vector3[] path)
        {
            for (int i = 0; i < path.Length; i++)
            {
                path[i] -= target;
            }
            return path;
        }
    }
}
