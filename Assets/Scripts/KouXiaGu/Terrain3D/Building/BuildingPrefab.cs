using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.World.Map;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 建筑物预制;
    /// </summary>
    [DisallowMultipleComponent]
    public class BuildingPrefab : MonoBehaviour, IBuildingPrefab
    {
        [SerializeField]
        Building prefab;

        /// <summary>
        /// 创建的预制物体;
        /// </summary>
        public Building Prefab
        {
            get { return prefab; }
            set { prefab = value; }
        }

        public Building BuildAt(CubicHexCoord coord, MapNode node, BuildingBuilder builder)
        {
            throw new NotImplementedException();
        }

        IBuilding IBuildingPrefab.BuildAt(CubicHexCoord coord, MapNode node, BuildingBuilder builder)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 建筑物实例;
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class Building : MonoBehaviour, IBuilding
    {
        CubicHexCoord coord;
        BuildingBuilder builder;

        public CubicHexCoord Coord
        {
            get { return coord; }

        }

        public abstract void Rebuild();

        public virtual void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
