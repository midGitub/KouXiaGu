using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 城镇;
    /// </summary>
    public class Town
    {

        public Town()
        {

        }

        public Town(int id)
        {
            this.ID = id;
        }

        /// <summary>
        /// 唯一标识;
        /// </summary>
        public int ID { get; private set; }




    }

}
