using System;
using System.Collections.Generic;
using KouXiaGu.World;
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
                    OnCompleted(Result);
                }
                catch (Exception ex)
                {
                    OnFaulted(ex);
                }
            }
        }

        public Landform(IWorldData worldData)
        {
            LandformManager = new LandformManager(worldData);
            BuildingManager = new BuildingManager(worldData, this, LandformManager.Builder);
            WaterManager = new WaterManager();
            MapWatcher = new MapWatcher(LandformBuilder, worldData.MapData.ObservableMap);
        }

        public LandformManager LandformManager { get; private set; }
        public BuildingManager BuildingManager { get; private set; }
        public WaterManager WaterManager { get; private set; }
        public MapWatcher MapWatcher { get; private set; }

        public LandformBuilder LandformBuilder
        {
            get { return LandformManager.Builder; }
        }

        public BuildingBuilder BuildingBuilder
        {
            get { return BuildingManager.Builder; }
        }

        /// <summary>
        /// 开始初始化场景,返回值表示场景是否准备完毕;
        /// </summary>
        public IAsyncOperation StartBuildScene()
        {
            return new SceneBuilder(this);
        }

        class SceneBuilder : AsyncOperation
        {
            public SceneBuilder(Landform landform)
            {
                this.landform = landform;
                landform.LandformManager.StartUpdate();
                landform.BuildingManager.StartUpdate();
            }

            readonly Landform landform;

            public override bool IsCompleted
            {
                get { return landform.LandformBuilder.Baker.IsEmpty; }
            }
        }


        /// <summary>
        /// 获取到高度,若不存在高度信息,则返回0;
        /// </summary>
        public float GetHeight(Vector3 position)
        {
            return LandformBuilder.GetHeight(position);
        }

        /// <summary>
        /// 重新创建所有;
        /// </summary>
        public void RebuildAll()
        {
            LandformBuilder.DestroyAll();
            BuildingBuilder.DestroyAll();
        }
    }

}
