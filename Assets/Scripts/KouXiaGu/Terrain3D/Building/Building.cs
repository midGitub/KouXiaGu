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
        public IWorld World { get; private set; }
        public CubicHexCoord Position { get; private set; }
        public BuildingInfo Info { get; private set; }

        public float Angle
        {
            get { return transform.rotation.eulerAngles.y; }
            set { transform.rotation = Quaternion.Euler(0, value, 0); }
        }

        /// <summary>
        /// 初始化建筑;
        /// </summary>
        public virtual void Build(IWorld world, CubicHexCoord position, BuildingInfo info)
        {
            World = world;
            Position = position;
            Info = info;
            UpdateHeight();
        }

        /// <summary>
        /// 更新建筑高度;
        /// </summary>
        public virtual void UpdateHeight()
        {
            Vector3 position = transform.position;
            position.y = World.Components.Landform.GetHeight(transform.position);
            transform.position = position;
        }

        /// <summary>
        /// 当邻居发生变化时调用;
        /// </summary>
        public virtual void NeighborChanged(CubicHexCoord position)
        {
            Debug.Log(position + "NeighborChanged");
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
