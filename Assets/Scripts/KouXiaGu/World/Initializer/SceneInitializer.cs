using KouXiaGu.Terrain3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{


    public class SceneInitializer : AsyncInitializer
    {
        public override string Prefix
        {
            get { return "等待世界场景构建完成;"; }
        }

        IWorld world;

        Landform landform
        {
            get { return world.Component.Landform; }
        }

        public IAsyncOperation Start(IWorld world)
        {
            this.world = world;
            StartInitialize();
            BuildingScene();
            return this;
        }

        /// <summary>
        /// 初始化游戏场景;
        /// </summary>
        void BuildingScene()
        {
            IAsyncOperation[] missions = new IAsyncOperation[]
              {
                  landform.StartBuildScene().SubscribeFaulted("等待地形组件初始化", OnFaulted),
              };
            (missions as IEnumerable<IAsyncOperation>).Subscribe("等待场景组件初始化", OnCompleted, OnFaulted);
        }
    }
}
