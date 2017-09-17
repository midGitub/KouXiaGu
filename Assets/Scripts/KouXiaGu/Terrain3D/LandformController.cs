using KouXiaGu.Grids;
using KouXiaGu.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 游戏地形控制;负责地形的更新和创建;
    /// </summary>
    [DisallowMultipleComponent]
    public class LandformController : SceneSington<LandformController>
    {

        /// <summary>
        /// unity线程执行;
        /// </summary>
        [SerializeField]
        LandformUnityDispatcher landformUnityDispatcher;

        public LandformBuilder LandformBuilder { get; private set; }
        public BuildingBuilder BuildingBuilder { get; private set; }
        public ChunkUpdater<RectCoord, LandformChunk> LandformUpdater { get; private set; }
        public ChunkUpdater<CubicHexCoord, BuildingUnit> BuildingUpdater { get; private set; }

        void Awake()
        {
            SetInstance(this);
        }

        public void WorldDataCompleted(IWorldData data)
        {
            LandformBuilder = new LandformBuilder(data, landformUnityDispatcher);
            BuildingBuilder = new BuildingBuilder(data, landformUnityDispatcher);

            LandformUpdater = new ChunkUpdater<RectCoord, LandformChunk>(LandformBuilder, new GuiderGroup<RectCoord>.GuiderGroup_List());
            BuildingUpdater = new ChunkUpdater<CubicHexCoord, BuildingUnit>(BuildingBuilder, new GuiderGroup<CubicHexCoord>.GuiderGroup_HashSet());
        }
    }
}
