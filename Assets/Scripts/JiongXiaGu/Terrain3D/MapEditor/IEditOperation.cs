using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiongXiaGu.VoidableOperations;
using JiongXiaGu.Grids;

namespace JiongXiaGu.Terrain3D.MapEditor
{

    /// <summary>
    /// 对地图进行操作;
    /// </summary>
    public interface IEditOperation
    {
        /// <summary>
        /// 鼠标指向的位置更新,用于预览;
        /// </summary>
        void OnPositionUpdate(CubicHexCoord position);

        /// <summary>
        /// 执行操作;
        /// </summary>
        VoidableOperation Perform(CubicHexCoord position);
    }
}
