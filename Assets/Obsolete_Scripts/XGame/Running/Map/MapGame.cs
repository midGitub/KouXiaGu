using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using UnityEngine;

namespace XGame.Running.Map
{

    /// <summary>
    /// 一个限制在地图范围内的二维地图;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ProtoContract]
    public class MapGame<T> : IDictionary<IntVector2, T>
    {

        public MapGame()
        {
            this.m_MapCollection = new Dictionary<IntVector2, T>();
        }

        [ProtoMember(1)]
        private Dictionary<IntVector2, T> m_MapCollection;


        #region 通过 m_MapCollection 实现的方法;

        public ICollection<IntVector2> Keys
        {
            get { return ((IDictionary<IntVector2, T>)this.m_MapCollection).Keys; }
        }

        public ICollection<T> Values
        {
            get { return ((IDictionary<IntVector2, T>)this.m_MapCollection).Values; }
        }

        public int Count
        {
            get { return ((IDictionary<IntVector2, T>)this.m_MapCollection).Count; }
        }

        public bool IsReadOnly
        {
            get { return ((IDictionary<IntVector2, T>)this.m_MapCollection).IsReadOnly; }
        }

        public T this[IntVector2 key]
        {
            get { return ((IDictionary<IntVector2, T>)this.m_MapCollection)[key]; }
            set { ((IDictionary<IntVector2, T>)this.m_MapCollection)[key] = value; }
        }

        public void Add(IntVector2 key, T value)
        {
            ((IDictionary<IntVector2, T>)this.m_MapCollection).Add(key, value);
        }

        public bool ContainsKey(IntVector2 key)
        {
            return ((IDictionary<IntVector2, T>)this.m_MapCollection).ContainsKey(key);
        }

        public bool Remove(IntVector2 key)
        {
            return ((IDictionary<IntVector2, T>)this.m_MapCollection).Remove(key);
        }

        public bool TryGetValue(IntVector2 key, out T value)
        {
            return ((IDictionary<IntVector2, T>)this.m_MapCollection).TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<IntVector2, T> item)
        {
            ((IDictionary<IntVector2, T>)this.m_MapCollection).Add(item);
        }

        public void Clear()
        {
            ((IDictionary<IntVector2, T>)this.m_MapCollection).Clear();
        }

        public bool Contains(KeyValuePair<IntVector2, T> item)
        {
            return ((IDictionary<IntVector2, T>)this.m_MapCollection).Contains(item);
        }

        public void CopyTo(KeyValuePair<IntVector2, T>[] array, int arrayIndex)
        {
            ((IDictionary<IntVector2, T>)this.m_MapCollection).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<IntVector2, T> item)
        {
            return ((IDictionary<IntVector2, T>)this.m_MapCollection).Remove(item);
        }

        public IEnumerator<KeyValuePair<IntVector2, T>> GetEnumerator()
        {
            return ((IDictionary<IntVector2, T>)this.m_MapCollection).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<IntVector2, T>)this.m_MapCollection).GetEnumerator();
        }

        #endregion


        /// <summary>
        /// 获取到这个点周围的 元素,包括其本身;
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetEnumerable(IntVector2 position, Aspect aspects)
        {
            T item;
            IntVector2 offsetPosition;
            foreach (var aspect in AspectHelper.GetAspect(aspects))
            {
                offsetPosition = position + AspectHelper.GetVector(aspect);
                if (m_MapCollection.TryGetValue(offsetPosition, out item))
                {
                    yield return item;
                }
                continue;
            }
        }



    }

}
