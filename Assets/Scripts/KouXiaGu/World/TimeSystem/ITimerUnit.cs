using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.TimeSystem
{

    /// <summary>
    /// 用于时间更新的单元;
    /// </summary>
    public interface ITimerUnit
    {
        /// <summary>
        /// 增加一个单位;
        /// </summary>
        void AddUnit(int time);
    }
}
