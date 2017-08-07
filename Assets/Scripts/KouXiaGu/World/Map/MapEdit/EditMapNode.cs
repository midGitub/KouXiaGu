using KouXiaGu.Grids;
using KouXiaGu.World.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map.MapEdit
{

    /// <summary>
    /// 编辑的地图节点数据;
    /// </summary>
    public class EditMapNode
    {
        public EditMapNode(CubicHexCoord position, MapNode value)
        {
            Position = position;
            Original = value;
            Value = value;
        }

        public CubicHexCoord Position { get; private set; }
        public MapNode Original { get; private set; }
        public MapNode Value;
    }
}
