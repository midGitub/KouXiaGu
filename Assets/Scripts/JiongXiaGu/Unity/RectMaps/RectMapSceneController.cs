using JiongXiaGu.Unity.Archives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;
using JiongXiaGu.Unity.Resources;
using JiongXiaGu.Grids;
using JiongXiaGu.Unity.Initializers;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 地图场景控制器;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class RectMapSceneController : SceneSington<RectMapSceneController>, ISceneDataInitializeHandle
    {
        RectMapSceneController()
        {
        }

        /// <summary>
        /// 游戏地图存放目录;
        /// </summary>
        [PathDefinition(ResourceTypes.DataDirectory, "游戏地图存放目录;")]
        internal const string MapsDirectoryName = "Maps";

        /// <summary>
        /// 默认地图(在不存在存档时通过此变量初始化,可以手动指定);
        /// </summary>
        public static Map DefaultMap { get; set; } 

        /// <summary>
        /// 是否正在进行存档操作?
        /// </summary>
        public static bool IsArchiving { get; private set; }

        /// <summary>
        /// 当前场景使用的地图;
        /// </summary>
        public static WorldMap WorldMap { get; private set; }

        Task ISceneDataInitializeHandle.Initialize(SceneArchivalData archivalData, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        void ISceneArchiveHandle.Prepare(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task ISceneArchiveHandle.Collect(SceneArchivalData archivalData, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task ISceneArchiveHandle.Read(IArchiveFileInfo archive, SceneArchivalData archivalData, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从地图构造器获取到地图;
        /// </summary>
        WorldMap ReadMapFromMemory(CancellationToken token)
        {
            WorldMap worldMap = new WorldMap(DefaultMap);
            return worldMap;
        }

        /// <summary>
        /// 从存档获取到地图;
        /// </summary>
        WorldMap ReadMapFromArchive(Archive archive, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        [System.Diagnostics.Conditional("EDITOR_LOG")]
        void OnCompleted()
        {
            const string prefix = "[地图资源]";
            string info = "[地图:Size:" + WorldMap.Map.Count + "]";
            Debug.Log(prefix + "初始化完成;" + info);
        }
    }
}
