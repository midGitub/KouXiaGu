using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 地图存档信息;
    /// </summary>
    [ProtoContract]
    public class ArchivedMap
    {

        /// <summary>
        /// 存档使用预制地图的路径;
        /// </summary>
        [ProtoMember(10)]
        public string PathPrefabMapDirectory;

    }

}
