using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;


namespace KouXiaGu.World
{

    /// <summary>
    /// 城镇管理;可序列化的;
    /// </summary>
    [XmlType("TownInfos")]
    public class TownManager
    {

        public TownManager()
        {
            towns = new Dictionary<int, Town>();
        }


        /// <summary>
        /// 城镇合集;
        /// </summary>
        Dictionary<int, Town> towns;


        /// <summary>
        /// 获取到城镇信息;
        /// </summary>
        public Town this[int townId]
        {
            get { return towns[townId]; }
        }


        /// <summary>
        /// 更新所有城镇的邻居信息;
        /// </summary>
        public void UpdateNeighbours()
        {
            throw new NotImplementedException();
        }


    }

}
