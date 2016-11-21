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
        public BuildGameData(DataCore coreData, ModGroup modData, ArchivedGroup archivedData)
        {
            this.CoreData = coreData;
            this.ModData = modData;
            this.ArchivedData = archivedData;
        }

        public DataCore CoreData { get; private set; }

        public ModGroup ModData { get; private set; }

        public ArchivedGroup ArchivedData { get; private set; }

        public static implicit operator DataCore(BuildGameData item)
        {
            return item.CoreData;
        }

        public static implicit operator ModGroup(BuildGameData item)
        {
            return item.ModData;
        }
        public static implicit operator ArchivedGroup(BuildGameData item)
        {
            return item.ArchivedData;
        }
    }

}
