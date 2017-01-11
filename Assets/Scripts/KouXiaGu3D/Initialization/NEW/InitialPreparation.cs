using UnityEngine;
using UnityEngine.SceneManagement;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 游戏一开始初始化等待;
    /// </summary>
    [DisallowMultipleComponent]
    public class InitialPreparation : Scheduler
    {

        [SerializeField]
        string nextSceneName = "Start";

        protected override void OnComplete(IOperateAsync operater)
        {
            return;
        }

        protected override void OnCompleteAll()
        {
            SceneManager.LoadScene(nextSceneName);
            Destroy(this);
        }

        protected override void OnFail(IOperateAsync operater)
        {
            Debug.LogError(operater.Ex);
        }
    }

}
