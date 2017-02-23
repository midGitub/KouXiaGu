using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 资源信息;
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// 产量;
        /// </summary>
        int DailyOutput { get; }

        /// <summary>
        /// 占用的产量;
        /// </summary>
        int OccupiedOutput { get; }

        /// <summary>
        /// 剩余的产量;
        /// </summary>
        int SurplusOutput { get; }


    }

    /// <summary>
    /// 只读的资源信息;
    /// </summary>
    public interface IReadOnlyResource
    {
        /// <summary>
        /// 日产量;
        /// </summary>
        int DailyOutput { get; }


    }

}
