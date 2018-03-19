using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 添加模式;
    /// </summary>
    public enum AddMode
    {
        /// <summary>
        /// 添加 或者 更新;
        /// </summary>
        AddOrUpdate,
        
        /// <summary>
        /// 仅加入,若存在则返回异常;
        /// </summary>
        AddOnly,
    }


    public static class AddModeExtensions
    {


        public static void Add<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value, AddMode addMode)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            switch (addMode)
            {
                case AddMode.AddOnly:
                    dictionary.Add(key, value);
                    break;

                case AddMode.AddOrUpdate:
                    if (dictionary.ContainsKey(key))
                    {
                        dictionary[key] = value;
                    }
                    else
                    {
                        dictionary.Add(key, value);
                    }
                    break;

                default:
                    throw new IndexOutOfRangeException();
            }
        }
    }
}
