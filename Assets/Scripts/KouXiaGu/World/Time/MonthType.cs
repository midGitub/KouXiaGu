using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{

    [Flags]
    public enum MonthType
    {

        /// <summary>
        /// 无;
        /// </summary>
        None = 0,

        /// <summary>
        /// 一月;
        /// </summary>
        January = 1,

        /// <summary>
        /// 二月;
        /// </summary>
        February = 2,

        /// <summary>
        /// 三月;
        /// </summary>
        March = 4,

        /// <summary>
        /// 四月;
        /// </summary>
        April = 8,

        /// <summary>
        /// 五月;
        /// </summary>
        May = 16,

        /// <summary>
        /// 六月;
        /// </summary>
        June = 32,

        /// <summary>
        /// 七月;
        /// </summary>
        July = 64,

        /// <summary>
        /// 八月;
        /// </summary>
        August = 128,

        /// <summary>
        /// 九月;
        /// </summary>
        September = 256,

        /// <summary>
        /// 十月;
        /// </summary>
        October = 512,

        /// <summary>
        /// 十一月;
        /// </summary>
        November = 1024,

        /// <summary>
        /// 十二月;
        /// </summary>
        December = 2048,

        /// <summary>
        /// 所有月份
        /// </summary>
        All = 0xFFF,
    }

}
