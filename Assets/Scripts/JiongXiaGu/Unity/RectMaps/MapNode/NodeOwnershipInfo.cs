using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 地图归属信息;
    /// </summary>
    public struct NodeOwnershipInfo : IEquatable<NodeOwnershipInfo>
    {
        /// <summary>
        /// 所属的国家ID;
        /// </summary>
        public int CountryID { get; set; }

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
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return CountryID;
        }
    }
}
