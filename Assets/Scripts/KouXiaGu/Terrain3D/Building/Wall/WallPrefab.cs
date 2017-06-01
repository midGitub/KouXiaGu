using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World.Map;
using UnityEngine;
using KouXiaGu.Terrain3D.DynamicMesh;
using KouXiaGu.World;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 墙体预制;
    /// </summary>
    [DisallowMultipleComponent]
    public class WallPrefab : MonoBehaviour, IBuildingPrefab
    {
        WallPrefab()
        {
        }

        [SerializeField]
        DynamicMeshScript prefab = null;

        IReadOnlyDictionary<CubicHexCoord, MapNode> map
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 迭代获取到这个点通向周围的路径点,若不存在节点则不进行迭代;
        /// </summary>
        public IEnumerable<CubicHexCoord[]> GetPeripheralRoutes(CubicHexCoord target)
        {
            PeripheralRoute.TryGetPeripheralValue tryGetValue = delegate (CubicHexCoord position, out uint value)
            {
                MapNode node;
                if (map.TryGetValue(position, out node))
                {
                    if (node.Building.Exist())
                    {
                        value = node.Building.ID;
                        return true;
                    }
                }
                value = default(uint);
                return false;
            };
            return PeripheralRoute.GetRoadRoutes(target, tryGetValue);
        }

        public IBuilding BuildAt(IWorld world, CubicHexCoord position, float angele)
        {
            throw new NotImplementedException();
        }
    }
}
