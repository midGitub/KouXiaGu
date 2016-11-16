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

        /// <summary>
        /// 若所有接口都保存完毕后,则将存档保存到磁盘;
        /// 若未保存成功,则也返回保存失败;
        /// </summary>
        internal void OnSavedComplete(ArchivedGroup archivedGroup, Action onSavedComplete)
        {
            try
            {
                archiveData.SaveInDisk(archivedGroup);
                onSavedComplete();
            }
            catch (Exception e)
            {
                Debug.LogWarning("归档时游戏失败!" + e);
            }
            onSavedComplete();
        }

    }

}
