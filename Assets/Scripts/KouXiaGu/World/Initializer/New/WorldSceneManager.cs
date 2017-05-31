using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KouXiaGu.World
{

    /// <summary>
    /// 游戏场景控制类;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class WorldSceneManager
    {
        WorldSceneManager()
        {
        }

        public const string SceneName = "Game";

        static ObservableStart<ICompleteWorld> onWorldInitializeComplted = new ObservableStart<ICompleteWorld>(new ObserverHashSet<IStateObserver<ICompleteWorld>>());

        /// <summary>
        /// 当世界初始化完毕时调用;
        /// </summary>
        public static IObservableStart<ICompleteWorld> OnWorldInitializeComplted
        {
            get { return onWorldInitializeComplted; }
        }

        /// <summary>
        /// 世界异步初始化程序,若正在初始化 或者 初始化完毕则不为Null;
        /// </summary>
        public static IAsyncOperation<ICompleteWorld> WorldInitializer { get; private set; }

        public void LoadScene(IBasicData basicData)
        {
            SceneManager.LoadSceneAsync(SceneName);
            WorldInitializer = WorldInitialization.CreateAsync(basicData);
        }
    }
}
