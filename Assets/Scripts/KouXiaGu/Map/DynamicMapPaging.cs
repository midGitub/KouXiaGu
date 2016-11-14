using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;

namespace KouXiaGu.Map
{
    /// <summary>
    /// 地图分页,地图块;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ProtoContract]
    public struct DynamicMapPaging<T> : IGameMap<ShortVector2, T>
    {
        public DynamicMapPaging(ShortVector2 address)
        {
            this.address = address;
            this.dictionary = new Dictionary<ShortVector2, T>();
        }

        public DynamicMapPaging(ShortVector2 address, Dictionary<ShortVector2, T> dictionary)
        {
            this.address = address;
            this.dictionary = dictionary;
        }

        /// <summary>
        /// 地图块的坐标;
        /// </summary>
        [ProtoMember(1)]
        private ShortVector2 address;

        /// <summary>
        /// 这个分页保存的信息;
        /// </summary>
        [ProtoMember(2)]
        internal Dictionary<ShortVector2, T> dictionary;

        /// <summary>
        /// 地图块的坐标;
        /// </summary>
        public ShortVector2 Address
        {
            get { return address; }
        }

        public T this[ShortVector2 key]
        {
            get { return this.dictionary[key]; }
            set { this.dictionary[key] = value; }
        }

        public int Count
        {
            get { return this.dictionary.Count; }
        }

        public bool IsEmpty
        {
            get { return dictionary.Count == 0; }
        }

        public void Add(ShortVector2 position, T item)
        {
            this.dictionary.Add(position, item);
        }

        public bool Remove(ShortVector2 position)
        {
            return this.dictionary.Remove(position);
        }

        public bool ContainsPosition(ShortVector2 position)
        {
            return this.dictionary.ContainsKey(position);
        }

        public bool TryGetValue(ShortVector2 position, out T item)
        {
            return this.dictionary.TryGetValue(position, out item);
        }

        public override string ToString()
        {
            return base.ToString() +
                "\n地图块地址:" + address.ToString() +
                "\n元素个数:" + dictionary.Count;
        }

        public override int GetHashCode()
        {
            return address.GetHashCode();
        }

    }
}
