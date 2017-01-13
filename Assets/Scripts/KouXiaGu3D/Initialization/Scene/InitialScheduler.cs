using UnityEngine;
using UnityEngine.SceneManagement;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 游戏一开始初始化等待;
    /// </summary>
    [DisallowMultipleComponent]
    public class InitialScheduler : StartOperateWaiter
    {

        [SerializeField]
        string nextSceneName = "Start";

        void Awake()
        {
            StartWait(GetComponentsInChildren<IStartOperate>());
        }

        protected override void OnComplete(IStartOperate operater)
        {
            return;
        }

        protected override void OnCompleteAll()
        {
            SceneManager.LoadScene(nextSceneName);
            Destroy(this);
        }

        protected override void OnFail(IStartOperate operater)
        {
            Debug.LogError(operater.Ex);
        }
    }

}
