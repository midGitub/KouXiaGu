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

        private RectMapSceneController()
        {
        }

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
                MapSceneArchivalData sceneArchivalData = MapSceneArchivalData.Create(archive);
                archivalData.Add(sceneArchivalData);
            });
        }

        private void OnDestroy()
        {
            WorldMap = null;
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
