using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D.Map
{

    /// <summary>
    /// 节点状态;
    /// </summary>
    public struct MapNodeState<T>
    {
        public MapNodeState(ChangeType eventType, ShortVector2 mapPoint, T worldNode)
        {
            this.EventType = eventType;
            this.MapPoint = mapPoint;
            this.WorldNode = worldNode;
        }

        /// <summary>
        /// 变化类型;
        /// </summary>
        public ChangeType EventType { get; private set; }

        /// <summary>
        /// 变化点的坐标;
        /// </summary>
        public ShortVector2 MapPoint { get; private set; }

        /// <summary>
        /// 变化后的点信息;
        /// </summary>
        public T WorldNode { get; private set; }
    }

}
