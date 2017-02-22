using System;
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

        public Country(
            int id,
            IEnumerable<int> resIdentifications,
            IEnumerable<int> townIdentifications)
        {
            this.ID = id;
            this.towns = townIdentifications.ToDictionary(townIdentification => new Town(townIdentification));
        }


        /// <summary>
        /// 唯一标识;
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// 资源信息;
        /// </summary>
        public ResourceGroup Resources { get; private set; }

        /// <summary>
        /// 城镇信息;
        /// </summary>
        CustomDictionary<int, Town> towns;


        /// <summary>
        /// 城镇信息;
        /// </summary>
        public IDictionary<int, Town> Towns
        {
            get { return towns; }
        }


        public bool Equals(Country other)
        {
            return this.ID == other.ID;
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
            return ID;
        }

    }

}
