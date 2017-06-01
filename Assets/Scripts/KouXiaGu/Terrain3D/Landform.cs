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
            LandformChunks = new SceneLandformCollection();
            Buildings = new SceneBuildingCollection();
            WaterManager = new WaterManager();
        }

        internal SceneLandformCollection LandformChunks { get; private set; }
        internal SceneBuildingCollection Buildings { get; private set; }
        public WaterManager WaterManager { get; private set; }

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
            return LandformChunks.GetHeight(position);
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
