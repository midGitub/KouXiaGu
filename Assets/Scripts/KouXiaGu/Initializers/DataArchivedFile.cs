using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏存档文件,保存内容主要为存档信息,能快速读取的信息;
    /// </summary>
    [ProtoContract, ProtoInclude(50, typeof(ArchivedExpand))]
    public class SmallArchived
    {

        /// <summary>
        /// 存档名;
        /// </summary>
        [ProtoMember(1)]
        public string Name { get; set; }

        /// <summary>
        /// 存档的描述;
        /// </summary>
        [ProtoMember(2)]
        public string Description { get; set; }

        #region 存档控制 10~20;

        /// <summary>
        /// 存档版本;
        /// </summary>
        [ProtoMember(10)]
        public float Version { get; set; }

        /// <summary>
        /// 这个游戏存档保存的真实时间;
        /// </summary>
        [ProtoMember(11)]
        public long SavedTime { get; set; }

        #endregion

    }

    /// <summary>
    /// 拓展的游戏存档,主要保存地图数据或其它需要大容量的数据信息;
    /// </summary>
    [ProtoContract]
    public class ArchivedExpand : SmallArchived
    {
        public ArchivedExpand() { }

        #region pathDictionary

        [ProtoMember(10)]
        private Dictionary<int, string> pathDictionary = new Dictionary<int, string>();

        /// <summary>
        /// 路径信息字典;
        /// </summary>
        public Dictionary<int, string> PathDictionary
        {
            get { return pathDictionary; }
        }

        /// <summary>
        /// 存档使用预制地图的路径;
        /// </summary>
        public const int PathPrefabMapDirectory = 10;

        #endregion


    }


    /// <summary>
    /// 基本数据的存储;
    /// </summary>
    [ProtoContract]
    public class DataDictionary
    {

        public DataDictionary()
        {
            LongDictionary = new Dictionary<string, long>();
            IntDictionary = new Dictionary<string, int>();
            ShortDictionary = new Dictionary<string, short>();
            ByteDictionary = new Dictionary<string, byte>();

            DoubleDictionary = new Dictionary<string, double>();
            FloatDictionary = new Dictionary<string, float>();

            BoolDictionary = new Dictionary<string, bool>();
            StringDictionary = new Dictionary<string, string>();
        }

        [ProtoMember(1)]
        public Dictionary<string, long> LongDictionary { get; private set; }
        [ProtoMember(2)]
        public Dictionary<string, int> IntDictionary { get; private set; }
        [ProtoMember(3)]
        public Dictionary<string, short> ShortDictionary { get; private set; }
        [ProtoMember(4)]
        public Dictionary<string, byte> ByteDictionary { get; private set; }

        [ProtoMember(10)]
        public Dictionary<string, double> DoubleDictionary { get; private set; }
        [ProtoMember(11)]
        public Dictionary<string, float> FloatDictionary { get; private set; }

        [ProtoMember(20)]
        public Dictionary<string, bool> BoolDictionary { get; private set; }
        [ProtoMember(21)]
        public Dictionary<string, string> StringDictionary { get; private set; }


        //public void Add(string value, int item)
        //{
        //    intDictionary.Add(value, item);
        //}
        //public void Add(string value, float item)
        //{
        //    floatDictionary.Add(value, item);
        //}
        //public void Add(string value, string item)
        //{
        //    stringDictionary.Add(value, item);
        //}
        //public void Add(string value, long item)
        //{
        //    longDictionary.Add(value, item);
        //}

        //public bool RemoveInt(string value)
        //{
        //    return intDictionary.Remove(value);
        //}
        //public bool RemoveFloat(string value)
        //{
        //    return floatDictionary.Remove(value);
        //}
        //public bool RemoveString(string value)
        //{
        //    return stringDictionary.Remove(value);
        //}

        //public int GetInt(string value)
        //{
        //    return intDictionary[value];
        //}
        //public float GetFloat(string value)
        //{
        //    return floatDictionary[value];
        //}
        //public string GetString(string value)
        //{
        //    return stringDictionary[value];
        //}

        //public bool ContainsInt(string value)
        //{
        //    return intDictionary.ContainsKey(value);
        //}
        //public bool ContainsIntFloat(string value)
        //{
        //    return floatDictionary.ContainsKey(value);
        //}
        //public bool ContainsIntString(string value)
        //{
        //    return stringDictionary.ContainsKey(value);
        //}

    }

}
