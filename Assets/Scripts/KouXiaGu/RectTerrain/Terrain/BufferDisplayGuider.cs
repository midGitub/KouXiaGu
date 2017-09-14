using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KouXiaGu.Grids;

namespace KouXiaGu.RectTerrain
{

    /// <summary>
    /// 有缓冲区域的显示导航;
    /// </summary>
    [Serializable]
    public class BufferDisplayGuider : IDisplayGuider<RectCoord>
    {
        /// <summary>
        /// 重要显示区域;
        /// </summary>
        public RectRange DisplayRange;

        /// <summary>
        /// 缓冲区域;
        /// </summary>
        public RectRange SmoothRange;

        /// <summary>
        /// 显示中心点;
        /// </summary>
        RectCoord center;

        IEnumerable<RectCoord> IDisplayGuider<RectCoord>.GetPointsToDisplay()
        {
            throw new NotImplementedException();
        }
    }
}
