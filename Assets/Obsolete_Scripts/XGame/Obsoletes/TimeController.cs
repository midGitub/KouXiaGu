//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace XGame
//{

//    /// <summary>
//    /// 事件控制器,定义从存档读取时间的方法;
//    /// </summary>
//    [DisallowMultipleComponent]
//    public class TimeController : Controller<TimeController>
//    {

//        /// <summary>
//        /// 时间倍数;(仅在编辑状态确认!)
//        /// </summary>
//        [SerializeField]
//        private float timeMultiple = 1;

//        /// <summary>
//        /// 指定开始的年份;
//        /// </summary>
//        [Header("指定开始时间;")]
//        [SerializeField]
//        private int year = 2016;

//        /// <summary>
//        /// 表示日期的月份部分。
//        /// </summary>
//        [SerializeField]
//        private int month = 10;

//        /// <summary>
//        /// 表示的日期为该月中的第几天。
//        /// </summary>
//        [SerializeField]
//        private int day = 18;

//        /// <summary>
//        /// 游戏开始于几点(24小时制);
//        /// </summary>
//        [SerializeField]
//        private int hour = 0;

//        /// <summary>
//        /// 游戏开始于几分;
//        /// </summary>
//        [SerializeField]
//        private int minute = 0;

//        /// <summary>
//        /// 游戏开始于几秒;
//        /// </summary>
//        [SerializeField]
//        private int second = 0;

//        /// <summary>
//        /// 游戏开始的年份;
//        /// </summary>
//        public int Year
//        {
//            get { return year; }
//            set { year = value; }
//        }

//        /// <summary>
//        /// 时间倍数;(仅在编辑状态确认!)
//        /// </summary>
//        public float TimeMultiple
//        {
//            get { return timeMultiple; }
//        }

//        protected override TimeController This
//        {
//            get { return this; }
//        }

//        /// <summary>
//        /// 获取到游戏开始的时间;
//        /// </summary>
//        /// <returns></returns>
//        public DateTime GetStartTime()
//        {
//            return new DateTime(year, month, day, hour, minute, second);
//        }

//    }

//}
