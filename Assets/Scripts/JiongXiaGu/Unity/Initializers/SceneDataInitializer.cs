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

        ///// <summary>
        ///// 进行初始化,
        ///// </summary>
        ///// <param name="archive">存档数据;</param>
        ///// <param name="token">取消标记;</param>
        //Task Initialize(IArchiveFileInfo archive, CancellationToken token);
    }

    /// <summary>
    /// 场景数据初始化;
    /// </summary>
    public sealed class SceneDataInitializer : InitializerBase
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
        /// 初始化处置器;
        /// </summary>
        private ISceneDataInitializeHandle[] initializers;

        private SceneDataInitializer()
        {
        }

        private void Awake()
        {
            initializers = GetComponentsInChildren<ISceneDataInitializeHandle>();
        }

        /// <summary>
        /// 存档信息,在初始化之前赋值,若为Null则表示不通过存档初始化;
        /// </summary>
        public static Archive Archive
        {
            get { return archive; }
            set
            {
                if (isInitializing)
                    throw new InvalidOperationException("在初始化时无法变更;");
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
                    throw new InvalidOperationException("在初始化时无法变更;");
                archivalData = value;
            }
        }

        protected override string InitializerName
        {
            get { return "[场景数据初始化]"; }
        }

        protected override  Task Initialize_internal(CancellationToken cancellationToken)
        {
            isInitializing = true;
            if (ArchivalData != null)
            {
                return Initialize(ArchivalData, cancellationToken);
            }
            else if (Archive != null)
            {
                return Initialize(Archive, cancellationToken);
            }
            else if (canUseDefaultSceneArchivalData)
            {
                var defaultValue = new DefaultSceneArchivalData();
                return Initialize(defaultValue.Value, cancellationToken);
            }

            Exception ex = new InvalidOperationException("未指定初始化方法;");
            OnFaulted(ex);
            throw ex;
        }

        /// <summary>
        /// 从存档进行初始化;
        /// </summary>
        private async Task Initialize(Archive archive, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //await WhenAll(initializers, initializer => initializer.Initialize(archive, cancellationToken));
            //isInitializing = false;
        }

        /// <summary>
        /// 从场景数据进行初始化;
        /// </summary>
        private async Task Initialize(SceneArchivalData sceneData, CancellationToken cancellationToken)
        {
            await WhenAll(initializers, initializer => initializer.Initialize(sceneData, cancellationToken));
            isInitializing = false;
        }
    }
}
