using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Collections
{

    /// <summary>
    /// 提供合集对比方法的工具类;
    /// </summary>
    public static class Contrast
    {


        /// <summary>
        /// 判断两个合集内容和顺序是否相同,若不相同则返回对应异常;
        /// </summary>
        public static void AreSame<T>(IEnumerable<T> source, IEnumerable<T> target)
        {
            AreSame(source, target, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// 判断两个合集内容和顺序是否相同,若不相同则返回对应异常;
        /// </summary>
        public static void AreSame<T>(IEnumerable<T> source, IEnumerable<T> target, IEqualityComparer<T> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            if (source == target)
                return;

            var sourceEnumerator = source.GetEnumerator();
            var targetEnumerator = target.GetEnumerator();
            int i = 0;
            bool targetNotFinished = false;

            while (sourceEnumerator.MoveNext())
            {
                var item1 = sourceEnumerator.Current;
                targetNotFinished = targetEnumerator.MoveNext();

                if (targetNotFinished)
                {
                    var item2 = targetEnumerator.Current;

                    if (!comparer.Equals(item1, item2))
                    {
                        throw new ArgumentException(string.Format("下标[{0}]对应的值不相同;", i));
                    }
                }
                else
                {
                    throw new ArgumentException(string.Format("合集[{0}]元素少于合集[{1}];", nameof(target), nameof(source)));
                }
            }

            if (i > 0 && targetNotFinished)
            {
                throw new ArgumentException(string.Format("合集[{0}]元素少于合集[{1}];", nameof(source), nameof(target)));
            }
        }


        /// <summary>
        /// 判断两个合集内容和顺序是否相同,若不相同则返回对应异常;
        /// </summary>
        public static void AreSame<T>(ICollection<T> source, ICollection<T> target)
        {
            AreSame(source, target, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// 判断两个合集内容和顺序是否相同,若不相同则返回对应异常;
        /// </summary>
        public static void AreSame<T>(ICollection<T> source, ICollection<T> target, IEqualityComparer<T> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            if (source == target)
                return;
            if (source.Count != target.Count)
                throw CreateDifferentCollectionCount(source.Count, target.Count);

            var targetEnumerator = target.GetEnumerator();
            int i = 0;
            foreach (var item1 in source)
            {
                if (targetEnumerator.MoveNext())
                {
                    var item2 = targetEnumerator.Current;

                    if (!comparer.Equals(item1, item2))
                    {
                        throw new ArgumentException(string.Format("下标[{0}]对应的值不相同;", i));
                    }
                }
                else
                {
                    throw new ArgumentException(string.Format("合集[{0}]发生变化,并且未返回异常;", nameof(target)));
                }

                i++;
            }
        }


        /// <summary>
        /// 判断两个合集内容和顺序是否相同,若不相同则返回对应异常;
        /// </summary>
        public static void AreSame<T>(IList<T> source, IList<T> target)
        {
            AreSame(source, target, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// 判断两个合集内容和顺序是否相同,若不相同则返回对应异常;
        /// </summary>
        public static void AreSame<T>(IList<T> source, IList<T> target, IEqualityComparer<T> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            if (source == target)
                return;
            if (source.Count != target.Count)
                throw CreateDifferentCollectionCount(source.Count, target.Count);

            for (int i = 0; i < source.Count; i++)
            {
                var item1 = source[i];
                var item2 = target[i];

                if (!comparer.Equals(item1, item2))
                {
                    throw new ArgumentException(string.Format("下标[{0}]对应的值不相同;", i));
                }
            }
        }


        /// <summary>
        /// 判断两个字典结构内容是否相同,若不相同则返回对应异常;
        /// </summary>
        public static void AreSame<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> source, IReadOnlyDictionary<TKey, TValue> target)
        {
            AreSame(source, target, EqualityComparer<TValue>.Default);
        }

        /// <summary>
        /// 判断两个字典结构内容是否相同,若不相同则返回对应异常;
        /// </summary>
        public static void AreSame<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> source, IReadOnlyDictionary<TKey, TValue> target, IEqualityComparer<TValue> valueComparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (valueComparer == null)
                throw new ArgumentNullException(nameof(valueComparer));
            if (source == target)
                return;
            if (source.Count != target.Count)
                throw CreateDifferentCollectionCount(source.Count, target.Count);

            foreach (var item1 in source)
            {
                TValue value2;
                if (target.TryGetValue(item1.Key, out value2))
                {
                    if (!valueComparer.Equals(item1.Value, value2))
                    {
                        throw new ArgumentException(string.Format("[{0}]对应的值不相同;", item1.Key));
                    }
                }
                else
                {
                    throw new ArgumentException("缺少键值:" + item1.Key);
                }
            }
        }

        /// <summary>
        /// 当合集数目不同返回的异常;
        /// </summary>
        private static Exception CreateDifferentCollectionCount(int count1, int count2)
        {
            return new ArgumentException(string.Format("两个合集元素总数不同 [{0} , {1}]", count1, count2));
        }
    }
}
