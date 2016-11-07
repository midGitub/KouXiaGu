using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XGame
{

    /// <summary>
    /// 读取进度;
    /// </summary>
    public interface ISchedule
    {
        /// <summary>
        /// 读取进度;
        /// </summary>
        float Progress { get; }

        /// <summary>
        /// 需要读取的总数;
        /// </summary>
        float Total { get; set; }

        /// <summary>
        /// 已经完成的数目;
        /// </summary>
        float Complete { get; set; }

    }

    /// <summary>
    /// 资源读取进度;
    /// </summary>
    public class Schedule : ISchedule
    {
        public Schedule()
        {
            Reset();
        }

        public Schedule(int total)
        {
            Reset();
            this.m_total = total;
        }

        /// <summary>
        /// 需要读取的总数;
        /// </summary>
        private float m_total;

        /// <summary>
        /// 已经完成的数目;
        /// </summary>
        private float m_complete;

        /// <summary>
        /// 当前的进度;
        /// </summary>
        public float Progress
        {
            get { return Complete / Total; }
        }

        /// <summary>
        /// 需要读取的总数;
        /// </summary>
        public float Total
        {
            get { return m_total; }
        }

        /// <summary>
        /// 已经完成的数目;
        /// </summary>
        public float Complete
        {
            get { return m_complete; }
        }

        float ISchedule.Total { get { return m_total; } set { m_total = value; } }
        float ISchedule.Complete { get { return m_complete; } set { m_complete = value; } }

        public void Reset()
        {
            this.m_total = 0;
            this.m_complete = 0;
        }

    }


}
