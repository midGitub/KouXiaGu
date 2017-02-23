﻿using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Collections;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 国家;
    /// </summary>
    public class Country : IEquatable<Country>
    {

        internal Country()
        {
        }

        internal Country(int id)
        {
            this.ID = id;
        }


        /// <summary>
        /// 唯一标识;
        /// </summary>
        public int ID { get; private set; }



        public bool Equals(Country other)
        {
            return other.ID == this.ID;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Country;

            if (other == null)
                return false;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            return this.ID;
        }


        public static bool operator ==(Country v1, Country v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Country v1, Country v2)
        {
            return !v1.Equals(v2);
        }

    }

}
