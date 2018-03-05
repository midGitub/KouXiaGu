using UnityEngine;

namespace JiongXiaGu.Unity.Schedulers
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UnityFixedUpdateSceneScheduler))]
    internal sealed class UnityFixedUpdateAfterUpdater : MonoBehaviour
    {
        private UnityFixedUpdateAfterUpdater()
        {
        }

        private UnityFixedUpdateSceneScheduler synchronousManager;

        private void Awake()
        {
            synchronousManager = GetComponent<UnityFixedUpdateSceneScheduler>();
        }

        private void FixedUpdate()
        {
            synchronousManager.FinalUpdate();
        }
    }
}
