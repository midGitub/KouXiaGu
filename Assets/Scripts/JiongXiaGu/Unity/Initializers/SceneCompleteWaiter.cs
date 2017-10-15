using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{

    /// <summary>
    /// 场景准备处置;
    /// </summary>
    public interface IScenePreparationHandle
    {
        Task Prepare(CancellationToken token);
    }

    /// <summary>
    /// 场景完成处置;
    /// </summary>
    public interface ISceneCompletedHandle
    {
        void OnSceneCompleted();
    }

    /// <summary>
    /// 场景更新等待;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class SceneCompleteWaiter : InitializerBase<SceneCompleteWaiter>
    {
        ISceneCompletedHandle[] completedHandles;

        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(WaitInitializers(Initialize, SceneComponentInitializer.Instance));
        }

        protected override string InitializerName
        {
            get { return "场景更新等待器"; }
        }

        private void Initialize()
        {
            IScenePreparationHandle[] initializers = GetComponentsInChildren<IScenePreparationHandle>();
            completedHandles = GetComponentsInChildren<ISceneCompletedHandle>();
            initializeCancellation = new CancellationTokenSource();
            Task task = WhenAll(initializers, initializer => initializer.Prepare(initializeCancellation.Token));
            initializeTask = task.ContinueWith(OnInitializeTaskCompleted);
        }

        protected override void OnCompleted()
        {
            base.OnCompleted();
            OnSceneCompleted();
        }

        private void OnSceneCompleted()
        {
            Debug.Log("场景更新完成,即将开始场景;");
            foreach (var completedHandle in completedHandles)
            {
                completedHandle.OnSceneCompleted();
            }
            Debug.Log("场景开始;");
        }
    }
}
