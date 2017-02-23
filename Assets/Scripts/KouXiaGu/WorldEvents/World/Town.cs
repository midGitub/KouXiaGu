using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 城镇;
    /// </summary>
    public class Town : IEquatable<Town>
    {

        internal Town()
        {
        }

        internal Town(int id)
        {
            this.ID = id;
        }


        /// <summary>
        /// 唯一标识;
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// 城镇所属国家;
        /// </summary>
        public Country BelongCountry { get; private set; }


        public bool Equals(Town other)
        {
            return other.ID == this.ID;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Town;

            if (other == null)
                return false;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            return this.ID;
        }


        public static bool operator ==(Town v1, Town v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Town v1, Town v2)
        {
            return !v1.Equals(v2);
        }

    }

}
