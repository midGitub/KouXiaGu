using KouXiaGu.World.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World
{

    public class WorldDataInitialization : IWorldData
    {
        public WorldDataInitialization(IBasicData basicData)
        {
            if (basicData == null)
                throw new ArgumentNullException("basicData");

            Initialize(basicData);
        }

        public GameMap MapData { get; private set; }

        void Initialize(IBasicData basicData)
        {
            Debug.Log("开始初始化游戏场景数据");
            WorldInfo info = basicData.WorldInfo;
            MapData = info.MapReader.Read(basicData.BasicResource);
        }
    }
}
