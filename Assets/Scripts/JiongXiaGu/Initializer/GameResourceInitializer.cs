using System;
using JiongXiaGu.Terrain3D;
using JiongXiaGu.World;
using System.Collections.Generic;
using UnityEngine;
using JiongXiaGu.Resources;
using System.Threading;
using JiongXiaGu.Concurrent;
using JiongXiaGu.World.Resources;

namespace JiongXiaGu
{

    /// <summary>
    /// 游戏资源;在游戏场景开始时初始化的资源;
    /// </summary>
    public interface IGameResource
    {
        WorldResources Terrain { get; }
    }

    /// <summary>
    /// 游戏数据,开始游戏前需要读取的资源;
    /// </summary>
    public class GameResourceInitializer : AsyncOperation<IGameResource>, IGameResource
    {
        public bool IsInitialized { get; private set; }
        public WorldResources Terrain { get; private set; }

        public void InitializeAsync(IOperationState state)
        {
            if (IsInitialized)
                throw new ArgumentException("已经在初始化中;");

            ThreadPool.QueueUserWorkItem(Initialize_internal, state);
        }

        void Initialize_internal(object s)
        {
            try
            {
                IsInitialized = true;
                IOperationState state = (IOperationState)s;

                WorldResourcesReader terrainReader = new WorldResourcesReader();
                Terrain = terrainReader.Read(state);
                LogResourceInfo(Terrain);

                OnCompleted(this);
            }
            catch (Exception ex)
            {
                OnFaulted(ex);
            }
        }

        string LogResourceInfo(WorldResources terrain)
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
