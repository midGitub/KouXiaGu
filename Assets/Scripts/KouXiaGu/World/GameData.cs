using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.World
{

    /// <summary>
    /// 游戏数据,启动程序时读取;
    /// </summary>
    public class GameData
    {

        //public static IAsync<GameData> CreateAsync()
        //{

        //}
 

        /// <summary>
        /// 基础信息;
        /// </summary>
        public WorldElementResource ElementInfo { get; private set; }

        /// <summary>
        /// 地形资源读取;
        /// </summary>
        public TerrainResource Terrain { get; private set; }

        IEnumerator Initialize()
        {
            var elementInfoReader = WorldElementResource.ReadAsync();
            yield return elementInfoReader;
            ElementInfo = elementInfoReader.Result;


        }

        //void Initialize()
        //{
        //    IAsync<WorldElementResource> ElementInfo = WorldElementResource.ReadAsync();
        //    IAsync<WorldElementResource> Terrain = TerrainResource.ReadAsync(ElementInfo);
        //}

    }

}
