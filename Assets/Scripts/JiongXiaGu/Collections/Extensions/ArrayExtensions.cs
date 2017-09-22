using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.Collections
{


    public static class ArrayExtensions
    {

        /// <summary>
        /// 使用 items 替换掉 index 下标处和其之后的内容,舍弃超出数组部分;
        /// </summary>
        public static void Add<T>(this IList<T> array, int index, IList<T> items)
        {
            for (int i = index, j = 0; i < array.Count && j < items.Count; i++, j++)
            {
                array[i] = items[j];
            }
        }

        /// <summary>
        /// 将 item 添加到返回数组的第一位;
        /// </summary>
        public static T[] AddFirst<T>(this IList<T> array, T item)
        {
            T[] newArray = new T[array.Count + 1];
            newArray[0] = item;

            for (int i = 0; i < array.Count; i++)
            {
                newArray[i + 1] = array[i];
            }

            return newArray;
        }

    }

}
