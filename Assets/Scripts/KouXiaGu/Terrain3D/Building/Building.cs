using KouXiaGu.Grids;
using KouXiaGu.World;
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
        //public BuildingInfo Info { get; private set; }
        public IWorld World { get; private set; }
        public CubicHexCoord Coord { get; private set; }

        /// <summary>
        /// 初始化建筑;
        /// </summary>
        public virtual void Build(IWorld world, CubicHexCoord position)
        {
            World = world;
            Coord = position;
            Rebuild();
        }

        /// <summary>
        /// 重新构建建筑(当地形发生变化时调用);
        /// </summary>
        public virtual void Rebuild()
        {
            Vector3 position = transform.position;
            position.y = World.Components.Landform.GetHeight(transform.position);
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
