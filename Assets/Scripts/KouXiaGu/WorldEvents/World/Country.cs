using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 国家;
    /// </summary>
    public class Country
    {

        Country(int id)
        {
            this.ID = id;
        }


        /// <summary>
        /// 唯一标识;
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// 资源信息;
        /// </summary>
        CustomDictionary<int, Resource> resources;


        /// <summary>
        /// 资源信息;
        /// </summary>
        public IReadOnlyDictionary<int, Resource> Resources
        {
            get { return resources; }
        }



    }

}
