using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏存档文件,保存内容主要为存档信息,能快速读取的信息;
    /// </summary>
    [ProtoContract, ProtoInclude(1, typeof(ArchiveExpand))]
    public class Archive
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

        /// <summary>
        /// 这个游戏存档保存的真实时间;
        /// </summary>
        [ProtoMember(3)]
        public long SavedTime { get; set; }


    }

    /// <summary>
    /// 拓展的游戏存档,主要保存地图数据或其它需要大容量的数据信息;
    /// </summary>
    [ProtoContract]
    public class ArchiveExpand : Archive
    {

    }

}
