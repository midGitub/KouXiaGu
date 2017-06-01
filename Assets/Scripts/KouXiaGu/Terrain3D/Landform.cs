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
        public Landform()
        {
            Buildings = new SceneBuildingCollection();
        }

        public Landform(IBasicData basicData, IWorldData worldData)
        {
            //LandformBuilder = new LandformBuilder(worldData);
            //BuildingBuilder = new BuildingBuilder(worldData, this, LandformBuilder);
            LandformUpdater = new LandformUpdater();
            BuildingUpdater = new BuildingUpdater();
            WaterManager = new WaterManager();
            //MapWatcher = new MapWatcher(LandformBuilder, BuildingBuilder, worldData.MapData.ObservableMap);
        }

        public SceneBuildingCollection Buildings { get; private set; }

        public LandformBuilder LandformBuilder { get; private set; }
        public SceneBuildingCollection BuildingBuilder { get; private set; }
        public LandformUpdater LandformUpdater { get; private set; }
        public BuildingUpdater BuildingUpdater { get; private set; }
        public WaterManager WaterManager { get; private set; }
        //public MapWatcher MapWatcher { get; private set; }

        ///// <summary>
        ///// 开始初始化场景,返回值表示场景是否准备完毕;
        ///// </summary>
        //public IAsyncOperation StartBuildScene()
        //{
        //    return new SceneBuilder(this);
        //}

        //class SceneBuilder : AsyncOperation
        //{
        //    public SceneBuilder(Landform landform)
        //    {
        //        this.landform = landform;
        //        landform.LandformManager.StartUpdate();
        //        landform.BuildingManager.StartUpdate();
        //    }

        //    readonly Landform landform;

        //    public override bool IsCompleted
        //    {
        //        get { return landform.LandformBuilder.Baker.IsEmpty; }
        //    }
        //}


        /// <summary>
        /// 获取到高度,若不存在高度信息,则返回0;
        /// </summary>
        public float GetHeight(Vector3 position)
        {
            return LandformBuilder.GetHeight(position);
        }

        ///// <summary>
        ///// 重新创建所有;
        ///// </summary>
        //public void RebuildAll()
        //{
        //    LandformBuilder.DestroyAll();
        //    BuildingBuilder.DestroyAll();
        //}
    }

}
