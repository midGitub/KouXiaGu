using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using UnityEngine;
using KouXiaGu.Concurrent;

namespace KouXiaGu.World
{


    public class WorldUpdaterInitialization : IWorldUpdater
    {
        public WorldUpdaterInitialization(IWorld world, IOperationState state)
        {
            if (world == null)
                throw new ArgumentNullException("world");

            Initialize(world, state);
        }

        public SceneUpdater LandformUpdater { get; private set; }
        public WorldTimeUpdater TimeUpdater { get; private set; }

        void Initialize(IWorld world, IOperationState state)
        {
            Debug.Log("开始初始化场景更新器;");

            try
            {
                LandformUpdater = new SceneUpdater(world);
                var landformUpdaterOperation = LandformUpdater.Start();
                while (!landformUpdaterOperation.IsCompleted)
                {
                    if (state.IsCanceled)
                        throw new OperationCanceledException();
                }

                TimeUpdater = new WorldTimeUpdater(world.Components.Time);
                TimeUpdater.Start();
            }
            finally
            {
                Dispose();
            }
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
