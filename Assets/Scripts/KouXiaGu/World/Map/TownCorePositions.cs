using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 记录城镇中心位置;
    /// </summary>
    [ProtoContract]
    public class TownCorePositions
    {
        public TownCorePositions()
        {
            positions = new HashSet<CubicHexCoord>();
            townIDs = new Dictionary<int, CubicHexCoord>();
        }

        [ProtoMember(1)]
        Dictionary<int, CubicHexCoord> townIDs;

        [ProtoMember(0)]
        HashSet<CubicHexCoord> positions;

        public CubicHexCoord this[int townID]
        {
            get { return townIDs[townID]; }
        }

        /// <summary>
        /// 更新定义的位置,若找到则返回true,否则返回false;
        /// </summary>
        public bool Update(int townID, CubicHexCoord position)
        {
            CubicHexCoord pos;
            if (townIDs.TryGetValue(townID, out pos))
            {
                townIDs[townID] = position;
                positions.Remove(pos);
                positions.Add(position);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 加入定义中心位置;
        /// </summary>
        public void Add(int townID, CubicHexCoord position)
        {
            if (townIDs.ContainsKey(townID))
            {
                throw new ArgumentException("该ID已经定义了中心位置");
            }
            else
            {
                if (positions.Contains(position))
                {
                    throw new ArgumentException("该位置已经定义了对应ID;");
                }
                else
                {
                    positions.Add(position);
                    townIDs.Add(townID, position);
                }
            }
        }

        /// <summary>
        /// 移除定义的位置;
        /// </summary>
        public bool Remove(int townID)
        {
            CubicHexCoord pos;
            if (townIDs.TryGetValue(townID, out pos))
            {
                townIDs.Remove(townID);
                positions.Remove(pos);
                return true;
            }
            return false;
        }
    }
}
