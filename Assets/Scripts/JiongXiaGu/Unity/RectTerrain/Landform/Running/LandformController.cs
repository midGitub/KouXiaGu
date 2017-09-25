using JiongXiaGu.Grids;
using JiongXiaGu.Unity;
using JiongXiaGu.World;
using JiongXiaGu.World.RectMap;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 管理地形组件的地貌;
    /// </summary>
    [DisallowMultipleComponent]
    public class LandformController : SceneSington<LandformController>, IComponentInitializeHandle, IRectTerrainUpdateHandle
    {
        LandformController()
        {
        }

        TerrainGuiderGroup<RectCoord> guiderGroup;
        public LandformBuilder Builder { get; private set; }
        public LandformUpdater Updater { get; private set; }

        public TerrainGuiderGroup<RectCoord> GuiderGroup
        {
            get { return guiderGroup != null ? guiderGroup : guiderGroup = new TerrainGuiderGroup<RectCoord>(); }
        }

        bool IRectTerrainUpdateHandle.IsCompleted
        {
            get { return LandformBaker.Instance.IsBakeComplete; }
        }

        void Awake()
        {
            SetInstance(this);
        }

        Task IComponentInitializeHandle.StartInitialize(CancellationToken token)
        {
            Builder = new LandformBuilder(LandformBaker.Instance);
            Updater = new LandformUpdater(Builder, GuiderGroup, RectMapDataInitializer.Instance.WorldMap);
            Debug.Log("[地貌组件]初始化完成;");
            return null;
        }

        void IRectTerrainUpdateHandle.TerrainUpdate()
        {
            Updater.Update();
        }
    }
}
