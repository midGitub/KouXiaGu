using JiongXiaGu.Unity.Archives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{

    /// <summary>
    /// 场景数据初始化处置器;
    /// </summary>
    public interface ISceneDataInitializeHandle
    {
        /// <summary>
        /// 进行初始化,
        /// </summary>
        /// <param name="sceneData">场景数据;</param>
        /// <param name="token">取消标记;</param>
        Task Initialize(SceneArchivalData sceneData, CancellationToken token);

        /// <summary>
        /// 从存档读取状态信息;
        /// </summary>
        /// <param name="archive">需要读取的存档;</param>
        /// <param name="archivalData">需要保存到的游戏状态;</param>
        /// <param name="cancellationToken">取消标记;</param>
        Task Read(IArchiveFileInfo archive, SceneArchivalData archivalData, CancellationToken cancellationToken);
    }

    /// <summary>
    /// 场景数据初始化;
    /// </summary>
    internal sealed class SceneDataInitializer : InitializerBase<SceneDataInitializer>
    {
        static bool isInitializing;
        static Archive archive;
        static SceneArchivalData archivalData;

        /// <summary>
        /// 在未指定任何初始化变量时,是否允许使用默认的场景数据?
        /// </summary>
        [SerializeField]
        bool canUseDefaultSceneArchivalData;

        /// <summary>
        /// 存档信息,在初始化之前赋值,若为Null则表示不通过存档初始化;
        /// </summary>
        public static Archive Archive
        {
            get { return archive; }
            set
            {
                if (isInitializing)
                    throw new InvalidOperationException(string.Format("在初始化时无法变更变量[{0}];", nameof(Archive)));

                archive = value;
            }
        }

        /// <summary>
        /// 场景信息,在初始化之前赋值,若不为Null则从此信息初始化;
        /// </summary>
        public static SceneArchivalData ArchivalData
        {
            get { return archivalData; }
            set
            {
                if (isInitializing)
                    throw new InvalidOperationException(string.Format("在初始化时无法变更变量[{0}];", nameof(ArchivalData)));

                archivalData = value;
            }
        }

        protected override string InitializerName
        {
            get { return "场景数据初始化"; }
        }

        protected override void Awake()
        {
            base.Awake();
            //StartCoroutine(WaitInitializers(Initialize, ModDataInitializer.Instance));
        }

        private void Initialize()
        {
            ISceneDataInitializeHandle[] initializers = GetComponentsInChildren<ISceneDataInitializeHandle>();
            initializeCancellation = new CancellationTokenSource();

            isInitializing = true;
            Task task;
            if (ArchivalData != null)
            {
                task = Initialize(initializers, ArchivalData);
            }
            else if (Archive != null)
            {
                task = Initialize(initializers, Archive);
            }
            else if (canUseDefaultSceneArchivalData)
            {
                var defaultValue = new DefaultSceneArchivalData();
                task = Initialize(initializers, defaultValue.Value);
            }
            else
            {
                Exception ex = new InvalidOperationException("未指定初始化方法;");
                task = Task.FromException(ex);
            }
            initializeTask = task.ContinueWith(OnInitializeTaskCompleted);
            isInitializing = false;
        }

        /// <summary>
        /// 从存档进行初始化;
        /// </summary>
        private async Task Initialize(ISceneDataInitializeHandle[] initializers, IArchiveFileInfo archive)
        {
            SceneArchivalData sceneData = new SceneArchivalData();
            await WhenAll(initializers, initializer => initializer.Read(archive, sceneData, initializeCancellation.Token));
            await Initialize(initializers, sceneData);
        }

        /// <summary>
        /// 从场景数据进行初始化;
        /// </summary>
        private Task Initialize(ISceneDataInitializeHandle[] initializers, SceneArchivalData sceneData)
        {
            return WhenAll(initializers, initializer => initializer.Initialize(sceneData, initializeCancellation.Token));
        }
    }
}
