using UnityEngine;

namespace JiongXiaGu.Unity.Schedulers
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UnityFixedUpdateSceneScheduler))]
    internal sealed class UnityFixedUpdateBeforeUpdater : MonoBehaviour
    {
        private UnityFixedUpdateBeforeUpdater()
        {
        }

        private UnityFixedUpdateSceneScheduler synchronousManager;

        private void Awake()
        {
            synchronousManager = GetComponent<UnityFixedUpdateSceneScheduler>();
        }

        private void FixedUpdate()
        {
            synchronousManager.FirstUpdate();
        }
    }
}
