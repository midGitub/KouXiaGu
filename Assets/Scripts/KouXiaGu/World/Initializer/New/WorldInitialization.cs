using System;
using System.Threading;

namespace KouXiaGu.World
{

    /// <summary>
    /// 对游戏场景初始化,允许在非Unity线程初始化;
    /// </summary>
    public class WorldInitialization : ICompleteWorld, IWorld
    {
        public WorldInitialization(IBasicData basicData)
        {
            if (basicData == null)
                throw new ArgumentNullException("basicData");

            Initialize(basicData);
        }

        public IBasicData BasicData { get; private set; }
        public IWorldData WorldData { get; private set; }
        public IWorldComponents Components { get; private set; }
        public IWorldUpdater Updater { get; private set; }

        void Initialize(IBasicData basicData)
        {
            BasicData = basicData;
            WorldData = new WorldDataInitialization(BasicData);
            Components = new WorldComponentInitialization(BasicData, WorldData);
            Updater = new WorldUpdaterInitialization(this);
        }

        /// <summary>
        /// 异步初始化资源;
        /// </summary>
        public static IAsyncOperation<ICompleteWorld> CreateAsync(IBasicData basicData)
        {
            return new AsyncInitializer(basicData);
        }

        /// <summary>
        /// 异步初始化结构;
        /// </summary>
        class AsyncInitializer : AsyncOperation<ICompleteWorld>
        {
            public AsyncInitializer(IBasicData basicData)
            {
                ThreadPool.QueueUserWorkItem(_ => Initialize(basicData));
            }

            void Initialize(IBasicData basicData)
            {
                try
                {
                    WorldInitialization item = new WorldInitialization(basicData);
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
