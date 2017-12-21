using System;
using System.Collections.Generic;
using System.Linq;

namespace JiongXiaGu.Collections
{

    /// <summary>
    /// 按类型作为Key的字典结构;
    /// </summary>
    public class TypeDictionary : ITypeDictionary
    {
        private readonly IDictionary<Type, object> dictionary;

        public IEnumerable<Type> Keys => dictionary.Keys;
        public IEnumerable<object> Values => dictionary.Values;
        public int Count => dictionary.Count;

        public TypeDictionary()
        {
            dictionary = new Dictionary<Type, object>();
        }

        public TypeDictionary(IDictionary<Type, object> values)
        {
            dictionary = values;
        }

        /// <summary>
        /// 获取到元素,若未能获取到则返回异常;
        /// </summary>
        public T Get<T>()
        {
            var key = GetKey<T>();
            var value = dictionary[key];
            var tValue = (T)value;
            return tValue;
        }

        /// <summary>
        /// 设置到元素,若未能获取到则返回异常;
        /// </summary>
        public void Set<T>(T item)
        {
            var key = GetKey<T>();
            dictionary[key] = item;
        }

        /// <summary>
        /// 添加元素,若已经存在则返回异常;
        /// </summary>
        public void Add<T>(T item)
        {
            var key = GetKey<T>();
            dictionary.Add(key, item);
        }

        /// <summary>
        /// 添加或更新元素;
        /// </summary>
        public AddOrUpdateStatus AddOrUpdate<T>(T item)
        {
            var key = GetKey<T>();
            return dictionary.AddOrUpdate(key, item);
        }

        /// <summary>
        /// 获取到相同类型的元素,若未找到则返回默认值;
        /// </summary>
        public T Find<T>()
        {
            foreach (var value in dictionary.Values)
            {
                if (value is T)
                {
                    T tValue = (T)value;
                    return tValue;
                }
            }
            return default(T);
        }

        /// <summary>
        /// 获取到所有相同类型的元素;
        /// </summary>
        public IEnumerable<T> FindAll<T>()
        {
            foreach (var value in dictionary.Values)
            {
                if (value is T)
                {
                    T tValue = (T)value;
                    yield return tValue;
                }
            }
        }

        /// <summary>
        /// 返回序列中满足指定条件的第一个元素,若不存在则返回异常 InvalidOperationException;
        /// </summary>
        public object Find<T>(Func<object, bool> predicate)
        {
            return dictionary.Values.First(predicate);
        }

        /// <summary>
        /// 移除对应类型;
        /// </summary>
        public bool Remove<T>()
        {
            var key = GetKey<T>();
            return dictionary.Remove(key);
        }

        /// <summary>
        /// 确认是否存在该元素;
        /// </summary>
        public bool Contains<T>()
        {
            var key = GetKey<T>();
            return dictionary.ContainsKey(key);
        }

        /// <summary>
        /// 尝试获取到对应元素;
        /// </summary>
        public bool TryGetValue<T>(out T item)
        {
            var key = GetKey<T>();
            object value;
            if (dictionary.TryGetValue(key, out value))
            {
                item = (T)value;
                return true;
            }
            item = default(T);
            return false;
        }

        /// <summary>
        /// 清除所有元素;
        /// </summary>
        public void Clear()
        {
            dictionary.Clear();
        }

        /// <summary>
        /// 获取到对应Key;
        /// </summary>
        private Type GetKey<T>()
        {
            var type = typeof(T);
            return type;
        }
    }
}
