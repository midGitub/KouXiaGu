using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 地图表面景观预制创建器,负责读取地图并且实例化物体到地图表面;
    /// </summary>
    [Serializable]
    public class WorldSurfaceCreater
    {
        public void _WorldSurface(IObjectInstantiate instantiate, IReadOnlyMap<Vector2, IReadOnlyWorldNode> readMap)
        {
            this.instantiate = instantiate;
            this.readMap = readMap;
        }

        /// <summary>
        /// 实例化接口;
        /// </summary>
        private IObjectInstantiate instantiate { get; set; }
        /// <summary>
        /// 根据这个地图创建物体;
        /// </summary>
        private IReadOnlyMap<Vector2, IReadOnlyWorldNode> readMap { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public void AsynDataUpdate(object state)
        {
            
        }

        public void AsynInstantiate()
        {

        }

    }

}
