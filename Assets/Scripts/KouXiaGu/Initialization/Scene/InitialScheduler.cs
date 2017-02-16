using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 游戏一开始初始化等待;
    /// </summary>
    [DisallowMultipleComponent]
    public class InitialScheduler : OperateWaiter
    {

        [SerializeField]
        string nextSceneName = "Start";

        void Awake()
        {
            var operates = GetComponentsInChildren<IStartOperate>();

            foreach (var operate in operates)
            {
                operate.Initialize();
            }

            StartWait(operates);
        }

        protected override void OnComplete(IOperateAsync operater)
        {
            return;
        }

        protected override void OnCompleteAll()
        {
            SceneManager.LoadScene(nextSceneName);
            Destroy(this);
        }

        protected override void OnFail(IOperateAsync operater, Exception e)
        {
            Debug.LogError(e);
        }
    }

}
