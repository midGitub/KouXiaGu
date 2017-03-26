using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{
    

    /// <summary>
    /// 人力资源级别要求;
    /// </summary>
    public class ManpowerLevel
    {

        /// <summary>
        /// 每日需求;
        /// </summary>
        public IEnumerable<Container> DailyRequirements { get; private set; }

    }

    /// <summary>
    /// 人力资源;
    /// </summary>
    public class Manpower
    {

        /// <summary>
        /// 数量;
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// 每日更新一次;
        /// </summary>
        public void DayUpdate()
        {

        }

    }

}
