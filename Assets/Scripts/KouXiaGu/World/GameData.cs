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

        public static IAsyncOperation<GameData> Create()
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

        public string ToLog()
        {
            string str = 
                ElementInfo.ToLog() +
                Terrain.ToLog();

            return str;
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
                    }, OnError);

                }, OnError);
            }

            void OnError<T>(IAsyncOperation<T> operation)
            {
                Debug.LogError(operation.Ex);
                OnError(operation.Ex);
            }

        }

    }

}
