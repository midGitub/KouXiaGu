using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using UnityEngine;
using KouXiaGu.Concurrent;
using System.Threading;

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
                WorldInitialization.Wait(landformUpdaterOperation, state);

                TimeUpdater = new WorldTimeUpdater(world.Components.Time);
                TimeUpdater.Start();
            }
            catch(Exception e)
            {
                Dispose();
                throw e;
            }
        }

        public void Dispose()
        {
            if (LandformUpdater != null)
            {
                LandformUpdater.Stop();
                LandformUpdater = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}
