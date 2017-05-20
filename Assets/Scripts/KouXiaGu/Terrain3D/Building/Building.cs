using KouXiaGu.Grids;
using KouXiaGu.World.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{
    /// <summary>
    /// 建筑物实例;
    /// </summary>
    [DisallowMultipleComponent]
    public class Building : MonoBehaviour, IBuilding
    {
        CubicHexCoord coord;
        BuildingBuilder builder;

        public CubicHexCoord Coord
        {
            get { return coord; }
        }

        public BuildingBuilder Builder
        {
            get { return builder; }
        }

        protected Landform landform
        {
            get { return builder.Landform; }
        }

        /// <summary>
        /// 初始化建筑;
        /// </summary>
        public virtual void Build(CubicHexCoord coord, MapNode node, BuildingBuilder builder)
        {
            this.coord = coord;
            this.builder = builder;
            Rebuild();
        }

        /// <summary>
        /// 重新构建建筑(当地形发生变化时调用);
        /// </summary>
        public virtual void Rebuild()
        {
            Vector3 position = transform.position;
            position.y = landform.GetHeight(transform.position);
            transform.position = position;
        }

        /// <summary>
        /// 销毁这个建筑物;
        /// </summary>
        public virtual void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
