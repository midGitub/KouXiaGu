using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 游戏地图;
    /// </summary>
    [DisallowMultipleComponent]
    public class WorldMap2D : MonoBehaviour, IStartGameEvent, IArchiveEvent, IQuitGameEvent
    {

        /// <summary>
        /// 保存到存档的位置;
        /// </summary>
        [SerializeField]
        private string archivedDirectoryName;
        /// <summary>
        /// 每个地图块的大小;
        /// </summary>
        [SerializeField]
        ShortVector2 partitionSizes;
        /// <summary>
        /// 地图读取的范围;
        /// </summary>
        [SerializeField]
        IntVector2 loadRang;

        internal BlockLoader<WorldNode, MapBlock<WorldNode>> mapCollection;
        [SerializeField]
        UseMapBlockIO mapBlockIO;

        public IMap<IntVector2, WorldNode> Map { get { return mapCollection; } }

        void Awake()
        {
            mapCollection = new BlockLoader<WorldNode, MapBlock<WorldNode>>(partitionSizes, loadRang, mapBlockIO);
        }


        /// <summary>
        /// 开始游戏时调用;
        /// </summary>
        IEnumerator IConstruct<BuildGameData>.Construction(BuildGameData item)
        {
            string fullArchivedDirectoryPath = GetFullArchivedDirectoryPath(item);
            string fullPrefabMapDirectoryPath = item.ArchivedData.Archived.World2D.PathPrefabMapDirectory;

            mapBlockIO.OnBulidGame(fullArchivedDirectoryPath, fullPrefabMapDirectoryPath);

            mapCollection.UpdateCenterPoint(new IntVector2(0, 0));

            yield break;
        }

        /// <summary>
        /// 获取到完整的归档地图文件夹路径;
        /// </summary>
        private string GetFullArchivedDirectoryPath(ArchivedGroup item)
        {
            string fullArchivedDirectoryPath = Path.Combine(item.ArchivedPath, archivedDirectoryName);
            return fullArchivedDirectoryPath;
        }

        IEnumerator IConstruct<ArchivedGroup>.Construction(ArchivedGroup item)
        {
            string fullArchivedDirectoryPath = GetFullArchivedDirectoryPath(item);
            mapBlockIO.OnGameArchive(fullArchivedDirectoryPath, mapCollection);
            item.Archived.World2D.PathPrefabMapDirectory = mapBlockIO.FullPrefabMapDirectoryPath;
            yield break;
        }

        IEnumerator IConstruct<QuitGameData>.Construction(QuitGameData item)
        {
            Map.Clear();
            yield break;
        }

        [Serializable]
        private class UseMapBlockIO : MapBlockIO<WorldNode> { }

    }

}
