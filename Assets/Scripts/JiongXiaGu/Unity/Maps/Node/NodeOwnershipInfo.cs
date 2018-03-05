using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Maps
{

    /// <summary>
    /// 地图归属信息;
    /// </summary>
    public struct NodeOwnershipInfo : IEquatable<NodeOwnershipInfo>
    {
        /// <summary>
        /// 所属的城镇ID;
        /// </summary>
        public int CityID { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is NodeOwnershipInfo)
            {
                return Equals((NodeOwnershipInfo)obj);
            }
            return false;
        }

        public bool Equals(NodeOwnershipInfo other)
        {
            return CityID == other.CityID;
        }

        public override int GetHashCode()
        {
            var hashCode = 1713381551;
            hashCode = hashCode * -1521134295 + CityID.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(NodeOwnershipInfo a, NodeOwnershipInfo b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(NodeOwnershipInfo a, NodeOwnershipInfo b)
        {
            return !a.Equals(b);
        }
    }
}
