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
    public class BasicResourceInitializer : AsyncInitializer<BasicResource>
    {
        BasicResource basicResource;

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
            operation.Subscribe(this, InitializeCompleted, OnFaulted);
        }

        void InitializeCompleted(IAsyncOperation<BasicResource> operation)
        {
            basicResource = operation.Result;
            LogResourceInfo(basicResource);
            OnCompleted(basicResource);
        }

        string LogResourceInfo(BasicResource item)
        {
            string str =
                "[地形资源]"
               + "\nLandform:" + item.Terrain.Landform.Count
               + "\nRoad:" + item.Terrain.Road.Count
               + "\nBuilding:" + item.Terrain.Building.Count;

            Debug.Log(str);
            return str;
        }
    }
}
