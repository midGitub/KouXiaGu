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
    public class Landform : MonoBehaviour
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
                    Result = SceneObject.GetObject<Landform>();
                    Result.Initialize(worldData);
                    OnCompleted();
                }
                catch (Exception ex)
                {
                    OnFaulted(ex);
                }
            }

            public override bool IsCompleted
            {
                get { return IsFaulted || Result.LandformBuilder.Baker.IsEmpty; }
            }
        }


        Landform()
        {
        }

        public bool IsInitialized { get; private set; }
        public LandformManager LandformManager { get; private set; }
        public WorldMapWatcher MapWatcher { get; private set; }
        public WaterManager Water { get; private set; }

        public LandformBuilder LandformBuilder
        {
            get { return LandformManager.Manager; }
        }

        Landform Initialize(IWorldData worldData)
        {
            if (!IsInitialized)
            {
                LandformManager = new LandformManager(worldData);
                MapWatcher = new WorldMapWatcher(LandformBuilder, worldData.Map.PredefinedMap.Data);
                Water = SceneObject.GetObject<WaterManager>();
                Water.IsDisplay = true;
            }
            return this;
        }

        /// <summary>
        /// 获取到高度,若不存在高度信息,则返回0;
        /// </summary>
        public float GetHeight(Vector3 position)
        {
            return LandformBuilder.GetHeight(position);
        }

    }

}
