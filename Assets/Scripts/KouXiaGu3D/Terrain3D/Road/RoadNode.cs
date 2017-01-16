using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using KouXiaGu.Collections;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 节点道路信息;
    /// </summary>
    [ProtoContract]
    public class RoadNode : IEquatable<RoadNode>
    {

        /// <summary>
        /// 最大存在的道路数量；
        /// </summary>
        const int MAX_ROAD_COUNT = 2;

        /// <summary>
        /// 最多两种道路重叠，组成 丁字路口，Y字路口，十字路口，道路;
        /// 若不存在道路则为空；
        /// </summary>
        [ProtoMember(1)]
        List<Road> roadInfos;

        /// <summary>
        /// 存在道路?
        /// </summary>
        public bool Exist
        {
            get { return roadInfos != null && roadInfos.Count > 0; }
        }


        /// <summary>
        /// 加入道路信息,若道路信息已经填满则返回异常 ArgumentOutOfRangeException();
        /// </summary>
        public void Add(Road road)
        {
            if (roadInfos == null)
            {
                roadInfos = new List<Road>(MAX_ROAD_COUNT);
                roadInfos.Add(road);
            }
            else if (roadInfos.Count < 2)
            {
                roadInfos.Add(road);
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 移除道路信息;
        /// </summary>
        public bool Remove(int id)
        {
            if (roadInfos == null)
                return false;

            return roadInfos.Remove(item => item.ID == id);
        }

        /// <summary>
        /// 获取到所有道路信息;
        /// </summary>
        public IEnumerable<Road> GetRoadInfos()
        {
            if (roadInfos == null)
                return EmptyCollection<Road>.GetInstance;

            return roadInfos;
        }

        public void Clear()
        {
            roadInfos.Clear();
        }

        /// <summary>
        /// 是否存在相同的道路信息;
        /// </summary>
        public bool Equals(RoadNode other)
        {
            if (other == null)
                return false;
            if (roadInfos == null && other.roadInfos == null)
                return true;
            else
                return roadInfos.Contains(item => other.roadInfos.Contains(item));
        }

    }

}
