using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World;
using KouXiaGu.World.Map;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形控制;
    /// </summary>
    public class Landform
    {
        /// <summary>
        /// 初始化,并且在等待地形初始化完毕;
        /// </summary>
        public static IAsyncOperation<Landform> InitializeAsync(IWorldData worldData)
        {
            return new AsyncInitializer(worldData);
        }

        class AsyncInitializer : AsyncOperation<Landform>
        {
            public AsyncInitializer(IWorldData worldData)
            {
                try
                {
                    Result = new Landform(worldData);
                    OnCompleted();
                }
                catch (Exception ex)
                {
                    OnFaulted(ex);
                }
            }

            public override bool IsCompleted
            {
                get { return IsFaulted || (isCompleted && Result.LandformBuilder.Baker.IsEmpty); }
            }
        }


        Landform(IWorldData worldData)
        {
            LandformManager = new LandformManager(worldData);
            BuildingManager = new BuildingManager(worldData, LandformManager);

            MapWatcher = new WorldMapWatcher(LandformBuilder, worldData.Map.PredefinedMap.Data);
            Water = SceneObject.GetObject<WaterManager>();
            Water.IsDisplay = true;
        }

        public bool IsInitialized { get; private set; }
        public LandformManager LandformManager { get; private set; }
        public BuildingManager BuildingManager { get; private set; }

        public WorldMapWatcher MapWatcher { get; private set; }
        public WaterManager Water { get; private set; }

        public LandformBuilder LandformBuilder
        {
            get { return LandformManager.Builder; }
        }

        public BuildingBuilder BuildingBuilder
        {
            get { return BuildingManager.Builder; }
        }

        /// <summary>
        /// 获取到高度,若不存在高度信息,则返回0;
        /// </summary>
        [Obsolete]
        public float GetHeight(Vector3 position)
        {
            return LandformBuilder.GetHeight(position);
        }
    }

}
