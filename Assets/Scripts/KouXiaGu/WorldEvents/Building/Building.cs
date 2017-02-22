using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 建筑类型;
    /// </summary>
    public class Building : IEquatable<Building>
    {

        Building()
        {
        }

        Building(int id)
        {
            this.ID = id;
        }


        /// <summary>
        /// 唯一编号;
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// 建筑物效果;
        /// </summary>
        public EffectGroup Effects { get; private set; }



        public bool Equals(Building other)
        {
            return this.ID == other.ID;
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
            return ID;
        }

    }

}
