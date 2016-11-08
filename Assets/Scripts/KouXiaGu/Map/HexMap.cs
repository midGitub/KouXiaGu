using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

namespace KouXiaGu.Map
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


        public IEnumerable<KeyValuePair<IntVector2, T>> GetAround(IntVector2 position)
        {
            T item;
            IntVector2 vectorPosition;
            foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
            {
                vectorPosition = HexMapConvert.GetVector(position, direction) + position;
                if (mapCollection.TryGetValue(vectorPosition, out item))
                {
                    yield return new KeyValuePair<IntVector2, T>(vectorPosition, item);
                }
            }
        }


        #region IDictionary<IntVector2, T>

        public ICollection<IntVector2> Keys
        {
            get{ return this.mapCollection.Keys; }
        }

        public ICollection<T> Values
        {
            get{return this.mapCollection.Values;}
        }

        public int Count
        {
            get{ return this.mapCollection.Count; }
        }

        public bool IsReadOnly
        {
            get{ return (this.mapCollection as ICollection<KeyValuePair<IntVector2, T>>).IsReadOnly; }
        }

        public T this[IntVector2 key]
        {
            get{ return this.mapCollection[key]; }
            set{ this.mapCollection[key] = value; }
        }

        public void Add(IntVector2 key, T value)
        {
            this.mapCollection.Add(key, value);
        }

        public bool ContainsKey(IntVector2 key)
        {
            return this.mapCollection.ContainsKey(key);
        }

        public bool Remove(IntVector2 key)
        {
            return this.mapCollection.Remove(key);
        }

        public bool TryGetValue(IntVector2 key, out T value)
        {
            return this.mapCollection.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<IntVector2, T> item)
        {
            (this.mapCollection as ICollection<KeyValuePair<IntVector2,T>>).Add(item);
        }

        public void Clear()
        {
            this.mapCollection.Clear();
        }

        public bool Contains(KeyValuePair<IntVector2, T> item)
        {
            return this.mapCollection.Contains(item);
        }

        public void CopyTo(KeyValuePair<IntVector2, T>[] array, int arrayIndex)
        {
            (this.mapCollection as ICollection<KeyValuePair<IntVector2, T>>).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<IntVector2, T> item)
        {
            return (this.mapCollection as ICollection<KeyValuePair<IntVector2, T>>).Remove(item);
        }

        public IEnumerator<KeyValuePair<IntVector2, T>> GetEnumerator()
        {
            return this.mapCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.mapCollection.GetEnumerator();
        }

        #endregion


        public override string ToString()
        {
            string str = base.ToString() + 
                "\n元素个数:" + mapCollection.Count;
            return str;
        }


    }

}
