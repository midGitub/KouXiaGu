//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using KouXiaGu.World.Map;
//using UniRx;
//using UnityEngine;
//using KouXiaGu.Resources;

//namespace KouXiaGu.World
//{

//    /// <summary>
//    /// 游戏世界数据初始化,游戏世界相关数据初始化;
//    /// </summary>
//    public class DataInitializer : AsyncInitializer<IWorldData>, IWorldData
//    {
//        public BasicResource BasicData { get; private set; }
//        public WorldInfo Info { get; private set; }
//        public TimeManager Time { get; private set; }
//        public GameMap MapData { get; private set; }

//        public override string Prefix
//        {
//            get { return "场景数据"; }
//        }

//        public IAsyncOperation<IWorldData> Start(BasicResource gameData, WorldInfo info, IObservable<IWorld> starter)
//        {
//            StartInitialize();
//            SetGameData(gameData);
//            SetWorldInfo(info);
//            BuildingData(starter);
//            return this;
//        }

//        void SetGameData(BasicResource gameData)
//        {
//            if (gameData == null)
//                throw new ArgumentNullException("gameData");

//            BasicData = gameData;
//        }

//        void SetWorldInfo(WorldInfo info)
//        {
//            if (info == null)
//                throw new ArgumentNullException("info");

//            Info = info;
//        }

//        void BuildingData(IObservable<IWorld> starter)
//        {
//            IAsyncOperation[] missions = new IAsyncOperation[]
//              {
//                  new ThreadDelegateOperation<GameMap>(() => Info.MapReader.Read(BasicData)).Subscribe(this, OnMapResourceCompleted, OnFaulted),
//                  TimeManager.Create(Info.Time, starter).Subscribe(this, OnTimeCompleted, OnFaulted),
//              };
//            (missions as IEnumerable<IAsyncOperation>).Subscribe(this, OnBuildingDataCompleted, OnFaulted);
//        }

//        void OnMapResourceCompleted(IAsyncOperation<GameMap> operation)
//        {
//            const string prefix = "[地图]";
//            MapData = operation.Result;
//            Debug.Log(prefix + InitializationCompletedStr + " 总共有 " + MapData.Map.Count + " 个节点;");
//        }

//        void OnTimeCompleted(IAsyncOperation<TimeManager> operation)
//        {
//            const string prefix = "[时间]";
//            Time = operation.Result;
//            Debug.Log(prefix + InitializationCompletedStr);
//        }

//        void OnBuildingDataCompleted(IList<IAsyncOperation> operations)
//        {
//            OnCompleted(operations, this);
//        }

//    }

//}
