using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace XGame
{

    /// <summary>
    /// 游戏中存在的方向;按顺时针旋转顺序定义每个位值,从North开始,本身算最后一位;
    /// </summary>
    [Flags]
    public enum Aspect : int
    {
        /// <summary>
        /// 北,
        /// </summary>
        [Aspect(South, AspectHelper.SideNorth, 0, 1, 0, South, North)]
        North = 1,

        /// <summary>
        /// 东北,
        /// </summary>
        [Aspect(SouthWestern, AspectHelper.SideNorth | AspectHelper.SideWestern, 1, 1, -45, SouthEast, NorthWestern)]
        NorthEast = 2,

        /// <summary>
        /// 东,
        /// </summary>
        [Aspect(Western, AspectHelper.SideEast, 1, 0, -90, East, Western)]
        East = 4,

        /// <summary>
        /// 东南;
        /// </summary>
        [Aspect(NorthWestern, AspectHelper.SideSouth | AspectHelper.SideEast, 1, -1, -135, NorthEast, SouthWestern)]
        SouthEast = 8,

        /// <summary>
        /// 南;
        /// </summary>
        [Aspect(North, AspectHelper.SideSouth, 0, -1, 180, North, South)]
        South = 16,

        /// <summary>
        /// 西南;
        /// </summary>
        [Aspect(NorthEast, AspectHelper.SideSouth | AspectHelper.SideWestern, -1, -1, 135, NorthWestern, SouthEast)]
        SouthWestern = 32,

        /// <summary>
        /// 西,
        /// </summary>
        [Aspect(East, AspectHelper.SideWestern, -1, 0, 90, Western, East)]
        Western = 64,

        /// <summary>
        /// 西北,
        /// </summary>
        [Aspect(SouthEast, AspectHelper.SideNorth | AspectHelper.SideWestern, -1, 1, 45, SouthWestern, NorthEast)]
        NorthWestern = 128,

        /// <summary>
        /// 本身,
        /// </summary>
        [Aspect(Itself, Itself, 0, 0, 0, Itself, Itself)]
        Itself = 256,

    }


    /// <summary>
    /// 方向信息;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field,
        AllowMultiple = false)]
    public class AspectAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="opposite">相对的方向;</param>
        /// <param name="whichSide">属于哪一面;</param>
        /// <param name="offset_x">向X偏移量;</param>
        /// <param name="offset_y">向Y偏移量;</param>
        /// <param name="euler_z">旋转到方向时,z轴量;</param>
        /// <param name="mirrorX">镜像x轴获取到的方向;</param>
        /// <param name="mirrorY">镜像y轴获取到的方向;</param>
        public AspectAttribute(
            Aspect opposite,
            Aspect whichSide,
            int offset_x,
            int offset_y,
            float euler_z, Aspect mirrorX, Aspect mirrorY)
        {
            this.Opposite = opposite;
            this.Side = whichSide;
            this.Vector = new IntVector2(offset_x, offset_y);
            this.Euler_z = Euler_z;
            quaternion = Quaternion.Euler(0, 0, euler_z);
            this.MirrorX = mirrorX;
            this.MirrorY = mirrorY;
        }

        /// <summary>
        /// 相对的方向;
        /// </summary>
        public Aspect Opposite { get; private set; }

        /// <summary>
        /// 属于哪一面;
        /// </summary>
        public Aspect Side { get; private set; }

        /// <summary>
        /// 偏移量;
        /// </summary>
        public IntVector2 Vector { get; private set; }

        /// <summary>
        /// 旋转到方向时,z轴量
        /// </summary>
        public float Euler_z { get; private set; }

        /// <summary>
        /// 角度;
        /// </summary>
        public Quaternion quaternion { get; private set; }

        /// <summary>
        /// 相对X轴获得的镜面方向;
        /// </summary>
        public Aspect MirrorX { get; private set; }

        /// <summary>
        /// 相对Y轴获得的镜面方向;
        /// </summary>
        public Aspect MirrorY { get; private set; }

    }


    public static class AspectHelper
    {
        /// <summary>
        /// 南面;
        /// </summary>
        public const Aspect SideSouth = Aspect.South | Aspect.SouthWestern | Aspect.SouthEast;

        /// <summary>
        /// 东面;
        /// </summary>
        public const Aspect SideEast = Aspect.East | Aspect.SouthEast | Aspect.NorthEast;

        /// <summary>
        /// 西面;
        /// </summary>
        public const Aspect SideWestern = Aspect.Western | Aspect.SouthWestern | Aspect.NorthWestern;

        /// <summary>
        /// 北面;
        /// </summary>
        public const Aspect SideNorth = Aspect.North | Aspect.NorthWestern | Aspect.NorthEast;


        /// <summary>
        /// 空;
        /// </summary>
        public const Aspect Empty = 0;

        /// <summary>
        /// 所有正方向;
        /// </summary>
        public const Aspect FourDirections = Aspect.East | Aspect.North | Aspect.South | Aspect.Western;

        /// <summary>
        /// 东南西北 + 本身;
        /// </summary>
        public const Aspect FiveDirections = FourDirections | Aspect.Itself;

        /// <summary>
        /// 所有角方向;
        /// </summary>
        public const Aspect Corners = Aspect.NorthWestern | Aspect.NorthEast | Aspect.SouthEast | Aspect.SouthWestern;

        /// <summary>
        /// 所有方向;
        /// </summary>
        public const Aspect Around = SideEast | SideNorth | SideSouth | SideWestern | Aspect.Itself;

        /// <summary>
        /// 所有方向;除了本身的方向;
        /// </summary>
        public const Aspect AroundExceptSelf = SideEast | SideNorth | SideSouth | SideWestern;

        public const int SidesCount = 4;

        public const int AroundExceptSelfCount = 8;

        /// <summary>
        /// 方向信息合集;
        /// </summary>
        private static readonly Dictionary<Aspect, AspectAttribute> m_AspectDictionary;

        static AspectHelper()
        {
            m_AspectDictionary = AttributeHelper.GetDictionaryFormField<Aspect, AspectAttribute>(
                typeof(Aspect),
                BindingFlags.Static | BindingFlags.Public);
        }

        private static AspectAttribute GetAspectAttribute(Aspect aspect)
        {
            try
            {
                return m_AspectDictionary[aspect];
            }
            catch (KeyNotFoundException e)
            {
                throw new KeyNotFoundException("获取的方向错误,仅支持单个方向获取! 获取的方向 :" + aspect, e);
            }
        }

        /// <summary>
        /// 将多个方向拆分为单个方向;并先从高位返回;
        /// </summary>
        /// <param name="aspects"></param>
        /// <returns></returns>
        public static IEnumerable<Aspect> GetAspect(Aspect aspects)
        {
            int intAspect = (int)aspects;
            for (int aspect = (int)Aspect.Itself; aspect != 0; aspect >>= 1)
            {
                if ((aspect & intAspect) > 0)
                    yield return (Aspect)aspect;
            }
        }

        /// <summary>
        /// 确认这个方向是否包含于 reference 传入的方向内,若是则返回true,否则返回false;
        /// </summary>
        /// <param name="aspect"></param>
        /// <param name="references"></param>
        public static bool IsAspect(Aspect aspect, Aspect references)
        {
            return (aspect & references) != 0;
        }

        /// <summary>
        /// 确认这个方向是否包含于 reference 传入的方向内,若不是则返回异常ArgumentOutOfRangeException;
        /// </summary>
        /// <param name="aspect"></param>
        /// <param name="references"></param>
        public static void ConfirmAspect(Aspect aspect, Aspect references)
        {
            if ((aspect & references) == 0)
            {
                throw new ArgumentOutOfRangeException("传入方向不符合定义!错误传入 : " + aspect.ToString());
            }
        }

        /// <summary>
        /// 是否属于单个方向?若不是则返回异常ArgumentOutOfRangeException;
        /// </summary>
        /// <returns></returns>
        public static void IsSingleAspect(Aspect aspects)
        {
            if (!m_AspectDictionary.ContainsKey(aspects))
            {
                throw new ArgumentOutOfRangeException("传入方向不符合定义!错误传入 : " + aspects.ToString());
            }
        }

        ///// <summary>
        ///// 是否为属于东南西北,若不是则返回异常ArgumentOutOfRangeException;
        ///// </summary>
        ///// <param name="aspects"></param>
        //[Obsolete]
        //public static void IsFourDirections(Aspect2D aspects)
        //{
        //    if ((aspects & FourDirections) == 0)
        //    {
        //        throw new ArgumentOutOfRangeException("传入方向不符合定义!错误传入 : " + aspects.ToString());
        //    }
        //}

        ///// <summary>
        ///// 是否为属于东南西北 + 本身,若不是则返回异常ArgumentOutOfRangeException;
        ///// </summary>
        //[Obsolete]
        //public static void IsFiveDirections(Aspect2D aspects)
        //{
        //    if ((aspects & FiveDirections) == 0)
        //    {
        //        throw new ArgumentOutOfRangeException("传入方向不符合定义!错误传入 : " + aspects.ToString());
        //    }
        //}

        ///// <summary>
        ///// 是否为属于角方向,若不是则返回异常ArgumentOutOfRangeException;
        ///// </summary>
        //[Obsolete]
        //public static void IsCorners(Aspect2D aspects)
        //{
        //    if ((aspects & Corners) == 0)
        //    {
        //        throw new ArgumentOutOfRangeException("传入方向不符合定义!错误传入 : " + aspects.ToString());
        //    }
        //}

        ///// <summary>
        ///// 是否属于周围方向,不包括本身;
        ///// </summary>
        //[Obsolete]
        //public static void IsAroundExceptSelf(Aspect2D aspects)
        //{
        //    if ((aspects & AroundExceptSelf) == 0)
        //    {
        //        throw new ArgumentOutOfRangeException("传入方向不符合定义!错误传入 : " + aspects.ToString());
        //    }
        //}

        /// <summary>
        /// 获取这个方向的对立方向;
        /// </summary>
        public static Aspect GetOpposite(Aspect aspect)
        {
            return GetAspectAttribute(aspect).Opposite;
        }

        /// <summary>
        /// 获取到所在的一面;
        /// </summary>
        /// <param name="aspect"></param>
        /// <returns></returns>
        public static Aspect GetSide(Aspect aspect)
        {
            return GetAspectAttribute(aspect).Side;
        }

        /// <summary>
        /// 获取到镜像;
        /// </summary>
        /// <param name="aspect">转换的方向;</param>
        /// <param name="axisX"> axisX true,根据X轴镜像; false,根据Y轴镜像;</param>
        /// <returns></returns>
        public static Aspect GetMirror(Aspect aspect, bool axisX)
        {
            AspectAttribute ass = GetAspectAttribute(aspect);

            if (axisX)
            {
                return ass.MirrorX;
            }
            else
            {
                return ass.MirrorY;
            }
        }


        /// <summary>
        /// 对这个单位进行旋转;
        /// </summary>
        /// <param name="aspect"></param>
        /// <param name="rotationUnit">多少个旋转单位;整数位顺时针,负数为逆时针;</param>
        /// <returns></returns>
        public static Aspect Rotate(Aspect aspect, int rotationUnit)
        {
            IsSingleAspect(aspect);
            ConfirmAspect(aspect, AroundExceptSelf);

            int maxAspect;
            int intAspect;

            rotationUnit %= AroundExceptSelfCount;

            if (rotationUnit == 0)
            {
                return aspect;
            }
            else
            {
                intAspect = (int)aspect;
                if (rotationUnit > 0)
                {
                    maxAspect = (int)Aspect.Itself;
                    for (; rotationUnit > 0; rotationUnit--)
                    {
                        intAspect <<= 1;
                        if (intAspect >= maxAspect)
                            intAspect = (int)Aspect.North;
                    }
                }
                else if (rotationUnit < 0)
                {
                    maxAspect = 0;
                    for (; rotationUnit < 0; rotationUnit++)
                    {
                        intAspect >>= 1;
                        if (intAspect <= maxAspect)
                            intAspect = (int)Aspect.NorthWestern;
                    }
                }
                return (Aspect)intAspect;
            }
        }

        /// <summary>
        /// 获取到旋转单位;仅逆时针转到;
        /// </summary>
        /// <returns></returns>
        public static int GetRotationUnit(Aspect oldRotation, Aspect newRotation)
        {
            IsSingleAspect(oldRotation);
            IsSingleAspect(newRotation);
            ConfirmAspect(oldRotation, AroundExceptSelf);
            ConfirmAspect(newRotation, AroundExceptSelf);

            if (oldRotation == newRotation)
                return 0;

            int intOldRotation = (int)oldRotation;
            int intNewRotation = (int)newRotation;
            int rotationUnit = 0;

            do
            {
                intOldRotation >>= 1;
                if (intOldRotation == 0)
                    intOldRotation = (int)Aspect.NorthWestern;
                rotationUnit--;
            }
            while (intOldRotation != intNewRotation);

            return rotationUnit;
        }

        /// <summary>
        /// 获取到最短的旋转单位;
        /// </summary>
        /// <param name="oldRotation"></param>
        /// <param name="newRotation"></param>
        /// <returns></returns>
        public static int GetRotationUnit_Min(Aspect oldRotation, Aspect newRotation)
        {
            IsSingleAspect(oldRotation);
            IsSingleAspect(newRotation);
            ConfirmAspect(oldRotation, AroundExceptSelf);
            ConfirmAspect(newRotation, AroundExceptSelf);

            if (oldRotation == newRotation)
                return 0;

            int clockwiseRotation = (int)oldRotation;
            int counterclockwiseRotation = (int)oldRotation;
            int intNewRotation = (int)newRotation;
            int clockwise = 0;
            int counterclockwise = 0;

            do
            {
                counterclockwiseRotation >>= 1;
                if (counterclockwiseRotation == 0)
                    counterclockwiseRotation = (int)Aspect.NorthWestern;
                counterclockwise--;

                clockwiseRotation <<= 1;
                if (clockwiseRotation == 256)
                    clockwiseRotation = 0;
                clockwise++;
            }
            while (clockwiseRotation != intNewRotation);

            return clockwise > counterclockwise ? counterclockwise : clockwise;
        }

        /// <summary>
        /// 获取到左转后的方向,旋转两个单位后的方向;
        /// </summary>
        /// <param name="aspect"></param>
        /// <returns></returns>
        public static Aspect GetLeft(Aspect aspect)
        {
            return Rotate(aspect, -2);
        }

        /// <summary>
        /// 获取到右转后的方向,旋转两个单位后的方向;
        /// </summary>
        /// <param name="aspect"></param>
        /// <returns></returns>
        public static Aspect GetRight(Aspect aspect)
        {
            return Rotate(aspect, 2);
        }

        /// <summary>
        /// 获取到这个方向的四元数;
        /// </summary>
        /// <param name="aspect"></param>
        /// <returns></returns>
        public static Quaternion GetQuaternion(Aspect aspect)
        {
            return GetAspectAttribute(aspect).quaternion;
        }

        /// <summary>
        /// 返回旋转方向;仅旋转两个旋转单位的方向;
        /// </summary>
        /// <returns>返回 0 = 没变化, -1 = Left, 1 = Right, other = Opposite </returns>
        public static int TransfromDirection2(Aspect oldRotation, Aspect newRotation)
        {
            if (oldRotation == newRotation)
                return 0;
            if (newRotation == GetLeft(oldRotation))
                return -1;
            if (newRotation == GetRight(oldRotation))
                return 1;

            return 2;
        }

        /// <summary>
        /// 根据旋转角度 返回 对应direction的旋转到的角度;
        /// </summary>
        public static Aspect TransfromDirection(Aspect oldRotation, Aspect newRotation, Aspect rotation)
        {
            int RotationUnit = GetRotationUnit(oldRotation, newRotation);
            return Rotate(rotation, RotationUnit);
        }

        /// <summary>
        /// 根据方向获取到偏移向量;
        /// </summary>
        public static IntVector2 GetVector(Aspect aspect)
        {
            return GetAspectAttribute(aspect).Vector;
        }

        /// <summary>
        /// 获取到所有方向
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IntVector2> GetVector()
        {
            foreach (var item in m_AspectDictionary.Values)
            {
                yield return item.Vector;
            }
        }

    }


}
