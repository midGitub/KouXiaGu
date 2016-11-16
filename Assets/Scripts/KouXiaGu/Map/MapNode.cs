﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace KouXiaGu.Map
{

    [ProtoContract]
    public class MapNode
    {
        public MapNode()
        {

        }

        /// <summary>
        /// 地貌;
        /// </summary>
        [ProtoMember(1)]
        public int landform { get; set; }



    }

}
