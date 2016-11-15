using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Map;
using ProtoBuf;
using UnityEngine;

namespace KouXiaGu.Characters
{

    /// <summary>
    /// 角色存档信息;
    /// </summary>
    [ProtoContract]
    public class ArchivedCharacter
    {

        /// <summary>
        /// 主角;
        /// </summary>
        [ProtoMember(100)]
        public MapObjectTransfrom ProtagonistTransfrom;

        /// <summary>
        /// 主角所在的位置;
        /// </summary>
        public Vector2 ProtagonistPosition
        {
            get { return ProtagonistTransfrom.Position; }
        }

    }

}
