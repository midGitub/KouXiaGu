using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace KouXiaGu.World2D
{

    [ProtoContract]
    public class ArchivedTime
    {

        /// <summary>
        /// 游戏已经经过了的时间;
        /// </summary>
        [ProtoMember(10)]
        public ulong Time;

    }

}
