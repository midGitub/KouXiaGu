using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.World
{

    /// <summary>
    /// 游戏数据,开始游戏前需要读取的资源;
    /// </summary>
    public class GameData
    {

        public static IAsyncOperation<GameData> Create()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 基础信息;
        /// </summary>
        public WorldElementResource ElementInfo { get; private set; }

        /// <summary>
        /// 地形资源;
        /// </summary>
        public TerrainResource Terrain { get; private set; }

        void Initialize()
        {
            WorldElementResource.ReadAsync().Subscribe(delegate (IAsyncOperation<WorldElementResource> result)
            {
                ElementInfo = result.Result;
                TerrainResource.ReadAsync(ElementInfo).Subscribe(terrainResult => Terrain = terrainResult.Result, OnError);
            },OnError);
        }

        void OnError<T>(IAsyncOperation<T> operation)
        {

        }

    }

}
