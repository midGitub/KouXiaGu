using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;

namespace KouXiaGu.WorldEvents
{


    public class TownGroup
    {

        public TownGroup()
        {
            this.towns = new CustomDictionary<int, Town>();
        }

        public TownGroup(IEnumerable<int> identifications)
        {
            this.towns = identifications.ToDictionary<Town>();
        }

        /// <summary>
        /// 城镇信息;
        /// </summary>
        CustomDictionary<int, Town> towns;


    }

}
