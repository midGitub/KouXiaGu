using KouXiaGu.Grids;
using KouXiaGu.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{
    /// <summary>
    /// 场景建筑物实例;
    /// </summary>
    public interface IBuilding
    {
        IWorld World { get; }
        CubicHexCoord Position { get; }
        float Angle { get; set; }
        BuildingInfo Info { get; }

        /// <summary>
        /// 更新建筑高度信息;
        /// </summary>
        void UpdateHeight();

        /// <summary>
        /// 当邻居节点内容发生变化时调用;
        /// </summary>
        void NeighborChanged(CubicHexCoord position);

        /// <summary>
        /// 销毁这个建筑物;
        /// </summary>
        void Destroy();
    }
}
