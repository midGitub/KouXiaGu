using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace KouXiaGu.World2D
{

    [ProtoContract]
    public class ArchivedWorld2D
    {

        /// <summary>
        /// 存档使用预制地图的路径;
        /// </summary>
        [ProtoMember(10)]
        public string PathPrefabMapDirectory;

    }

}
