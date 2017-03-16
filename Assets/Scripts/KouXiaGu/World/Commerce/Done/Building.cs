using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{


    public class Building : IEquatable<Building>
    {

        /// <summary>
        /// 编号;
        /// </summary>
        public int BuildingID { get; private set; }

        /// <summary>
        /// 生产信息;
        /// </summary>
        public ProductionInfoGroup ProductionInfo { get; private set; }


        public bool Equals(Building other)
        {
            return other.BuildingID == this.BuildingID;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Building))
                return false;

            return Equals((Building)obj);
        }

        public override int GetHashCode()
        {
            return BuildingID;
        }


        public static bool operator ==(Building v1, Building v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Building v1, Building v2)
        {
            return !v1.Equals(v2);
        }

    }

}
