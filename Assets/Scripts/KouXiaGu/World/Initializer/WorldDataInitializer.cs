using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World.Map;
using UniRx;
using UnityEngine;

namespace KouXiaGu.World
{

    /// <summary>
    /// 世界数据;
    /// </summary>
    public interface IWorldData
    {
        IGameData GameData { get; }
        WorldInfo Info { get; }
        TimeManager Time { get; }
        MapResource Map { get; }
    }


    /// <summary>
    /// 游戏世界数据初始化,游戏世界相关数据初始化;
    /// </summary>
    public class WorldDataInitializer : AsyncInitializer<IWorldData>, IWorldData
    {
        public IGameData GameData { get; private set; }
        public WorldInfo Info { get; private set; }
        public TimeManager Time { get; private set; }
        public MapResource Map { get; private set; }

        public override string Prefix
        {
            get { return "场景数据"; }
        }

        public IAsyncOperation<IWorldData> Start(IGameData gameData, WorldInfo info, IObservable<IWorld> starter)
        {
            StartInitialize();
            SetGameData(gameData);
            SetWorldInfo(info);
            BuildingData(starter);
            return this;
        }

        void SetGameData(IGameData gameData)
        {
            if (gameData == null)
                throw new ArgumentNullException("gameData");

            GameData = gameData;
        }

        void SetWorldInfo(WorldInfo info)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            Info = info;
        }

        void BuildingData(IObservable<IWorld> starter)
        {
            IAsyncOperation[] missions = new IAsyncOperation[]
              {
                  MapResource.ReadOrCreateAsync().Subscribe(OnMapResourceCompleted, OnFaulted),
                  TimeManager.Create(Info.Time, starter).Subscribe(OnTimeCompleted, OnFaulted),
              };
            (missions as IEnumerable<IAsyncOperation>).Subscribe(OnBuildingDataCompleted, OnFaulted);
        }

        void OnMapResourceCompleted(IAsyncOperation<MapResource> operation)
        {
            const string prefix = "[地图]";
            Map = operation.Result;
            Debug.Log(prefix + InitializationCompletedStr + " 总共有 " + Map.Data.Count + " 个节点;");
        }

        void OnTimeCompleted(IAsyncOperation<TimeManager> operation)
        {
            const string prefix = "[时间]";
            Time = operation.Result;
            Debug.Log(prefix + InitializationCompletedStr);
        }

        void OnBuildingDataCompleted(IList<IAsyncOperation> operations)
        {
            OnCompleted(operations, this);
        }

    }

}
