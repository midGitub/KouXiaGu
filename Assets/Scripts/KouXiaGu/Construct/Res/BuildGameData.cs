using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 构建游戏需要的数据;
    /// </summary>
    public struct BuildGameData
    {
        public BuildGameData(ArchivedGroup archivedData)
        {
            this.ArchivedData = archivedData;
        }

        public ArchivedGroup ArchivedData { get; private set; }

        public static implicit operator ArchivedGroup(BuildGameData item)
        {
            return item.ArchivedData;
        }
    }

}
