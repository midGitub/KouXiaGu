using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using UnityEngine;

namespace KouXiaGu.World
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

        public Landform Landform { get; private set; }
        public WorldTime Time { get; private set; }

        void Initialize(IBasicData basicData, IWorldData worldData)
        {
            Debug.Log("开始初始化游戏场景组件;");
            Landform = new Landform();
            Time = new WorldTime(worldData);
        }
    }
}
