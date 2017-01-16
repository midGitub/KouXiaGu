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
        /// 最多两种道路重叠，组成 丁字路口，Y字路口，十字路口，道路;
        /// </summary>
        const int MAX_ROAD_COUNT = RoadBuilder.MAX_ROAD_COUNT;


        /// <summary>
        /// 若不存在道路则为空；
        /// </summary>
        [ProtoMember(1)]
        public List<Road> RoadInfos;


        /// <summary>
        /// 存在道路?
        /// </summary>
        public bool Exist
        {
            get { return RoadInfos != null && RoadInfos.Count > 0; }
        }


        /// <summary>
        /// 加入道路信息,若道路信息已经填满则返回异常 ArgumentOutOfRangeException();
        /// </summary>
        public void Add(Road road)
        {
            if (RoadInfos == null)
            {
                RoadInfos = new List<Road>(MAX_ROAD_COUNT);
                RoadInfos.Add(road);
            }
            else if (RoadInfos.Count < 2)
            {
                RoadInfos.Add(road);
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
            if (RoadInfos == null)
                return false;

            return RoadInfos.Remove(item => item.ID == id);
        }

        /// <summary>
        /// 尝试获取到ID相同的道路信息;
        /// </summary>
        public bool TryGetValue(int id, out Road road)
        {
            if (RoadInfos == null)
            {
                road = default(Road);
                return false;
            }
            else
            {
                road = RoadInfos.Find(item => item.ID == id);
                return true;
            }
        }

        /// <summary>
        /// 获取到所有道路信息;
        /// </summary>
        public IEnumerable<Road> GetRoadInfos()
        {
            if (RoadInfos == null)
                return EmptyCollection<Road>.GetInstance;

            return RoadInfos;
        }

        public void Clear()
        {
            RoadInfos = null;
        }

        /// <summary>
        /// 是否存在相同的道路信息;
        /// </summary>
        public bool Equals(RoadNode other)
        {
            if (other == null)
                return false;
            if (RoadInfos == null && other.RoadInfos == null)
                return true;
            else
                return RoadInfos.Contains(item => other.RoadInfos.Contains(item));
        }

    }

}
