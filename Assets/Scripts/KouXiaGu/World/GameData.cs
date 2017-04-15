using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using KouXiaGu.World;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏数据,开始游戏前需要读取的资源;
    /// </summary>
    public class GameData
    {

        public static IAsyncOperation<GameData> CreateAsync()
        {
            return new GameDataCreater();
        }

        /// <summary>
        /// 基础信息;
        /// </summary>
        public WorldElementResource ElementInfo { get; private set; }

        /// <summary>
        /// 地形资源;
        /// </summary>
        public TerrainResource Terrain { get; private set; }

        GameData()
        {
        }

        class GameDataCreater : AsyncOperation<GameData>
        {
            public GameDataCreater()
            {
                data = new GameData();
                Initialize();
            }

            GameData data;

            void Initialize()
            {
                WorldElementResource.ReadAsync().Subscribe(delegate (IAsyncOperation<WorldElementResource> result)
                {
                    data.ElementInfo = result.Result;

                    var terrainReader = TerrainResource.ReadAsync(data.ElementInfo);
                    terrainReader.Subscribe(delegate (IAsyncOperation<TerrainResource> terrainResult)
                    {
                        data.Terrain = terrainResult.Result;
                        OnCompleted(data);
                    }, OnFaulted);

                }, OnFaulted);
            }

            void OnFaulted<T>(IAsyncOperation<T> operation)
            {
                OnFaulted(operation.Exception);
            }

        }

    }

}
