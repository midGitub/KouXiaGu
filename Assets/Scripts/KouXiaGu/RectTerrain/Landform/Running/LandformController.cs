using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using KouXiaGu.World.RectMap;
using KouXiaGu.Grids;
using KouXiaGu.World;

namespace KouXiaGu.RectTerrain
{

    /// <summary>
    /// 管理地形组件的地貌;
    /// </summary>
    [DisallowMultipleComponent]
    public class LandformController : MonoBehaviour, IComponentInitializer
    {
        LandformController()
        {
        }

        [SerializeField]
        RectMapDataInitializer rectMapData;
        [SerializeField]
        LandformBaker baker;

        [SerializeField, Range(0, 64)]
        float tessellation = 48f;
        [SerializeField, Range(0, 5)]
        float displacement = 1.5f;

        TerrainGuiderGroup<RectCoord> guiderGroup;
        public LandformBuilder Builder { get; private set; }
        public LandformUpdater Updater { get; private set; }

        /// <summary>
        /// 地形质量设置;
        /// </summary>
        public LandformQuality Quality
        {
            get { return baker.BakeCamera.Quality; }
        }

        /// <summary>
        /// 地形细分程度;
        /// </summary>
        public float Tessellation
        {
            get { return tessellation; }
        }

        /// <summary>
        /// 地形高度缩放;
        /// </summary>
        public float Displacement
        {
            get { return displacement; }
        }

        /// <summary>
        /// 地形烘培器;
        /// </summary>
        public LandformBaker Baker
        {
            get { return baker; }
        }

        /// <summary>
        /// 是否现在所有地形已经创建完成;
        /// </summary>
        public bool IsBakeComplete
        {
            get { return baker.RequestDispatcher.Count <= 0; }
        }

        public TerrainGuiderGroup<RectCoord> GuiderGroup
        {
            get { return guiderGroup != null ? guiderGroup : guiderGroup = new TerrainGuiderGroup<RectCoord>(); }
        }

        void Awake()
        {
            OnValidate();
        }

        void OnValidate()
        {
            LandformChunkRenderer.SetTessellation(tessellation);
            LandformChunkRenderer.SetDisplacement(displacement);
        }

        Task IComponentInitializer.StartInitialize(CancellationToken token)
        {
            Builder = new LandformBuilder(baker);
            Updater = new LandformUpdater(Builder, GuiderGroup, rectMapData.WorldMap);
            Debug.Log("[地貌组件]初始化完成;");
            return null;
        }
    }
}
