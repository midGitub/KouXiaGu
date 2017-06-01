using KouXiaGu.Resources;
using System;
using System.Threading;

namespace KouXiaGu.World
{

    /// <summary>
    /// 对游戏场景初始化,允许在非Unity线程初始化;
    /// </summary>
    public class WorldInitialization : IBasicData, ICompleteWorld, IWorld
    {
        WorldInitialization(BasicResource basicResource, WorldInfo worldInfo)
        {
            if (basicResource == null)
                throw new ArgumentNullException("basicResource");
            if (worldInfo == null)
                throw new ArgumentNullException("worldInfo");

            Initialize(basicResource, worldInfo);
        }

        public BasicResource BasicResource { get; private set; }
        public WorldInfo WorldInfo { get; private set; }
        public IWorldData WorldData { get; private set; }
        public IWorldComponents Components { get; private set; }
        public IWorldUpdater Updater { get; private set; }

        IBasicData IWorld.BasicData
        {
            get { return this; }
        }

        void Initialize(BasicResource basicResource, WorldInfo worldInfo)
        {
            BasicResource = basicResource;
            WorldInfo = worldInfo;
            WorldData = new WorldDataInitialization(this);
            Components = new WorldComponentInitialization(this, WorldData);
            Updater = new WorldUpdaterInitialization(this);
        }

        public static IAsyncOperation<ICompleteWorld> CreateAsync(IAsyncOperation<BasicResource> basicResource, IAsyncOperation<WorldInfo> infoReader)
        {
            return new AsyncInitializer(basicResource, infoReader);
        }

        /// <summary>
        /// 异步初始化结构;
        /// </summary>
        class AsyncInitializer : AsyncOperation<ICompleteWorld>
        {
            public AsyncInitializer(IAsyncOperation<BasicResource> basicResource, IAsyncOperation<WorldInfo> infoReader)
            {
                ThreadPool.QueueUserWorkItem(_ => Initialize(basicResource, infoReader));
            }

            void Initialize(IAsyncOperation<BasicResource> resourceReader, IAsyncOperation<WorldInfo> infoReader)
            {
                try
                {
                    while (!infoReader.IsCompleted)
                    {
                    }
                    while (!resourceReader.IsCompleted)
                    {
                    }
                    BasicResource resource = resourceReader.Result;
                    WorldInfo worldInfo = infoReader.Result;
                    WorldInitialization item = new WorldInitialization(resource, worldInfo);
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
