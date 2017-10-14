using JiongXiaGu.Unity.Archives;
using JiongXiaGu.Unity.Initializers;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 地图场景控制器;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class RectMapSceneController : MonoBehaviour, ISceneDataInitializeHandle
    {
        /// <summary>
        /// 当前场景使用的地图;
        /// </summary>
        public static WorldMap WorldMap { get; private set; }

        Task ISceneDataInitializeHandle.Initialize(SceneArchivalData archivalData, CancellationToken token)
        {
            return Task.Run(delegate ()
            {
                MapSceneArchivalData mapSceneArchivalData = archivalData.Get<MapSceneArchivalData>();

                if (mapSceneArchivalData == null)
                    throw new ArgumentException(string.Format("{0}未定义地图数据;", nameof(archivalData)));

                WorldMap = mapSceneArchivalData.CreateMap();
                OnCompleted();
            });
        }

        Task ISceneDataInitializeHandle.Read(IArchiveFileInfo archive, SceneArchivalData archivalData, CancellationToken cancellationToken)
        {
            return Task.Run(delegate ()
            {
                MapSceneArchivalData sceneArchivalData = new MapSceneArchivalData(archive);
                archivalData.Add(sceneArchivalData);
            });
        }

        private void OnDestroy()
        {
            WorldMap = null;
        }

        [System.Diagnostics.Conditional("EDITOR_LOG")]
        private void OnCompleted()
        {
            const string log = "[地图资源]初始化完成;\n";
            string Info = GetInfoLog();
            Debug.Log(log + Info);
        }

        private string GetInfoLog()
        {
            string log = "地图名:" + WorldMap.MapData.Name
                + ", 地图节点数目:" + WorldMap.Map.Count;
            return log;
        }

        [ContextMenu("报告详细信息")]
        private void LogInfo()
        {
            Debug.Log(GetInfoLog());
        }
    }
}
