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
    public sealed class SceneCompleteWaiter : InitializerBase
    {
        private IScenePreparationHandle[] preparationHandles;
        private ISceneCompletedHandle[] completedHandles;

        private SceneCompleteWaiter()
        {
        }

        private void Awake()
        {
            preparationHandles = GetComponentsInChildren<IScenePreparationHandle>();
            completedHandles = GetComponentsInChildren<ISceneCompletedHandle>();
        }

        protected override string InitializerName
        {
            get { return "[场景更新等待器]"; }
        }

        protected override Task Initialize_internal(CancellationToken cancellationToken)
        {
            return WhenAll(preparationHandles, preparationHandle => preparationHandle.Prepare(cancellationToken), cancellationToken);
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
        }
    }
}
