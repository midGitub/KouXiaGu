using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    [Serializable]
    public class DataGame
    {

        [SerializeField]
        private DataCore coreData;

        [SerializeField]
        private DataMod modData;

        [SerializeField]
        private DataArchive archiveData;


        public DataArchive ArchiveData
        {
            get { return archiveData; }
        }
        public DataCore CoreData
        {
            get { return coreData; }
        }
        public DataMod ModData
        {
            get { return modData; }
        }


        /// <summary>
        /// 根据当前状态获取到创建游戏所需的资源信息;
        /// </summary>
        public BuildGameData GetBuildGameData()
        {
            DataCore coreDataResource = coreData;
            ModGroup modRes = modData.GetModGroup();
            ArchivedGroup archivedGroup = archiveData.CreateArchived();
            BuildGameData buildGameData = new BuildGameData(coreDataResource, modRes, archivedGroup);
            return buildGameData;
        }

        /// <summary>
        /// 根据这个存档获取到创建游戏所需的资源信息;
        /// </summary>
        public BuildGameData GetBuildGameData(ArchivedGroup archivedGroup)
        {
            DataCore coreDataResource = coreData;
            ModGroup modRes = modData.GetModGroup();
            BuildGameData buildGameData = new BuildGameData(coreDataResource, modRes, archivedGroup);
            return buildGameData;
        }

    }

}
