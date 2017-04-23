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
    public class DataInitializer : IGameData
    {

        public static IAsyncOperation<DataInitializer> CreateAsync()
        {
            return new Initializer();
        }

        DataInitializer()
        {
        }

        /// <summary>
        /// 基础信息;
        /// </summary>
        public WorldElementResource ElementInfo { get; private set; }

        /// <summary>
        /// 地形资源;
        /// </summary>
        public TerrainResource Terrain { get; private set; }


        /// <summary>
        /// 游戏文件资源初始化;
        /// </summary>
        class Initializer : AsyncInitializer<DataInitializer>
        {
            public Initializer()
            {
                data = new DataInitializer();
                Initialize0();
            }

            DataInitializer data;

            public override string Prefix
            {
                get { return "世界基础资源"; }
            }

            void InitializeCompleted(IList<IAsyncOperation> operations)
            {
                OnCompleted(operations, data);
            }

            void Initialize0()
            {
                StartInitialize();
                WorldElementResource.ReadAsync().Subscribe(OnWorldResourceCompleted, OnFaulted);
            }

            void OnWorldResourceCompleted(IAsyncOperation<WorldElementResource> operation)
            {
                data.ElementInfo = operation.Result;
                string log = GetWorldResourceLog(data.ElementInfo);
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
                    TerrainResource.ReadAsync(data.ElementInfo).Subscribe(OnTerrainCompleted, OnFaulted),
                };
                (missions as IEnumerable<IAsyncOperation>).Subscribe(InitializeCompleted, OnFaulted);
            }


            void OnTerrainCompleted(IAsyncOperation<TerrainResource> operation)
            {
                data.Terrain = operation.Result;
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

}
