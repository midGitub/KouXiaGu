using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏数据静态类;
    /// </summary>
    public static class GameData
    {

        private static IBuildGameData buildGameData;

        /// <summary>
        /// 获取到游戏创建接口;
        /// </summary>
        public static IBuildGameData BuildGameData
        {
            get { return buildGameData ?? (buildGameData = GetBuildGameData()); }
        }

        /// <summary>
        /// 获取到所有资源目录路径;
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> EnumerableDirectoryPath()
        {
            yield return BuildGameData.CoreData.GetCoreDataDirectoryPath();

            IEnumerable<string> directoryPaths = BuildGameData.ModData.GetModInfos().Keys;
            foreach (var item in directoryPaths)
            {
                yield return item;
            }
        }

        private static IBuildGameData GetBuildGameData()
        {
            return GameObject.FindWithTag("GameController").GetComponent<Initializers>();
        }

    }

}
