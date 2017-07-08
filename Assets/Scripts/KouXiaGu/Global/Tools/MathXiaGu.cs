using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 提供的数学方法;
    /// </summary>
    public static class MathXiaGu
    {


        public static float Clamp01(float value)
        {
            value = value < 0 ? 0 : value;
            value = value > 1 ? 1 : value;
            return value;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException();

            value = value < min ? min : value;
            value = value > max ? max : value;
            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException();

            value = value < min ? min : value;
            value = value > max ? max : value;
            return value;
        }

        public static short Clamp(short value, short min, short max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException();

            value = value < min ? min : value;
            value = value > max ? max : value;
            return value;
        }



        ///// <summary>
        ///// 获取到最大值,若相等则返回 value1;
        ///// </summary>
        //public static T Max<T>(T value1, T value2)
        //    where T : IComparable<T>
        //{
        //    if (value1.CompareTo(value2) >= 0)
        //    {
        //        return value1;
        //    }
        //    else
        //    {
        //        return value2;
        //    }
        //}

        //public static T Max<T>(IEnumerable<T> values)
        //    where T : IComparable<T>
        //{
        //    bool isFirst = true;
        //    T max = default(T);
        //    foreach (var value in values)
        //    {
        //        if (isFirst)
        //        {
        //            max = value;
        //            isFirst = false;
        //        }
        //        max = Max(max, value);
        //    }
        //    return max;
        //}

        //public static T Max<T>(params T[] values)
        //    where T : IComparable<T>
        //{
        //    return Max(values);
        //}

        //public static T Max<T>(IList<T> values)
        //    where T : IComparable<T>
        //{
        //    var max = values[0];
        //    for (int i = 1; i < values.Count; i++)
        //    {
        //        var value = values[i];
        //        max = Max(max, value);
        //    }
        //    return max;
        //}



        ///// <summary>
        ///// 获取到最小值,若相等则返回 value1
        ///// </summary>
        //public static T Min<T>(T value1, T value2)
        //    where T : IComparable<T>
        //{
        //    if (value1.CompareTo(value2) <= 0)
        //    {
        //        return value1;
        //    }
        //    else
        //    {
        //        return value2;
        //    }
        //}

        //public static T Min<T>(IEnumerable<T> values)
        //    where T : IComparable<T>
        //{
        //    bool isFirst = true;
        //    T min = default(T);
        //    foreach (var value in values)
        //    {
        //        if (isFirst)
        //        {
        //            min = value;
        //            isFirst = false;
        //        }
        //        min = Min(min, value);
        //    }
        //    return min;
        //}

        //public static T Min<T>(IList<T> values)
        //    where T : IComparable<T>
        //{
        //    var min = values[0];
        //    for (int i = 1; i < values.Count; i++)
        //    {
        //        var value = values[i];
        //        min = Min(min, value);
        //    }
        //    return min;
        //}

        //public static T Min<T>(params T[] values)
        //    where T : IComparable<T>
        //{
        //    return Min(values);
        //}
    }
}
