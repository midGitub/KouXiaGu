//using KouXiaGu.Terrain3D;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace KouXiaGu.World
//{

//    /// <summary>
//    /// 场景构建;
//    /// </summary>
//    public class WorldUpdater : AsyncInitializer, IWorldUpdater
//    {
//        public WorldUpdater()
//        {
//            LandformUpdater = new SceneLandformUpdater();
//        }

//        public IWorld World{ get; private set; }
//        public SceneLandformUpdater LandformUpdater { get; private set; }

//        public override string Prefix
//        {
//            get { return "等待世界场景构建完成;"; }
//        }

//        public IAsyncOperation Start(IWorld world)
//        {
//            World = world;
//            StartInitialize();
//            BuildingScene();
//            return this;
//        }

//        /// <summary>
//        /// 初始化游戏场景;
//        /// </summary>
//        void BuildingScene()
//        {
//            IAsyncOperation[] missions = new IAsyncOperation[]
//              {
//                  LandformUpdater.Start(World).SubscribeFaulted("等待地形组件初始化", OnFaulted),
//              };
//            (missions as IEnumerable<IAsyncOperation>).Subscribe("等待场景初始化完毕", OnCompleted, OnFaulted);
//        }

//        public void Dispose()
//        {
//            LandformUpdater.Dispose();
//        }
//    }
//}
