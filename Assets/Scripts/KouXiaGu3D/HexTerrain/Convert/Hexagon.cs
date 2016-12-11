
//是否允许设置其它属性?
//#define SET_PROPERTY

using System;
using UnityEngine;

namespace KouXiaGu
{

    [Serializable]
    public struct Hexagon
    {

        public Hexagon(double outerRadius)
        {
            this.outerRadius = outerRadius;
        }

        static readonly double cos30 = Math.Sqrt(3) / 2;

        [SerializeField]
        double outerRadius;

        /// <summary>
        /// 外半径
        /// </summary>
        public double OuterRadius
        {
            get { return outerRadius; }
#if SET_PROPERTY
            set { outerRadius = value; }
#endif
        }

        /// <summary>
        /// 外直径
        /// </summary>
        public double OuterDiameters
        {
            get { return OuterRadius * 2; }
#if SET_PROPERTY
            set { OuterRadius = value / 2; }
#endif
        }

        /// <summary>
        /// 内半径
        /// </summary>
        public double InnerRadius
        {
            get { return cos30 * OuterRadius; }
#if SET_PROPERTY
            set { OuterRadius = value / cos30; }
#endif
        }

        /// <summary>
        /// 内直径
        /// </summary>
        public double InnerDiameters
        {
            get { return InnerRadius * 2; }
#if SET_PROPERTY
            set { OuterRadius = value / 2 / cos30; }
#endif
        }

    }

}
