using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 允许记录和观察的资源;
    /// </summary>
    public abstract class Resources : ObservableVariable
    {


        /// <summary>
        /// 总产量,每次更新增加的数量;
        /// </summary>
        public int Throughput { get; private set; }

        /// <summary>
        /// 生产总量;
        /// </summary>
        public int Number { get; private set; }




    }

}
