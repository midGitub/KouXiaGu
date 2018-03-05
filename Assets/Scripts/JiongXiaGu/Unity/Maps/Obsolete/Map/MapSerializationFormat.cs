//using System;
//using System.Collections.Generic;
//using System.Collections;
//using System.Xml.Serialization;

//namespace JiongXiaGu.Unity.Maps
//{

//    /// <summary>
//    /// 提供序列化的格式,因为 System.Xml.Serialization 无法序列化 IDictionary<TKey, TValue> 接口;
//    /// </summary>
//    [XmlRoot("Map")]
//    public struct MapSerializationFormat<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
//    {
//        public IDictionary<TKey, TValue> Dictionary { get; private set; }

//        public MapSerializationFormat(IDictionary<TKey, TValue> dictionary)
//        {
//            Dictionary = dictionary;
//        }

//        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
//        {
//            return Dictionary != null ? Dictionary.GetEnumerator() : new List<KeyValuePair<TKey, TValue>>(0).GetEnumerator();
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return GetEnumerator();
//        }
//    }
//}
