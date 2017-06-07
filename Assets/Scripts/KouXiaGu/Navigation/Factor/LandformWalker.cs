using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World.Map;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.Navigation
{

    /// <summary>
    /// UnwalkableTagsMask = 湖(地形), 海(地形), 河(地形), 城区(建筑);
    /// WalkableTagsMask = 渡船线路(建筑),
    /// 允许行走到存在"渡船线路"的不可行走地形;
    /// 允许行走到 WalkableTagsMask 定义的 和 UnwalkableTagsMask 未定义的 位置;
    /// 不允许行走到 UnwalkableTagsMask 定义的 且 WalkableTagsMask 未定义的 位置;
    /// </summary>
    public class LandformWalker : IWalkableConfirmer<CubicHexCoord>
    {
        public LandformWalker(IReadOnlyDictionary<CubicHexCoord, MapNode> map, TerrainResources resources)
        {
            Map = map;
            Resources = resources;
        }

        public IReadOnlyDictionary<CubicHexCoord, MapNode> Map { get; set; }
        public TerrainResources Resources { get; set; }

        /// <summary>
        /// 不可行走到的标签,只要存在这些标签则不允许行走到,除非存在 WalkableTagsMask 定义的其它标签;
        /// </summary>
        public int UnwalkableTagsMask { get; set; }

        /// <summary>
        /// 可行走到的标签,只要存在这些标签就允许行走到;
        /// </summary>
        public int WalkableTagsMask { get; set; }

        /// <summary>
        /// 设置可行走的标签;
        /// </summary>
        public int SetWalkableTags(string tags)
        {
            WalkableTagsMask = Resources.Tags.TagsToMask(tags);
            return WalkableTagsMask;
        }

        public int SetUnwalkableTags(string tags)
        {
            UnwalkableTagsMask = Resources.Tags.TagsToMask(tags);
            return UnwalkableTagsMask;
        }

        /// <summary>
        /// 获取到该位置的地形标签;
        /// </summary>
        public int GetTagsMask(CubicHexCoord position)
        {
            MapNode node;
            if (Map.TryGetValue(position, out node))
            {
                return GetTagsMask(node);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取到该节点的地形标签;
        /// </summary>
        public int GetTagsMask(MapNode node)
        {
            int mask = 0;
            if (node.Landform.Exist())
            {
                LandformInfo landformInfo;
                if (Resources.Landform.TryGetValue(node.Landform.LandformType, out landformInfo))
                {
                    mask |= landformInfo.TagsMask;
                }
            }
            if (node.Building.Exist())
            {
                BuildingInfo buildingInfo;
                if (Resources.Building.TryGetValue(node.Building.BuildingType, out buildingInfo))
                {
                    mask |= buildingInfo.TagsMask;
                }
            }
            return mask;
        }

        /// <summary>
        /// 获取到该点是否允许行走到;
        /// </summary>
        public bool IsWalkable(CubicHexCoord position)
        {
            MapNode node;
            if (Map.TryGetValue(position, out node))
            {
                return IsWalkable(node);
            }
            else
            {
                return false;
            }
        }

        public bool IsWalkable(MapNode node)
        {
            int landformTagsMask = GetTagsMask(node);
            if ((landformTagsMask & WalkableTagsMask) > 0)
            {
                return true;
            }
            else if ((landformTagsMask & UnwalkableTagsMask) > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
