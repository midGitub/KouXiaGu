using System;
using System.Collections.Generic;
using System.Linq;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 建筑物;
    /// </summary>
    public class Building : IEquatable<Building>
    {

        internal Building()
        {
        }

        internal Building(int id) : this()
        {
            this.ID = id;
        }


        /// <summary>
        /// 唯一标识;
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// 建筑所归属的城镇;
        /// </summary>
        public Town BelongTown { get; private set; }


        /// <summary>
        /// 设置建筑所归属的城镇;
        /// </summary>
        public void SetBelongTown(Town newTown)
        {
            if (BelongTown == newTown)
                return;

            throw new NotImplementedException();
        }


        public bool Equals(Building other)
        {
            return other.ID == this.ID;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Building;

            if (other == null)
                return false;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            return this.ID;
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
