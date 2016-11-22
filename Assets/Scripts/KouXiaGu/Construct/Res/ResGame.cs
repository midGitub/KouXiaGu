using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    public class ResGame
    {

        /// <summary>
        /// 根据当前状态获取到创建游戏所需的资源信息;
        /// </summary>
        public static BuildGameData GetBuildGameData()
        {
            ArchivedGroup archivedGroup = ResArchiver.CreateArchived();
            BuildGameData buildGameData = new BuildGameData(archivedGroup);
            return buildGameData;
        }

        /// <summary>
        /// 根据最新的存档获取到构建游戏资源;
        /// </summary>
        public static BuildGameData GetRecentBuildGameData()
        {
            ArchivedGroup archivedGroup = ResArchiver.GetRecentArchivedGroup();
            BuildGameData buildGameData = new BuildGameData(archivedGroup);
            return buildGameData;
        }

        /// <summary>
        /// 根据这个存档获取到创建游戏所需的资源信息;
        /// </summary>
        public static BuildGameData GetBuildGameData(ArchivedGroup archivedGroup)
        {
            BuildGameData buildGameData = new BuildGameData(archivedGroup);
            return buildGameData;
        }

    }

}
