using System;
using KouXiaGu.Terrain3D;
using KouXiaGu.World;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu.Resources
{

    /// <summary>
    /// 游戏数据,开始游戏前需要读取的资源;
    /// </summary>
    [Serializable]
    public class BasicResourceInitializer : AsyncInitializer<BasicResource>
    {
        [SerializeField]
        TerrainResource terrain;

        public BasicResource basicResource { get; private set; }

        public override string Prefix
        {
            get { return "游戏基础资源"; }
        }

        public void Start()
        {
            StartInitialize();
            Initialize();
        }

        void Initialize()
        {
            BasicResourceSerializer reader = new BasicResourceSerializer();
            ThreadDelegateOperation<BasicResource> operation = new ThreadDelegateOperation<BasicResource>(() => reader.Read());
            operation.Subscribe(this, OnWorldResourceCompleted, OnFaulted);
        }

        void OnWorldResourceCompleted(IAsyncOperation<BasicResource> operation)
        {
            basicResource = operation.Result;
            string log = GetWorldResourceLog(basicResource.BasicTerrain);
            Debug.Log(log);
            Initialize1();
        }

        string GetWorldResourceLog(BasicTerrainResource item)
        {
            string str =
                "[基础资源]"
               + "\nLandform:" + item.Landform.Count
               + "\nRoad:" + item.Road.Count
               + "\nBuilding:" + item.Building.Count;
            return str;
        }

        void Initialize1()
        {
            IAsyncOperation[] missions = new IAsyncOperation[]
            {
                    terrain.Init(basicResource.BasicTerrain).Subscribe(this, OnTerrainCompleted, OnFaulted),
            };
            (missions as IEnumerable<IAsyncOperation>).Subscribe(this, InitializeCompleted, OnFaulted);
        }

        void InitializeCompleted(IList<IAsyncOperation> operations)
        {
            basicResource.Terrain = terrain;
            OnCompleted(operations, basicResource);
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
