using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiongXiaGu.Terrain3D;
using UnityEngine;
using JiongXiaGu.World.TimeSystem;

namespace JiongXiaGu.World
{


    public class WorldComponentInitialization : IWorldComponents
    {
        public WorldComponentInitialization(IBasicData basicData, IWorldData worldData)
        {
            if (basicData == null)
                throw new ArgumentNullException("basicData");
            if (worldData == null)
                throw new ArgumentNullException("worldData");

            Initialize(basicData, worldData);
        }

        public OLandform Landform { get; private set; }
        public WorldTime Time { get; private set; }

        void Initialize(IBasicData basicData, IWorldData worldData)
        {
            Debug.Log("开始初始化游戏场景组件;");
            Landform = new OLandform();
        }
    }
}
