using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using KouXiaGu.World;
using KouXiaGu.World.Map;
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
    public class DataInitializer : AsyncInitializer<IGameData>, IGameData
    {
        public DataInitializer()
        {
        }

        public WorldElementResource ElementInfo { get; private set; }
        public TerrainResource Terrain { get; private set; }

        public override string Prefix
        {
            get { return "游戏基础资源"; }
        }

        public override void Start()
        {
            base.Start();
            Initialize0();
        }

        void InitializeCompleted(IList<IAsyncOperation> operations)
        {
            OnCompleted(operations, this);
        }

        void Initialize0()
        {
            base.Start();
            WorldElementResource.ReadAsync().Subscribe(OnWorldResourceCompleted, OnFaulted);
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
                    TerrainResource.ReadAsync(ElementInfo).Subscribe(OnTerrainCompleted, OnFaulted),
            };
            (missions as IEnumerable<IAsyncOperation>).Subscribe(InitializeCompleted, OnFaulted);
        }


        void OnTerrainCompleted(IAsyncOperation<TerrainResource> operation)
        {
            Terrain = operation.Result;
            string log = GetTerrainResourceLog(operation.Result);
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
