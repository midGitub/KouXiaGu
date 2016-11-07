using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XGame
{

    /// <summary>
    /// 合集工具类;
    /// </summary>
    public static class CollectionHelper
    {

        #region IDictionary, Dictionary;

        #endregion


        /// <summary>
        /// 对合集内的所有元素都进行操作;
        /// </summary>
        /// <param name="enumerable">合集</param>
        /// <param name="action">进行的操作;</param>
        public static void Operating<T>(IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var t in enumerable)
            {
                action(t);
            }
        }

        /// <summary>
        /// 将 IEnumerable 的元素转换为指定的类型
        /// </summary>
        /// <param name="enumerable">合集</param>
        /// <param name="func">进行转换的方法;</param>
        /// <returns></returns>
        public static IEnumerable<TResult> Cast<T, TResult>(IEnumerable<T> enumerable, Func<T, TResult> func)
        {
            foreach (T t in enumerable)
            {
                yield return func(t);
            }
        }

        /// <summary>
        /// 将 IEnumerable 的元素转换为指定的类型
        /// </summary>
        /// <param name="enumerable">合集</param>
        /// <param name="func">进行转换的方法;</param>
        /// <returns></returns>
        public static IEnumerable<TResult> Cast<T, TResult>(IEnumerable enumerable, Func<T, TResult> func)
        {
            foreach (T t in enumerable)
            {
                yield return func(t);
            }
        }


    }

}
