using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{


    public class HumanResources
    {

        /// <summary>
        /// 人口数目;
        /// </summary>
        public int HumanNumber { get; private set; }




        /// <summary>
        /// 分配单元;
        /// </summary>
        public class Branch
        {

            /// <summary>
            /// 优先级;
            /// </summary>
            public int Priority { get; private set; }

            /// <summary>
            /// 需求;
            /// </summary>
            public int Demand { get; private set; }

            /// <summary>
            /// 实际分配;
            /// </summary>
            public int Practice { get; private set; }

            public HumanResources Manager { get; private set; }



        }


    }

}
