using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using UnityEngine;

namespace KouXiaGu.World
{


    public class WorldUpdaterInitialization : IWorldUpdater
    {
        public WorldUpdaterInitialization(IWorld world)
        {
            if (world == null)
                throw new ArgumentNullException("world");

            Initialize(world);
        }

        public SceneUpdater LandformUpdater { get; private set; }
        public WorldTimeUpdater TimeUpdater { get; private set; }

        void Initialize(IWorld world)
        {
            Debug.Log("开始初始化场景更新器;");

            LandformUpdater = new SceneUpdater(world);
            var landformUpdaterOperation = LandformUpdater.Start();
            while (!landformUpdaterOperation.IsCompleted)
            {
                if (!WorldSceneManager.IsActivated)
                {
                    throw new OperationCanceledException();
                }
            }

            TimeUpdater = new WorldTimeUpdater(world.Components.Time);
            TimeUpdater.Start();
        }

        public void Dispose()
        {
            if (LandformUpdater != null)
            {
                LandformUpdater.Stop();
                LandformUpdater = null;
            }
        }
    }
}
