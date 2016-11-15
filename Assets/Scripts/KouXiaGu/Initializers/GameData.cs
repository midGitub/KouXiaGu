using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏数据静态拓展类;
    /// </summary>
    public static class GameData
    {

        /// <summary>
        /// 获取到所有资源目录路径;
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> EnumerableDirectoryPath(this IBuildGameData item)
        {
            yield return item.CoreData.GetCoreDataDirectoryPath();

            IEnumerable<string> directoryPaths = item.ModData.GetModInfos().Keys;
            foreach (var irectoryPath in directoryPaths)
            {
                yield return irectoryPath;
            }
        }

    }

}
