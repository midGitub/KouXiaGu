using System;
using KouXiaGu.Terrain3D;
using KouXiaGu.World;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu
{

    public interface IGameData
    {
        TerrainResource Terrain { get; }
    }

    /// <summary>
    /// 游戏数据,开始游戏前需要读取的资源;
    /// </summary>
    [Serializable]
    public class GameDataInitializer : AsyncInitializer<IGameData>, IGameData
    {
        public GameDataInitializer()
        {
        }

        [SerializeField]
        TerrainResource terrain;

        public WorldElementResource ElementInfo { get; private set; }

        public TerrainResource Terrain
        {
            get { return terrain; }
        }

        public override string Prefix
        {
            get { return "游戏基础资源"; }
        }

        public void Start()
        {
            StartInitialize();
            Initialize0();
        }

        void InitializeCompleted(IList<IAsyncOperation> operations)
        {
            OnCompleted(operations, this);
        }

        void Initialize0()
        {
            WorldElementResource.ReadAsync().Subscribe(this, OnWorldResourceCompleted, OnFaulted);
        }

        void OnWorldResourceCompleted(IAsyncOperation<WorldElementResource> operation)
        {
            ElementInfo = operation.Result;
            string log = GetWorldResourceLog(ElementInfo);
            Debug.Log(log);
            Initialize1();
        }

        string GetWorldResourceLog(WorldElementResource item)
        {
            string str =
                "[基础资源]"
               + "\nLandform:" + item.LandformInfos.Count
               + "\nRoad:" + item.RoadInfos.Count
               + "\nBuilding:" + item.BuildingInfos.Count
               + "\nProduct:" + item.ProductInfos.Count;
            return str;
        }

        void Initialize1()
        {
            IAsyncOperation[] missions = new IAsyncOperation[]
            {
                    terrain.Init(ElementInfo).Subscribe(this, OnTerrainCompleted, OnFaulted),
            };
            (missions as IEnumerable<IAsyncOperation>).Subscribe(this, InitializeCompleted, OnFaulted);
        }


        void OnTerrainCompleted(IAsyncOperation operation)
        {
            string log = GetTerrainResourceLog(terrain);
            Debug.Log(log);
        }

        string GetTerrainResourceLog(TerrainResource item)
        {
            string str =
                "[地形资源]"
               + "\nLandform:" + item.LandformInfos.Count
               + "\nRoad:" + item.RoadInfos.Count;
            return str;
        }

    }

}
