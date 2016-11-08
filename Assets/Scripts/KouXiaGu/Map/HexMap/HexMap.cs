using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using UnityEngine;

namespace KouXiaGu.Map.HexMap
{

    [ProtoContract]
    public class HexMap<T> : IDictionary<IntVector2, T>
    {

        public HexMap()
        {
            mapCollection = new Dictionary<IntVector2, T>();
        }

        public HexMap(IDictionary<IntVector2, T> dictionary)
        {
            mapCollection = new Dictionary<IntVector2, T>(dictionary);
        }

        public HexMap(int capacity)
        {
            mapCollection = new Dictionary<IntVector2, T>(capacity);
        }

        [ProtoMember(1)]
        private Dictionary<IntVector2, T> mapCollection;

        #region IDictionary<IntVector2, T>

        public ICollection<IntVector2> Keys
        {
            get
            {
                return ((IDictionary<IntVector2, T>)this.mapCollection).Keys;
            }
        }

        public ICollection<T> Values
        {
            get
            {
                return ((IDictionary<IntVector2, T>)this.mapCollection).Values;
            }
        }

        public int Count
        {
            get
            {
                return ((IDictionary<IntVector2, T>)this.mapCollection).Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((IDictionary<IntVector2, T>)this.mapCollection).IsReadOnly;
            }
        }

        public T this[IntVector2 key]
        {
            get
            {
                return ((IDictionary<IntVector2, T>)this.mapCollection)[key];
            }

            set
            {
                ((IDictionary<IntVector2, T>)this.mapCollection)[key] = value;
            }
        }

        public void Add(IntVector2 key, T value)
        {
            ((IDictionary<IntVector2, T>)this.mapCollection).Add(key, value);
        }

        public bool ContainsKey(IntVector2 key)
        {
            return ((IDictionary<IntVector2, T>)this.mapCollection).ContainsKey(key);
        }

        public bool Remove(IntVector2 key)
        {
            return ((IDictionary<IntVector2, T>)this.mapCollection).Remove(key);
        }

        public bool TryGetValue(IntVector2 key, out T value)
        {
            return ((IDictionary<IntVector2, T>)this.mapCollection).TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<IntVector2, T> item)
        {
            ((IDictionary<IntVector2, T>)this.mapCollection).Add(item);
        }

        public void Clear()
        {
            ((IDictionary<IntVector2, T>)this.mapCollection).Clear();
        }

        public bool Contains(KeyValuePair<IntVector2, T> item)
        {
            return ((IDictionary<IntVector2, T>)this.mapCollection).Contains(item);
        }

        public void CopyTo(KeyValuePair<IntVector2, T>[] array, int arrayIndex)
        {
            ((IDictionary<IntVector2, T>)this.mapCollection).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<IntVector2, T> item)
        {
            return ((IDictionary<IntVector2, T>)this.mapCollection).Remove(item);
        }

        public IEnumerator<KeyValuePair<IntVector2, T>> GetEnumerator()
        {
            return ((IDictionary<IntVector2, T>)this.mapCollection).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<IntVector2, T>)this.mapCollection).GetEnumerator();
        }

        #endregion

    }

}
