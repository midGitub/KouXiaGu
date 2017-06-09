using System;
using KouXiaGu.Terrain3D;
using KouXiaGu.World;
using System.Collections.Generic;
using UnityEngine;
using KouXiaGu.Resources;
using System.Threading;
using KouXiaGu.Concurrent;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏数据,开始游戏前需要读取的资源;
    /// </summary>
    public class BasicResourceInitializer : AsyncOperation<IGameResource>, IGameResource
    {
        public TerrainResources Terrain { get; internal set; }
        ISign sign;

        public void InitializeAsync(ISign sign)
        {
            this.sign = sign;
            ThreadPool.QueueUserWorkItem(_ => _Initialize());
        }

        void _Initialize()
        {
            Debug.Log("开始初始化游戏资源;");
            TerrainResourcesFileReader terrainReader = new TerrainResourcesFileReader();
            Terrain = terrainReader.Read(sign);
            LogResourceInfo(Terrain);

            OnCompleted(this);
        }

        string LogResourceInfo(TerrainResources terrain)
        {
            string str =
                "[地形资源]"
               + "\nLandform:" + terrain.Landform.Count
               + "\nRoad:" + terrain.Road.Count
               + "\nBuilding:" + terrain.Building.Count;

            Debug.Log(str);
            return str;
        }
    }
}
