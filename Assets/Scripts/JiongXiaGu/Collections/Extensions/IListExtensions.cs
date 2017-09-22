using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.Collections
{


    public static class IListExtensions
    {


        /// <summary>
        /// 移除最先符合条件的元素;
        /// </summary>
        public static bool Remove<T>(this IList<T> list, Func<T, bool> func)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (func(list[i]))
                {
                    list.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

    }

}
