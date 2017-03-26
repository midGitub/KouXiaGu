using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 人力资源提高要求;
    /// </summary>
    public class ManpowerRequirement
    {
        public ManpowerRequirement(Manpower parent)
        {
            Parent = parent;
        }
        
        public Manpower Parent { get; private set; }


    }

    /// <summary>
    /// 人力资源;
    /// </summary>
    public class Manpower : Townish
    {

        public Manpower(Town belongToTown) : base(belongToTown)
        {
        }

        int[] level;

        /// <summary>
        /// 数量;
        /// </summary>
        public int ManpowerNumber { get; private set; }

        /// <summary>
        /// 成长值;
        /// </summary>
        public int GrowthValue { get; private set; }

        public void AddGrowth()
        {

        }

    }

}
