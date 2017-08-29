using KouXiaGu.Grids;
using KouXiaGu.World;
using KouXiaGu.World.Map;
using KouXiaGu.World.Resources;
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
    public class BuildingUnit : MonoBehaviour, IBuilding
    {
        protected float angle;
        protected CubicHexCoord position;
        public IWorld World { get; protected set; }
        public BuildingInfo Info { get; protected set; }

        public CubicHexCoord Position
        {
            get { return position; }
            protected set { position = value; transform.position = value.GetTerrainPixel(); }
        }

        public float Angle
        {
            get { return angle; }
            set { angle = value; transform.rotation = Quaternion.Euler(0, value, 0); }
        }

        /// <summary>
        /// 初始化建筑;
        /// </summary>
        public virtual void Build(IWorld world, CubicHexCoord position, float angle, BuildingInfo info)
        {
            World = world;
            Position = position;
            Angle = angle;
            Info = info;
            UpdateHeight();
        }

        /// <summary>
        /// 更新建筑高度;
        /// </summary>
        public virtual void UpdateHeight()
        {
            //Vector3 position = transform.position;
            //position = World.Components.Landform.Normalize(position);
            //transform.position = position;
        }

        /// <summary>
        /// 当邻居发生变化时调用;
        /// </summary>
        public virtual void NeighborChanged(CubicHexCoord position)
        {
            Debug.Log(Position + " Neighbor:" + position + "had Changed;");
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
