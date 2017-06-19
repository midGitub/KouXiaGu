using KouXiaGu.Concurrent;
using KouXiaGu.Resources;
using System;
using System.Threading;
using UnityEngine;

namespace KouXiaGu.World
{

    /// <summary>
    /// 对游戏场景初始化,允许在非Unity线程初始化;
    /// </summary>
    public class WorldInitialization : AsyncOperation<IWorldComplete>, IBasicData, IWorldComplete, IWorld
    {
        public WorldInitialization()
        {
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

        public void InitializeAsync(IAsyncOperation<IGameResource> resourceReader, IAsyncOperation<WorldInfo> infoReader, IOperationState state)
        {
            if (resourceReader == null)
                throw new ArgumentNullException("resourceReader");
            if (infoReader == null)
                throw new ArgumentNullException("infoReader");

            ThreadPool.QueueUserWorkItem(_ => Initialize(resourceReader, infoReader, state));
        }

        void Initialize(IAsyncOperation<IGameResource> resourceReader, IAsyncOperation<WorldInfo> infoReader, IOperationState state)
        {
            try
            {
                while (!infoReader.IsCompleted)
                {
                    if (state.IsCanceled)
                        throw new OperationCanceledException();
                }
                if (infoReader.IsFaulted)
                {
                    throw new InvalidOperationException("infoReader 出现异常!" + infoReader.Exception);
                }

                while (!resourceReader.IsCompleted)
                {
                    if (state.IsCanceled)
                        throw new OperationCanceledException();
                }
                if (resourceReader.IsFaulted)
                {
                    throw new InvalidOperationException("resourceReader 出现异常!" + resourceReader.Exception);
                }

                Debug.Log("开始初始化游戏场景;");

                BasicResource = resourceReader.Result;
                WorldInfo = infoReader.Result;
                WorldData = new WorldDataInitialization(this);
                Components = new WorldComponentInitialization(this, WorldData);
                Updater = new WorldUpdaterInitialization(this, state);
                OnCompleted(this);
            }
            catch (Exception ex)
            {
                OnFaulted(ex);
            }
        }
    }
}
