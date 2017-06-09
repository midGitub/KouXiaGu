using KouXiaGu.Resources;
using System;
using System.Threading;
using UnityEngine;

namespace KouXiaGu.World
{

    /// <summary>
    /// 对游戏场景初始化,允许在非Unity线程初始化;
    /// </summary>
    public class WorldInitialization : IBasicData, IWorldComplete, IWorld
    {
        WorldInitialization(IGameResource basicResource, WorldInfo worldInfo)
        {
            if (basicResource == null)
                throw new ArgumentNullException("basicResource");
            if (worldInfo == null)
                throw new ArgumentNullException("worldInfo");

            Initialize(basicResource, worldInfo);
        }

        public IGameResource BasicResource { get; private set; }
        public WorldInfo WorldInfo { get; private set; }
        public IWorldData WorldData { get; private set; }
        public IWorldComponents Components { get; private set; }
        public IWorldUpdater Updater { get; private set; }

        IBasicData IWorld.BasicData
        {
            get { return this; }
        }

        void Initialize(IGameResource basicResource, WorldInfo worldInfo)
        {
            try
            {
                Debug.Log("开始初始化游戏场景;");
                BasicResource = basicResource;
                WorldInfo = worldInfo;
                WorldData = new WorldDataInitialization(this);
                Components = new WorldComponentInitialization(this, WorldData);
                Updater = new WorldUpdaterInitialization(this);
            }
            catch (Exception ex)
            {
                Dispose();
                throw ex;
            }
        }

        public static IAsyncOperation<IWorldComplete> CreateAsync(IAsyncOperation<IGameResource> basicResource, IAsyncOperation<WorldInfo> infoReader)
        {
            return new AsyncInitializer(basicResource, infoReader);
        }

        public void Dispose()
        {
            if (Updater != null)
            {
                Updater.Dispose();
                Updater = null;
            }
        }

        /// <summary>
        /// 异步初始化结构;
        /// </summary>
        class AsyncInitializer : AsyncOperation<IWorldComplete>
        {
            public AsyncInitializer(IAsyncOperation<IGameResource> basicResource, IAsyncOperation<WorldInfo> infoReader)
            {
                if (basicResource == null)
                    throw new ArgumentNullException("basicResource");
                if (infoReader == null)
                    throw new ArgumentNullException("infoReader");

                ThreadPool.QueueUserWorkItem(_ => Initialize(basicResource, infoReader));
            }

            void Initialize(IAsyncOperation<IGameResource> resourceReader, IAsyncOperation<WorldInfo> infoReader)
            {
                try
                {
                    while (!infoReader.IsCompleted)
                    {
                    }
                    while (!resourceReader.IsCompleted)
                    {
                    }
                    var resource = resourceReader.Result;
                    var worldInfo = infoReader.Result;
                    var item = new WorldInitialization(resource, worldInfo);
                    OnCompleted(item);
                }
                catch (Exception ex)
                {
                    OnFaulted(ex);
                }
            }
        }
    }
}
