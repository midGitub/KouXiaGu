using JiongXiaGu.Grids;
using JiongXiaGu.Unity;
using JiongXiaGu.Unity;
using JiongXiaGu.Unity.Initializers;
using JiongXiaGu.Unity.RectMaps;
using System.Threading;
using System.Threading.Tasks;
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
            Builder = new LandformBuilder(LandformBaker.Instance);
            Updater = new LandformUpdater(Builder, LandformGuiderGroup, RectMapSceneController.WorldMap);
            Debug.Log("[地貌组件]初始化完成;");
            return null;
        }

        void IRectTerrainUpdateHandle.TerrainUpdate()
        {
            Updater.Update();
        }
    }
}
