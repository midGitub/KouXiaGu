using JiongXiaGu.Grids;
using JiongXiaGu.Unity.Initializers;
using JiongXiaGu.Unity.RectMaps;
using System.Threading;
using System.Threading.Tasks;
using System;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 管理地形组件的地貌;
    /// </summary>
    [DisallowMultipleComponent]
    public class LandformController : SceneSington<LandformController>, ISceneComponentInitializeHandle, IRectTerrainUpdateHandle
    {
        LandformController()
        {
        }

        TerrainGuiderGroup<RectCoord> landformGuiderGroup;
        public LandformBuilder Builder { get; private set; }
        public LandformUpdater Updater { get; private set; }

        public TerrainGuiderGroup<RectCoord> LandformGuiderGroup
        {
            get { return landformGuiderGroup != null ? landformGuiderGroup : landformGuiderGroup = new TerrainGuiderGroup<RectCoord>(); }
        }

        bool IRectTerrainUpdateHandle.IsCompleted
        {
            get { return LandformBaker.Instance.IsBakeComplete; }
        }

        Task ISceneComponentInitializeHandle.Initialize(CancellationToken token)
        {
            try
            {
                var landformBaker = LandformBaker.Instance;
                var worldMap = RectMapSceneController.WorldMap;
                var rectTerrainResources = RectTerrainResourcesInitializer.RectTerrainResources;

                if (landformBaker == null)
                    return Task.FromException(new ArgumentNullException(nameof(landformBaker)));
                if (worldMap == null)
                    return Task.FromException(new ArgumentNullException(nameof(worldMap)));
                if (rectTerrainResources == null)
                    return Task.FromException(new ArgumentNullException(nameof(rectTerrainResources)));

                landformBaker.Initialize(rectTerrainResources, worldMap.Map);
                Builder = new LandformBuilder(landformBaker);
                Updater = new LandformUpdater(Builder, LandformGuiderGroup, worldMap);
                Debug.Log("[地貌组件]初始化完成;");
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        void IRectTerrainUpdateHandle.TerrainUpdate()
        {
            Updater.Update();
        }
    }
}
